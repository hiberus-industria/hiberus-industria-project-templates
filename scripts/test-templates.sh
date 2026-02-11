#!/usr/bin/env bash
set -e  # Exit on any error

echo "=== Template Testing Script ==="
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
echo "Instantiating template 'aspire-react'..."
dotnet new aspire-react -n TestProject -o "$TEST_OUTPUT"
echo ""

# Navigate to the generated project
cd "$TEST_OUTPUT"

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore
echo ""

# Build the project
echo "Building project..."
dotnet build --no-restore
echo ""

echo "=== Template testing completed successfully! ==="
