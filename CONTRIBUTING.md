# Contributing to Hiberus Industria Project Templates

Thank you for your interest in contributing to this project! We welcome contributions from the community.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How to Contribute](#how-to-contribute)
- [Development Workflow](#development-workflow)
- [Commit Message Guidelines](#commit-message-guidelines)
- [Pull Request Process](#pull-request-process)
- [Template Development Guidelines](#template-development-guidelines)
- [Testing](#testing)

## Code of Conduct

This project adheres to professional and respectful collaboration standards. We expect all contributors to:

- Be respectful and inclusive
- Accept constructive criticism gracefully
- Focus on what is best for the community
- Show empathy towards other community members

## Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/hiberus-industria-project-templates.git
   cd hiberus-industria-project-templates
   ```
3. **Add upstream remote**:
   ```bash
   git remote add upstream https://github.com/hiberus-industria/hiberus-industria-project-templates.git
   ```
4. **Install prerequisites**:
   - .NET 9+ SDK
   - Docker Desktop (for testing templates)
   - Node.js (for frontend templates)

## How to Contribute

### Reporting Bugs

- Use the [GitHub Issues](https://github.com/hiberus-industria/hiberus-industria-project-templates/issues) tracker
- Check if the issue already exists before creating a new one
- Include:
  - Clear description of the problem
  - Steps to reproduce
  - Expected vs actual behavior
  - .NET SDK version, OS, Docker version
  - Generated project structure (if applicable)

### Requesting Features

- Use GitHub Issues with the "enhancement" label
- Describe the use case and benefits
- Consider if this fits the project's scope (enterprise .NET templates)

### Submitting Changes

- Fix bugs, improve documentation, or add features
- Follow the development workflow below

## Development Workflow

### Branch Strategy

- `main` - Stable releases, protected
- `develop` - Integration branch (if used)
- `feature/*` - New features or templates
- `fix/*` - Bug fixes
- `docs/*` - Documentation improvements

### Making Changes

1. **Create a feature branch** from `main`:
   ```bash
   git checkout main
   git pull upstream main
   git checkout -b feature/my-new-feature
   ```

2. **Make your changes**:
   - Keep commits focused and atomic
   - Write clear commit messages (see guidelines below)
   - Test your changes locally

3. **Build and test**:
   ```bash
   # Build the template pack
   dotnet pack -c Release -o ./artifacts

   # Test template installation
   dotnet new install ./artifacts/Hiberus.Industria.Templates.*.nupkg

   # Generate a test project
   dotnet new aspire-react -n TestProject -o ./test-output

   # Verify the generated project builds
   cd test-output/TestProject
   dotnet build
   dotnet run --project src/TestProject.AppHost
   ```

4. **Run validation scripts**:
   ```bash
   # Validate template configurations
   ./scripts/validate-template-config.sh

   # Test all templates
   ./scripts/test-templates.sh
   ```

5. **Commit your changes**:
   ```bash
   git add .
   git commit -m "feat: add new database migration feature"
   ```

6. **Push to your fork**:
   ```bash
   git push origin feature/my-new-feature
   ```

## Commit Message Guidelines

We use [Conventional Commits](https://www.conventionalcommits.org/) for automated versioning and changelog generation.

### Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- `feat:` - New feature (MINOR version bump)
- `fix:` - Bug fix (PATCH version bump)
- `docs:` - Documentation only
- `style:` - Formatting, missing semicolons, etc.
- `refactor:` - Code change that neither fixes a bug nor adds a feature
- `test:` - Adding or updating tests
- `chore:` - Maintenance tasks, dependencies

### Breaking Changes

For breaking changes, include `BREAKING CHANGE:` in the footer (MAJOR version bump):

```
feat(template): change default database to PostgreSQL

BREAKING CHANGE: Projects generated with this template now default to PostgreSQL instead of SQL Server. Migration required for existing projects.
```

### Examples

```
feat(aspire-react): add authentication scaffolding
fix(template): correct namespace resolution in template.json
docs: update installation instructions in README
chore: upgrade to .NET 9.0
```

### Scope

Optional, use template name or component:
- `aspire-react`
- `template`
- `ci`
- `scripts`

## Pull Request Process

1. **Ensure your branch is up to date**:
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Create a Pull Request** on GitHub:
   - Use a clear, descriptive title
   - Reference related issues (`Fixes #123`)
   - Describe what changes were made and why
   - Include testing steps

3. **PR Review**:
   - Address reviewer feedback
   - Keep the PR focused (one feature/fix per PR)
   - Be patient and responsive

4. **Automated Checks**:
   - CI build must pass
   - Template validation must succeed
   - All tests must pass

5. **Merge**:
   - Squash commits if there are many small commits
   - Maintainers will merge when approved

## Template Development Guidelines

### Adding a New Template

1. **Create template directory**:
   ```
   templates/
     Hiberus.Industria.Templates.YourTemplate/
       .template.config/
         template.json
       README.md
       <template files>
   ```

2. **Configure template.json**:
   ```json
   {
     "$schema": "http://json.schemastore.org/template",
     "author": "Hiberus Industria",
     "classifications": ["Web", "Aspire", "..."],
     "identity": "Hiberus.Industria.Templates.YourTemplate",
     "name": "Hiberus Industria Your Template",
     "shortName": "your-template",
     "tags": {
       "language": "C#",
       "type": "project"
     },
     "sourceName": "Hiberus.Industria.Templates.YourTemplate",
     "symbols": {
       "name": {
         "type": "parameter",
         "datatype": "string",
         "isRequired": true,
         "description": "The name of the project"
       }
     }
   }
   ```

3. **Add template README**:
   - Describe what the template generates
   - List prerequisites
   - Provide usage instructions
   - Document parameters

4. **Test thoroughly**:
   - Generate projects with different parameter values
   - Ensure generated projects build and run
   - Test on multiple platforms (Windows, macOS, Linux)

### Modifying Existing Templates

- **Maintain backwards compatibility** when possible
- If breaking changes are necessary, document them clearly
- Update the template's README
- Consider migration path for existing users

### Template Best Practices

- Use meaningful default values
- Provide parameter descriptions
- Include comprehensive README in generated projects
- Follow .NET naming conventions
- Use conditional inclusions for optional features
- Test generated projects in isolation
- Document template parameters clearly

## Testing

### Local Testing

1. **Build the package**:
   ```bash
   dotnet pack -c Release -o ./artifacts
   ```

2. **Install locally**:
   ```bash
   dotnet new uninstall Hiberus.Industria.Templates || true
   dotnet new install ./artifacts/Hiberus.Industria.Templates.*.nupkg
   ```

3. **Generate test project**:
   ```bash
   dotnet new aspire-react -n TestCompany.TestProject -o ../test-projects/test1
   ```

4. **Verify generated project**:
   ```bash
   cd ../test-projects/test1/TestCompany.TestProject
   dotnet restore
   dotnet build
   dotnet run --project src/TestCompany.TestProject.AppHost
   ```

5. **Test with different parameters**:
   ```bash
   # Test with different names
   dotnet new aspire-react -n MyCompany.MyApp -o ../test-projects/test2

   # Test with special characters
   dotnet new aspire-react -n "My-App" -o ../test-projects/test3
   ```

### Automated Testing

- CI runs on every push and PR
- Scripts in `scripts/` directory validate templates
- All checks must pass before merge

### Clean Up

```bash
# Uninstall test package
dotnet new uninstall Hiberus.Industria.Templates

# Remove test projects
rm -rf ../test-projects
```

## Questions?

- Open an issue for questions
- Check existing issues and documentation
- Contact maintainers if needed

Thank you for contributing! 🚀
