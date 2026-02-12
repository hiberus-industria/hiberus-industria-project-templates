## Phase 4 Complete: Release Workflow - NuGet Publishing and GitHub Releases

This phase automated the complete release process with push to NuGet.org and GitHub release creation. When code is merged to main, the workflow automatically versions, builds, publishes to NuGet, creates git tags, and generates GitHub releases with changelogs based on conventional commits.

**Files created/changed:**

- .github/workflows/release.yml
- README.md

**Functions created/changed:**

- Release workflow jobs: checkout, setup, tools, gitversion, pack, nuget-push, git-tag, github-release
- README badges: NuGet version, NuGet downloads, CI status, License

**Tests created/changed:**

- Workflow YAML syntax validation (implicit via GitHub Actions)
- Manual testing approach documented for user
- Dry-run test plan: merge to main after secret configuration

**Review Status:** APPROVED

**Manual Setup Required:**

⚠️ **IMPORTANT**: Before the release workflow can function, you must configure the `NUGET_API_KEY` secret:

1. Go to: https://github.com/hiberus-industria/hiberus-industria-project-templates/settings/secrets/actions
2. Click "New repository secret"
3. Name: `NUGET_API_KEY`
4. Value: Your NuGet.org API key (get from https://www.nuget.org/account/apikeys)
5. Click "Add secret"

**How It Works:**

When you merge code to main:

1. GitVersion calculates semantic version from commits (e.g., `0.1.0`)
2. Package is built with that version
3. Package is pushed to NuGet.org (skips if already exists)
4. Git tag `v0.1.0` is created and pushed
5. GitHub release is created with auto-generated changelog
6. README badges update automatically

**Git Commit Message:**

```
feat: Add release workflow for automated NuGet publishing

- Create GitHub Actions release workflow triggered on main branch merges
- Automate NuGet package publishing with duplicate-skip protection
- Generate git tags automatically in v{version} format
- Create GitHub releases with auto-generated changelogs from commits
- Add NuGet and CI status badges to README for visibility
```
