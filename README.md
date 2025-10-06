# JSonGenerateTest

## GitHub Actions Workflows

### Create Sample PR
A workflow that demonstrates using `gh pr create` to automatically create a pull request.

**To run this workflow:**
1. Go to the "Actions" tab in your GitHub repository
2. Select "Create Sample PR" from the workflows list
3. Click "Run workflow"
4. The workflow will create a new branch, add a sample file, and create a PR

**What it does:**
- Creates a new branch with a timestamp
- Adds a sample SAMPLE.md file
- Commits the changes
- Creates a pull request using `gh pr create`