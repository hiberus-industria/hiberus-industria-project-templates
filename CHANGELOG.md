# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

> **Note:** This changelog is automatically generated from Git tags and GitHub Releases.
> For the complete history, see [GitHub Releases](https://github.com/hiberus-industria/hiberus-industria-project-templates/releases).

## [Unreleased]

### Added
- Initial release preparation
- Documentation and metadata

## Versioning Strategy

This project follows **Semantic Versioning**:

- **MAJOR** version: Breaking changes to template structure or required parameters
- **MINOR** version: New templates, new features, backwards-compatible changes
- **PATCH** version: Bug fixes, documentation updates, template refinements

## Commit Convention

We use [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` - New feature or template (triggers MINOR version bump)
- `fix:` - Bug fix (triggers PATCH version bump)
- `docs:` - Documentation only changes
- `chore:` - Maintenance tasks, dependency updates
- `BREAKING CHANGE:` - Breaking changes (triggers MAJOR version bump)

## Release Process

1. Commits are pushed to `main` branch
2. CI/CD analyzes commit messages using GitVersion
3. New version is calculated based on conventional commits
4. Release is created with auto-generated changelog
5. Package is published to NuGet

---

[Unreleased]: https://github.com/hiberus-industria/hiberus-industria-project-templates/compare/HEAD...HEAD
