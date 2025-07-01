# How to Contribute

Depending on your workflow, you can contribute in one of three ways:

## 1. Contributing via Bash (Terminal)

  Fork the repository on GitHub (via website)
  
```bash
# Clone your fork locally
git clone https://github.com/USERNAME/repository-name.git
cd repository-name

# Create a new branch for your changes
git checkout -b branch-name

# Make changes to code or files

# Add changed files to the staging area
git add .

# Commit changes with a descriptive message
git commit -m "Description of your changes"

# Push the branch to your fork
git push origin branch-name
```

  Go to GitHub and create a Pull Request from this branch

## 2. Contributing via GitHub Website

  1. Open the repository you want to contribute to.

  2. Click the Fork button in the top-right corner to create your copy of the repository.

  3. In your fork, find the file you want to edit.

  4. Click the pencil icon (✏️) to edit the file directly on the site.

  5. Make the necessary changes.

  6. Scroll down to the Commit changes section at the bottom.

  7. Write a short description of your changes and click Commit changes.

  8. GitHub will then offer you to create a Pull Request — click Compare & pull request.

  9. Describe your changes and submit the Pull Request for review

## 3. Contributing via GitHub Desktop

  1. Fork the repository via the GitHub website.

  2. Open GitHub Desktop.

  3. Clone your fork using GitHub Desktop.

  4. Create a new branch in GitHub Desktop (Branch > New Branch).

  5. Open the project in your preferred editor and make changes.

  6. Switch back to GitHub Desktop and enter a commit message at the bottom.

  7. Click Commit to [branch].

  8. Click Push origin to upload changes to your fork.

  9. In GitHub Desktop, click Create Pull Request — this will open the GitHub page for PR creation.

  10. Describe your changes and submit the Pull Request.

## Code Style Guidelines

- We follow the official [Microsoft C# coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Please use consistent indentation, meaningful names for variables, methods, and classes.
- Format your code according to these guidelines before submitting a Pull Request.
- You can use your IDE’s auto-formatting tools (e.g., Visual Studio’s **Format Document** feature or `dotnet format` CLI tool) to ensure proper formatting.
- If the project uses any linters or style checkers (e.g., StyleCop), please make sure your code complies with their rules.
- If you have suggestions about the coding style, feel free to discuss them in the Pull Request comments.

## Reporting Issues and Suggesting Features

If you find a bug or have an idea for a new feature, please follow these steps:

1. Check the [Issues](https://github.com/your-repo/issues) to see if a similar issue or suggestion already exists.

2. Create a new issue describing the problem or feature request in detail.  
   - For bugs, include steps to reproduce, expected vs actual behavior, and any error messages.  
   - For feature requests, explain what you want to add and why it would be helpful.

3. Be polite and constructive to help us review and respond quickly.


