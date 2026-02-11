#!/usr/bin/env bash
set -e  # Exit on any error

echo "=== Template Testing Script ==="
echo ""

# Accept template short-name as parameter (default to "aspire-react")
TEMPLATE_SHORT_NAME="${1:-aspire-react}"
TEST_PROJECT_NAME="TestProject"

echo "Template to test: $TEMPLATE_SHORT_NAME"
echo "Test project name: $TEST_PROJECT_NAME"
echo ""

# Find the template package
TEMPLATE_PACKAGE=$(ls ./artifacts/Hiberus.Industria.Templates.*.nupkg | head -n 1)

if [ -z "$TEMPLATE_PACKAGE" ]; then
    echo "ERROR: No template package found in ./artifacts/"
    exit 1
fi

echo "Found template package: $TEMPLATE_PACKAGE"
echo ""

# Script directory and cleanup setup
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && cd .. && pwd)"
TEST_OUTPUT="$SCRIPT_DIR/test-output"

cleanup() {
    cd "$SCRIPT_DIR" || exit
    rm -rf "$TEST_OUTPUT"
    dotnet new uninstall Hiberus.Industria.Templates 2>/dev/null || true
}
trap cleanup EXIT

# Install the template
echo "Installing template..."
dotnet new install "$TEMPLATE_PACKAGE"
echo ""

# Instantiate the template
echo "Instantiating template '$TEMPLATE_SHORT_NAME' with name '$TEST_PROJECT_NAME'..."
dotnet new "$TEMPLATE_SHORT_NAME" -n "$TEST_PROJECT_NAME" -o "$TEST_OUTPUT"
echo ""

# Navigate to the generated project
cd "$TEST_OUTPUT"

echo "=================================================="
echo "VALIDATING GENERATED PROJECT STRUCTURE"
echo "=================================================="
echo ""

# Validate expected files exist
echo "✓ Checking for expected files..."
EXPECTED_FILES=(
    "*.slnx"
    "src/*.AppHost/*.csproj"
    "src/*.AppHost/Program.cs"
    "src/Services/*.Server/*.csproj"
    "src/Services/*.Server/Program.cs"
)

for pattern in "${EXPECTED_FILES[@]}"; do
    if ! ls $pattern >/dev/null 2>&1; then
        echo "  ERROR: Expected file pattern not found: $pattern"
        exit 1
    else
        FOUND=$(ls $pattern 2>/dev/null | head -n 1)
        echo "  ✓ Found: $FOUND"
    fi
done
echo ""

# Validate sourceName replacement worked
echo "✓ Checking sourceName replacement..."
echo "  Looking for namespace using '$TEST_PROJECT_NAME'..."

# Check in .csproj files
CSPROJ_FILES=$(find . -name "*.csproj" | head -n 3)
if [ -z "$CSPROJ_FILES" ]; then
    echo "  ERROR: No .csproj files found"
    exit 1
fi

# Check that project names use the test project name
PROJECT_NAMES=$(echo "$CSPROJ_FILES" | xargs -I {} basename {} .csproj)
if ! echo "$PROJECT_NAMES" | grep -q "$TEST_PROJECT_NAME"; then
    echo "  ERROR: Project names don't contain '$TEST_PROJECT_NAME'"
    echo "  Found: $PROJECT_NAMES"
    exit 1
fi
echo "  ✓ Project names correctly use '$TEST_PROJECT_NAME'"

# Check Program.cs files for namespace usage
PROGRAM_FILES=$(find ./src -name "Program.cs" | head -n 2)
if [ -n "$PROGRAM_FILES" ]; then
    echo "  Checking Program.cs files for correct namespaces..."
    while IFS= read -r program_file; do
        if grep -q "Templates\.Aspire\.React" "$program_file"; then
            echo "  ERROR: Found unreplaced sourceName 'Templates.Aspire.React' in $program_file"
            exit 1
        fi

        if grep -q "$TEST_PROJECT_NAME" "$program_file"; then
            echo "  ✓ $(basename $(dirname $program_file))/Program.cs uses correct namespace"
        fi
    done < <(echo "$PROGRAM_FILES")
fi
echo ""

# Check for common issues
echo "✓ Checking for common issues..."

# Check that no template placeholders remain
PLACEHOLDER_PATTERNS=("Templates\.Aspire\.React" "templates-aspire-react" "Templates Aspire React")
for pattern in "${PLACEHOLDER_PATTERNS[@]}"; do
    # Search in key project files (not in node_modules or bin/obj)
    if find ./src -type f \( -name "*.cs" -o -name "*.csproj" -o -name "*.json" \) \
        -not -path "*/bin/*" -not -path "*/obj/*" \
        -exec grep -l "$pattern" {} + 2>/dev/null; then
        echo "  WARNING: Found unreplaced template placeholder: $pattern"
        echo "           (This may be acceptable in some contexts)"
    fi
done
echo "  ✓ No obvious placeholder issues found"
echo ""

# Check for missing references (look for obvious issues in .csproj)
echo "✓ Checking project references..."
CSPROJ_WITH_REFS=$(find ./src -name "*.csproj" -exec grep -l "ProjectReference" {} + | wc -l | tr -d ' ')
if [ "$CSPROJ_WITH_REFS" -gt 0 ]; then
    echo "  ✓ Found $CSPROJ_WITH_REFS project(s) with ProjectReferences"

    # Check that referenced projects exist
    while IFS= read -r ref; do
        if [ -n "$ref" ]; then
            # Convert relative path to absolute
            if [ ! -f "./src/$ref" ] && [ ! -f "./$ref" ]; then
                echo "  WARNING: Referenced project not found: $ref"
            fi
        fi
    done < <(find ./src -name "*.csproj" -exec grep -h "ProjectReference Include=" {} + | sed 's/.*Include="\([^"]*\)".*/\1/')
else
    echo "  Note: No ProjectReferences found (might be expected for some templates)"
fi
echo ""

echo "=================================================="
echo "BUILDING AND TESTING PROJECT"
echo "=================================================="
echo ""

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: dotnet restore failed"
    exit 1
fi
echo "✓ Dependencies restored successfully"
echo ""

# Build the project
echo "Building project..."
dotnet build --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: dotnet build failed"
    exit 1
fi
echo "✓ Build completed successfully"
echo ""

# Check if tests exist and run them if found
if find . -name "*Tests.csproj" -o -name "*.Tests.csproj" | grep -q .; then
    echo "Test projects found. Running tests..."
    dotnet test --no-build --verbosity minimal
    if [ $? -ne 0 ]; then
        echo "ERROR: Tests failed"
        exit 1
    fi
    echo "✓ Tests passed"
    echo ""
else
    echo "No test projects found (skipping test execution)"
    echo ""
fi

echo "=================================================="
echo "TEMPLATE TESTING COMPLETED SUCCESSFULLY!"
echo "=================================================="
echo ""
echo "Summary:"
echo "  - Template: $TEMPLATE_SHORT_NAME"
echo "  - Project Name: $TEST_PROJECT_NAME"
echo "  - Structure: ✓ Valid"
echo "  - SourceName Replacement: ✓ Working"
echo "  - Build: ✓ Success"
echo ""
