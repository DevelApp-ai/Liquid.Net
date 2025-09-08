# CI/CD Pipeline for Liquid.Net

This directory contains the CI/CD pipeline configuration and GitHub templates for the Liquid.Net project.

## Workflows

### 🚀 CI/CD Pipeline (`ci.yml`)
- **Triggers**: Push to main/develop, pull requests, releases
- **Jobs**:
  - **Build & Test**: Runs on Ubuntu, Windows, and macOS
  - **Package**: Creates NuGet packages on releases
  - **Benchmark**: Runs performance benchmarks on main branch pushes

### 🔍 Code Quality (`code-quality.yml`)
- **Triggers**: Push to main/develop, pull requests
- **Jobs**:
  - **Static Analysis**: Format checking, CodeQL analysis
  - **Security Scan**: Vulnerable and deprecated package detection

## Templates

### Pull Request Template
Standardized PR template with checklist for:
- Change description and type
- Testing requirements
- Code quality checks

### Issue Templates
- **Bug Report**: For reporting issues with structured information
- **Feature Request**: For proposing new features with use cases

## Dependabot
Automated dependency updates for:
- NuGet packages (weekly)
- GitHub Actions (weekly)

## Features

### 📊 Multi-Platform Testing
Tests run on Ubuntu, Windows, and macOS to ensure cross-platform compatibility.

### 📦 Automated Packaging
NuGet packages are automatically created on releases and can be published to NuGet.org with proper secrets configuration.

### 🔒 Security Scanning
CodeQL analysis and dependency vulnerability scanning help maintain code security.

### ⚡ Performance Monitoring
Benchmark jobs run automatically to track performance changes over time.

## Configuration

### Secrets Required for Full Functionality
- `NUGET_API_KEY`: For publishing packages to NuGet.org

### Branch Protection Recommendations
- Require PR reviews
- Require CI checks to pass
- Require up-to-date branches
- Include administrators in restrictions