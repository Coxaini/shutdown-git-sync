# Shutdown.Monitor.Git.Tests

This project contains integration tests for the `Shutdown.Monitor.Git` library, which provides functionality for
saving changes to a temporary branch in a Git repository and restoring

## Project Structure

- `GitChangesSenderTests.cs`: Contains tests for the `GitChangesSender` service, which handles saving changes to a
  temporary branch in a Git repository.
- `Common/GitTestBase.cs`: Provides a base class for Git-related tests, including setup and teardown methods for
  creating and cleaning up test branches.

## Prerequisites

- .NET SDK
- LibGit2Sharp library
- A Git repository for testing

## Configuration

The test project uses a configuration file `testsettings.json` to store repository settings. Ensure this file is present
and contains the necessary configuration:

```json
{
  "RepoPath": "path/to/your/repo",
  "Origin": "url of the remote repository",
  "TempBranchPrefix": "temp",
  "UserName": "your-username or 'x-access-token'",
  "Password": "your-password or your-token"
}
```

For `github.com` repositories, you should use a personal access token instead of your password. To create a token, go to
your GitHub account settings, then `Developer settings` -> `Personal access tokens` -> `Generate new token`. The token
should have the `repo` scope.

## Running Tests

To run the tests, use the following command:

```sh
dotnet test
```
