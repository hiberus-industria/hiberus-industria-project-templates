#!/usr/bin/env bash
set -e  # Exit on any error

echo "=== Template Configuration Validation ==="
echo ""

# Check if jq is available for JSON validation
HAS_JQ=false
if command -v jq >/dev/null 2>&1; then
    HAS_JQ=true
    echo "Using jq for JSON validation"
else
    echo "Warning: jq not found, using basic JSON validation"
fi
echo ""

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && cd .. && pwd)"
TEMPLATES_DIR="$SCRIPT_DIR/templates"

# Required fields in template.json
REQUIRED_FIELDS=("identity" "name" "shortName" "tags" "sourceName")

# Find all template.json files
echo "Searching for template configurations..."
TEMPLATE_CONFIGS=$(find "$TEMPLATES_DIR" -type f -name "template.json" -path "*/.template.config/template.json")

if [ -z "$TEMPLATE_CONFIGS" ]; then
    echo "ERROR: No template.json files found in $TEMPLATES_DIR"
    exit 1
fi

echo "Found template configurations:"
while IFS= read -r config; do
    echo "  - $config"
done < <(echo "$TEMPLATE_CONFIGS")
echo ""

# Validate each template.json
while IFS= read -r TEMPLATE_CONFIG; do
    TEMPLATE_NAME=$(dirname "$(dirname "$TEMPLATE_CONFIG")" | xargs basename)
    echo "=================================================="
    echo "Validating: $TEMPLATE_NAME"
    echo "Path: $TEMPLATE_CONFIG"
    echo "=================================================="
    echo ""

    # Check if file exists and is readable
    if [ ! -r "$TEMPLATE_CONFIG" ]; then
        echo "ERROR: Cannot read template.json file"
        exit 1
    fi

    # Validate JSON syntax
    echo "✓ Checking JSON syntax..."
    if [ "$HAS_JQ" = true ]; then
        if ! jq empty "$TEMPLATE_CONFIG" 2>/dev/null; then
            echo "ERROR: Invalid JSON syntax in $TEMPLATE_CONFIG"
            exit 1
        fi
    else
        # Basic JSON validation using python
        if ! python3 -c "import json; json.load(open('$TEMPLATE_CONFIG'))" 2>/dev/null; then
            echo "ERROR: Invalid JSON syntax in $TEMPLATE_CONFIG"
            exit 1
        fi
    fi
    echo "  JSON syntax is valid"
    echo ""

    # Validate required fields
    echo "✓ Checking required fields..."
    for field in "${REQUIRED_FIELDS[@]}"; do
        if [ "$HAS_JQ" = true ]; then
            VALUE=$(jq -r ".$field // empty" "$TEMPLATE_CONFIG")
        else
            VALUE=$(python3 -c "import json; data=json.load(open('$TEMPLATE_CONFIG')); print(data.get('$field', ''))" 2>/dev/null || echo "")
        fi

        if [ -z "$VALUE" ]; then
            echo "  ERROR: Missing required field: $field"
            exit 1
        else
            echo "  ✓ $field: $VALUE"
        fi
    done
    echo ""

    # Validate schema reference
    echo "✓ Checking \$schema field..."
    if [ "$HAS_JQ" = true ]; then
        SCHEMA=$(jq -r '."$schema" // empty' "$TEMPLATE_CONFIG")
    else
        SCHEMA=$(python3 -c "import json; data=json.load(open('$TEMPLATE_CONFIG')); print(data.get('\$schema', ''))" 2>/dev/null || echo "")
    fi

    if [ -z "$SCHEMA" ]; then
        echo "  WARNING: Missing \$schema field (recommended but not required)"
    else
        echo "  ✓ \$schema: $SCHEMA"
    fi
    echo ""

    # Validate symbols section if present
    echo "✓ Checking symbols section..."
    if [ "$HAS_JQ" = true ]; then
        HAS_SYMBOLS=$(jq 'has("symbols")' "$TEMPLATE_CONFIG")
        if [ "$HAS_SYMBOLS" = "true" ]; then
            SYMBOL_COUNT=$(jq '.symbols | length' "$TEMPLATE_CONFIG")
            echo "  ✓ Found $SYMBOL_COUNT symbol(s)"

            # Check each symbol has a type
            SYMBOLS=$(jq -r '.symbols | keys[]' "$TEMPLATE_CONFIG")
            while IFS= read -r symbol; do
                SYMBOL_TYPE=$(jq -r ".symbols.\"$symbol\".type // empty" "$TEMPLATE_CONFIG")
                if [ -z "$SYMBOL_TYPE" ]; then
                    echo "  ERROR: Symbol '$symbol' missing 'type' field"
                    exit 1
                else
                    echo "  ✓ Symbol '$symbol': type=$SYMBOL_TYPE"
                fi
            done < <(echo "$SYMBOLS")
        else
            echo "  No symbols defined (ok)"
        fi
    else
        # Basic check with python
        HAS_SYMBOLS=$(python3 -c "import json; data=json.load(open('$TEMPLATE_CONFIG')); print('symbols' in data)" 2>/dev/null || echo "False")
        if [ "$HAS_SYMBOLS" = "True" ]; then
            SYMBOL_COUNT=$(python3 -c "import json; data=json.load(open('$TEMPLATE_CONFIG')); print(len(data.get('symbols', {})))")
            echo "  ✓ Found $SYMBOL_COUNT symbol(s)"
        else
            echo "  No symbols defined (ok)"
        fi
    fi
    echo ""

    # Check for common misconfigurations
    echo "✓ Checking for common issues..."

    # Check if sourceName matches identity pattern
    if [ "$HAS_JQ" = true ]; then
        IDENTITY=$(jq -r '.identity' "$TEMPLATE_CONFIG")
        SOURCE_NAME=$(jq -r '.sourceName' "$TEMPLATE_CONFIG")

        # sourceName should typically be related to identity
        if [[ ! "$IDENTITY" =~ "$SOURCE_NAME" ]]; then
            echo "  WARNING: sourceName '$SOURCE_NAME' doesn't appear in identity '$IDENTITY'"
            echo "           This may be intentional, but verify it's correct"
        else
            echo "  ✓ sourceName appears consistent with identity"
        fi

        # Check if shortName is lowercase and kebab-case
        SHORT_NAME=$(jq -r '.shortName' "$TEMPLATE_CONFIG")
        if [[ "$SHORT_NAME" =~ [A-Z] ]] || [[ "$SHORT_NAME" =~ [_\ ] ]]; then
            echo "  WARNING: shortName '$SHORT_NAME' contains uppercase or spaces"
            echo "           Recommended: use lowercase kebab-case (e.g., 'my-template')"
        else
            echo "  ✓ shortName follows conventions"
        fi
    fi
    echo ""

    echo "✓ Validation passed for $TEMPLATE_NAME"
    echo ""
done < <(echo "$TEMPLATE_CONFIGS")

# If we got here, all validations passed
echo "=================================================="
echo "ALL TEMPLATE CONFIGURATIONS VALID"
echo "=================================================="
exit 0
