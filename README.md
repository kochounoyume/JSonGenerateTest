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

### Create PR with GraphQL
A workflow that demonstrates using GitHub's GraphQL API to automatically create a pull request.

**To run this workflow:**
1. Go to the "Actions" tab in your GitHub repository
2. Select "Create PR with GraphQL" from the workflows list
3. Click "Run workflow"
4. The workflow will create a new branch, add a sample file, and create a PR using GraphQL

**What it does:**
- Creates a new branch with a timestamp
- Adds a sample GRAPHQL_SAMPLE.md file
- Commits the changes
- Retrieves the repository ID using a GraphQL query
- Creates a pull request using GraphQL `createPullRequest` mutation

### Create PR with REST API
A workflow that demonstrates using GitHub's REST API to automatically create a pull request.

**To run this workflow:**
1. Go to the "Actions" tab in your GitHub repository
2. Select "Create PR with REST API" from the workflows list
3. Click "Run workflow"
4. The workflow will create a new branch, add a sample file, and create a PR using REST API

**What it does:**
- Creates a new branch with a timestamp
- Adds a sample RESTAPI_SAMPLE.md file
- Commits the changes
- Creates a pull request using REST API endpoint `/repos/{owner}/{repo}/pulls`