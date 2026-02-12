## Plan: NuGet Template Pack with CI/CD Automation

This plan converts the project into a publishable NuGet template pack with full automation for versioning, building, testing, and releasing. The solution supports multiple templates (currently React, future Angular+), uses semantic versioning based on conventional commits, and automates NuGet publishing and GitHub releases through workflows.

**Phases: 6**

1. **Phase 1: Template Pack Foundation**
    - **Objective:** Create the core template pack project structure that bundles all templates into a single NuGet package
    - **Files/Functions to Modify/Create:**
        - CREATE: [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj) - Root-level project file for template pack
        - CREATE: [.gitignore](.gitignore) additions for artifacts/ and \*.nupkg
        - UPDATE: Root [README.md](README.md) - Add template pack usage instructions
    - **Tests to Write:**
        - Manual test: `dotnet pack` succeeds and creates .nupkg
        - Manual test: `dotnet new install` with local .nupkg works
        - Manual test: `dotnet new aspire-react -n Test -o ./test-output` creates project
    - **Steps:**
        1. Create [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj) with PackageType=Template, proper Content includes/excludes for templates directory
        2. Configure package metadata (PackageId, Title, Authors, Description, PackageTags, Version)
        3. Set IncludeContentInPack=true, IncludeBuildOutput=false, ContentTargetFolders=content
        4. Add proper exclusions for bin/, obj/, node_modules/, .yarn/
        5. Test local pack: `dotnet pack -c Release -o ./artifacts`
        6. Test local install: `dotnet new install ./artifacts/Hiberus.Industria.ProjectTemplates.*.nupkg`
        7. Test template instantiation: `dotnet new aspire-react -n TestProject -o ./test-react`
        8. Verify generated project builds successfully
        9. Uninstall test: `dotnet new uninstall Hiberus.Industria.ProjectTemplates`

2. **Phase 2: Semantic Versioning with GitVersion**
    - **Objective:** Implement GitVersion for automatic semantic versioning based on git history and conventional commits
    - **Files/Functions to Modify/Create:**
        - CREATE: [GitVersion.yml](GitVersion.yml) - GitVersion configuration
        - CREATE: [.github/release.yml](.github/release.yml) - GitHub release notes categories
        - UPDATE: [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj) - Add VersionPrefix/VersionSuffix support
    - **Tests to Write:**
        - Manual test: Install GitVersion CLI and run `dotnet-gitversion` locally
        - Manual test: Verify version calculated matches expected semantic version
        - Manual test: Create test commits with conventional format and verify version bump
    - **Steps:**
        1. Create [GitVersion.yml](GitVersion.yml) with ContinuousDeployment mode
        2. Configure branch strategies: main (stable), develop (beta), feature (alpha)
        3. Set increment rules: feat=minor, fix=patch, breaking=major
        4. Configure tag prefix (v) and version format
        5. Test GitVersion locally: Install `dotnet tool install --global GitVersion.Tool`
        6. Run `dotnet-gitversion` and verify calculated version
        7. Test with conventional commits: Create feature branch, commit with `feat:`, `fix:`, verify version increments
        8. Create [.github/release.yml](.github/release.yml) for categorizing release notes (features, bugs, chores)
        9. Test pack with calculated version: `dotnet pack -p:Version=$(dotnet-gitversion -showvariable SemVer)`

3. **Phase 3: CI Workflow - Build and Validation**
    - **Objective:** Create GitHub Actions workflow for continuous integration that builds, validates, and tests the template pack on every PR
    - **Files/Functions to Modify/Create:**
        - CREATE: [.github/workflows/ci.yml](.github/workflows/ci.yml) - CI workflow
        - CREATE: [scripts/test-templates.sh](scripts/test-templates.sh) - Template testing script
        - CREATE: [.github/workflows/template-test.yml](.github/workflows/template-test.yml) - Reusable template test workflow
    - **Tests to Write:**
        - Workflow validation: Ensure workflow YAML syntax is correct
        - Test script validation: Run test script locally and verify it catches failures
        - Integration test: Create PR and verify CI workflow runs successfully
    - **Steps:**
        1. Create [.github/workflows/ci.yml](.github/workflows/ci.yml) with trigger on pull_request and push to develop
        2. Add job: checkout with fetch-depth=0 for GitVersion
        3. Add job: Setup .NET using global.json from template directory
        4. Add job: Restore .NET tools from template .config directory
        5. Add job: Install and execute GitVersion, capture SemVer output
        6. Add job: Run `dotnet pack` with calculated version
        7. Add job: Upload artifacts (\*.nupkg) for inspection
        8. Create [scripts/test-templates.sh](scripts/test-templates.sh) bash script
        9. Script step: Install packed template from artifacts
        10. Script step: Instantiate aspire-react template with test parameters
        11. Script step: Restore, build, and verify exit code=0
        12. Script step: Cleanup test output
        13. Add job in ci.yml to execute test script
        14. Create [.github/workflows/template-test.yml](.github/workflows/template-test.yml) as reusable workflow
        15. Test locally: Run script against manual pack
        16. Test CI: Create test PR and verify all jobs pass

4. **Phase 4: Release Workflow - NuGet Publishing and GitHub Releases**
    - **Objective:** Automate NuGet package publishing and GitHub release creation on merges to main
    - **Files/Functions to Modify/Create:**
        - CREATE: [.github/workflows/release.yml](.github/workflows/release.yml) - Release and publish workflow
        - UPDATE: Repository settings for NUGET_API_KEY secret
        - UPDATE: [README.md](README.md) - Add badges for NuGet version and build status
    - **Tests to Write:**
        - Dry-run test: Run workflow with `--skip-duplicate` to verify pack succeeds
        - Secret validation: Ensure NUGET_API_KEY is properly configured
        - End-to-end test: Merge to main and verify successful NuGet publish and GitHub release
    - **Steps:**
        1. Create [.github/workflows/release.yml](.github/workflows/release.yml) with trigger on push to main
        2. Add permissions: contents=write, packages=write for release creation
        3. Add job: Checkout with full history (fetch-depth=0)
        4. Add job: Setup .NET from global.json
        5. Add job: Install and execute GitVersion, capture MajorMinorPatch for tagging
        6. Add job: Run `dotnet pack` with version, output to ./artifacts
        7. Add job: Push to NuGet.org using `dotnet nuget push` with NUGET_API_KEY secret
        8. Add job: Create git tag `v{version}` and push tag
        9. Add job: Create GitHub release using softprops/action-gh-release with auto-generated notes
        10. Add job: Upload .nupkg file as release asset
        11. Configure GitHub repository secret NUGET_API_KEY (get from nuget.org account)
        12. Test release workflow: Create test branch, merge to main, verify NuGet publish
        13. Verify package appears on nuget.org: https://www.nuget.org/packages/Hiberus.Industria.ProjectTemplates
        14. Verify GitHub release created with correct version tag and notes
        15. Update [README.md](README.md) with shields.io badges for NuGet version and CI status

5. **Phase 5: Advanced Template Testing and Validation**
    - **Objective:** Enhance template testing to validate generated projects build, test structure integrity, and verify template parameters
    - **Files/Functions to Modify/Create:**
        - UPDATE: [scripts/test-templates.sh](scripts/test-templates.sh) - Add comprehensive validation
        - CREATE: [scripts/validate-template-config.sh](scripts/validate-template-config.sh) - Validate .template.config files
        - UPDATE: [.github/workflows/ci.yml](.github/workflows/ci.yml) - Add template config validation
    - **Tests to Write:**
        - Test: Validate all .template.config/template.json files have required fields
        - Test: Instantiate template with custom parameters and verify replacements
        - Test: Generated project restores without errors
        - Test: Generated project builds without warnings/errors
        - Test: Verify template exclusions work (no bin/obj in pack)
    - **Steps:**
        1. Update [scripts/test-templates.sh](scripts/test-templates.sh) to accept template short-name parameter
        2. Add validation: Check generated project has expected files (.csproj, Program.cs, etc.)
        3. Add validation: Verify `dotnet restore` succeeds with exit code 0
        4. Add validation: Verify `dotnet build` succeeds with exit code 0
        5. Add validation: Check for common issues (missing references, incorrect namespaces)
        6. Add parameter testing: Instantiate with different parameter values
        7. Add validation: Verify source name replacement worked correctly
        8. Create [scripts/validate-template-config.sh](scripts/validate-template-config.sh)
        9. Script: Find all .template.config/template.json files
        10. Script: Validate required fields (identity, name, shortName, tags, sourceName)
        11. Script: Validate symbols and parameters syntax
        12. Script: Check for common misconfigurations
        13. Update [.github/workflows/ci.yml](.github/workflows/ci.yml) to run validation script
        14. Test validation catches intentional errors in template.json
        15. Run full test suite locally before committing

6. **Phase 6: Documentation, Metadata, and Release Polish**
    - **Objective:** Complete package documentation, add metadata (icon, license), update READMEs, and prepare for public release
    - **Files/Functions to Modify/Create:**
        - UPDATE: Root [README.md](README.md) - Comprehensive template pack documentation
        - CREATE: [LICENSE](LICENSE) - Add license file (MIT or org standard)
        - CREATE: [icon.png](icon.png) - Package icon for NuGet gallery
        - UPDATE: [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj) - Add icon, license, repo URL
        - CREATE: [CONTRIBUTING.md](CONTRIBUTING.md) - Contribution guidelines
        - UPDATE: Template READMEs in [templates/Hiberus.Industria.Templates.Aspire.React/README.md](templates/Hiberus.Industria.Templates.Aspire.React/README.md)
    - **Tests to Write:**
        - Manual review: Verify README instructions are accurate and complete
        - Manual test: Follow installation instructions from scratch
        - Manual test: Verify NuGet package page displays correctly with icon and description
    - **Steps:**
        1. Update root [README.md](README.md) with comprehensive documentation
        2. Add sections: Overview, Installation, Available Templates, Usage Examples, Parameters, Development, Contributing
        3. Include code examples for `dotnet new install` and template usage
        4. Create or update [LICENSE](LICENSE) file (confirm with organization standards)
        5. Create package icon (512x512 PNG with Hiberus Industria branding)
        6. Save icon as [icon.png](icon.png) in root directory
        7. Update [Hiberus.Industria.Templates.csproj](Hiberus.Industria.Templates.csproj) with PackageIcon, PackageLicenseExpression
        8. Add PackageProjectUrl and RepositoryUrl pointing to GitHub repo
        9. Add copyright and additional metadata
        10. Create [CONTRIBUTING.md](CONTRIBUTING.md) with commit conventions, PR process, template development guidelines
        11. Update [templates/Hiberus.Industria.Templates.Aspire.React/README.md](templates/Hiberus.Industria.Templates.Aspire.React/README.md)
        12. Template README: Describe generated project structure, prerequisites, getting started, available parameters
        13. Add CHANGELOG.md (initially empty, will be generated by releases)
        14. Verify pack includes all necessary files: `dotnet pack` and inspect .nupkg with `unzip`
        15. Test complete workflow: Install from NuGet, create project, verify all docs are accurate
        16. Prepare for first release: Create PR to main with all changes

**Decisions Made:**

1. **NuGet Package Naming:** `Hiberus.Industria.Templates` ✅
2. **NuGet Visibility:** Público en NuGet.org ✅
3. **License Selection:** MIT License (industry standard for templates) ✅
4. **Icon/Branding:** Use Hiberus Tecnología logo (Spanish company) ✅
5. **Angular Template:** Wait, not included in first release ✅
6. **Version Strategy:** Start at 0.1.0 (pre-release) ✅
7. **Testing Directory:** Keep in repo (already in .gitignore for local testing) ✅
8. **Conventional Commits:** All collaborators familiar ✅
9. **.NET Tools:** Define in `.config/dotnet-tools.json` at root (avoid global installs) ✅
