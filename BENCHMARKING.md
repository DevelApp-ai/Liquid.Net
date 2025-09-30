# Liquid.Net Benchmarking Guide

This guide explains how to run comprehensive benchmarks for the Liquid.Net framework on your local machine, particularly on Windows 11.

## Prerequisites

### Windows 11 Requirements

1. **.NET 8.0 SDK or later**
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify installation: `dotnet --version`

2. **Git for Windows** (if cloning from repository)
   - Download from: https://git-scm.com/download/win

3. **Visual Studio 2022** or **Visual Studio Code** (optional, for development)
   - Visual Studio 2022: https://visualstudio.microsoft.com/vs/
   - VS Code: https://code.visualstudio.com/

4. **Hardware Recommendations**
   - **CPU**: Multi-core processor (Intel i5/i7 or AMD Ryzen 5/7+)
   - **RAM**: 8GB minimum, 16GB+ recommended for large benchmarks
   - **Storage**: 2GB free space for results and temporary files

## Getting Started

### 1. Clone or Download the Repository

```powershell
# Using Git
git clone https://github.com/DevelApp-ai/Liquid.Net.git
cd Liquid.Net

# Or download ZIP from GitHub and extract
```

### 2. Build the Solution

```powershell
# Restore NuGet packages
dotnet restore

# Build in Release mode for optimal performance
dotnet build --configuration Release
```

### 3. Verify Installation

```powershell
# Run a quick test to verify everything works
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release ci
```

## Benchmark Types

### 1. Standard LNN Corpus Benchmarks

Evaluates against the original test corpus used in academic LNN research:

```powershell
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release
```

**What it tests:**
- Mackey-Glass chaotic time series prediction
- Noisy sine wave continuous learning
- Lorenz attractor dynamical system modeling

**Expected runtime:** 5-15 minutes depending on hardware

### 2. Comparative Analysis

Compares LNN performance against traditional neural networks:

```powershell
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release comparative
```

**What it compares:**
- LSTM (Long Short-Term Memory)
- RNN (Vanilla Recurrent Neural Network)
- ESN (Echo State Network)
- SVM (Support Vector Machine)
- MLP (Multi-Layer Perceptron)
- Linear Regression baseline

**Expected runtime:** 10-30 minutes depending on hardware

### 3. Micro-benchmarks

Detailed performance profiling with BenchmarkDotNet:

```powershell
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release micro
```

**What it measures:**
- Memory allocation patterns
- CPU usage optimization
- Execution time precision
- Garbage collection impact

**Expected runtime:** 15-45 minutes (most comprehensive)

### 4. Complete Benchmark Suite

Runs all benchmark types in sequence:

```powershell
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release all
```

**Expected runtime:** 30-90 minutes depending on hardware

## Understanding Results

### Performance Metrics

The benchmarks report several key metrics:

- **MSE (Mean Squared Error)**: Lower is better, measures prediction accuracy
- **RMSE (Root Mean Squared Error)**: Square root of MSE, same units as target values
- **MAE (Mean Absolute Error)**: Average absolute prediction error
- **NRMSE (Normalized RMSE)**: RMSE normalized by target range
- **R² Score**: Coefficient of determination (1.0 = perfect prediction)

### Benchmark Results Location

Results are saved in several locations:

```
Liquid.Net/
├── BenchmarkDotNet.Artifacts/     # Detailed micro-benchmark results
│   ├── results/
│   └── logs/
├── benchmark-results.txt          # Summary results
└── performance-report.html        # Visual report (if generated)
```

### Sample Result Interpretation

```
Dataset: Mackey-Glass
----------------------------------------
Small-LNN       | MSE: 0.000867 | RMSE: 0.029444 | R²: 0.9856 | Time: 224ms
Medium-LNN      | MSE: 0.000867 | RMSE: 0.029444 | R²: 0.9856 | Time: 132ms
Large-LNN       | MSE: 0.000867 | RMSE: 0.029444 | R²: 0.9856 | Time: 100ms
```

- **Lower MSE/RMSE** = Better accuracy
- **Higher R²** = Better fit (closer to 1.0)
- **Time** = Training time in milliseconds

## Advanced Configuration

### Custom Dataset Sizes

You can modify dataset sizes by editing `src/Liquid.Net.Benchmarks/BenchmarkDatasets.cs`:

```csharp
// For faster testing, reduce dataset sizes
var quickMackeyGlass = StandardDatasets.GenerateMackeyGlass(500); // Default: 1000
var quickSineWave = StandardDatasets.GenerateSineWave(500);       // Default: 1000
var quickLorenz = StandardDatasets.GenerateLorenzAttractor(1000); // Default: 2000
```

### Environment Variables

Configure benchmark behavior:

```powershell
# Set environment variables in PowerShell
$env:DOTNET_BENCHMARK_ITERATIONS = "10"    # Reduce iterations for faster results
$env:DOTNET_BENCHMARK_WARMUP = "5"        # Reduce warmup for development

# Run benchmarks with custom settings
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release
```

## Troubleshooting

### Common Issues

1. **Out of Memory Errors**
   - Reduce dataset sizes in configuration
   - Close other applications
   - Ensure you have sufficient RAM

2. **Long Execution Times**
   - Use `ci` mode for quick tests
   - Run individual benchmark types separately
   - Consider running overnight for complete suite

3. **Build Errors**
   ```powershell
   # Clean and rebuild
   dotnet clean
   dotnet restore
   dotnet build --configuration Release
   ```

4. **Permission Issues**
   - Run PowerShell as Administrator
   - Ensure antivirus isn't blocking execution
   - Check Windows Defender exclusions

### Performance Tips

1. **Close unnecessary applications** before running benchmarks
2. **Use Release configuration** for accurate performance measurements
3. **Disable Windows power saving** modes during benchmarking
4. **Run from SSD** storage for better I/O performance
5. **Monitor system temperature** during extended benchmark runs

## Interpreting Academic Comparison

The benchmarks replicate methodologies from key research papers:

- **Hasani et al. (2020)**: "Liquid Time-constant Networks"
- **Lechner et al. (2020)**: "Neural Circuit Policies Enabling Auditable Autonomy"

Results can be directly compared with published academic benchmarks using the same datasets and metrics.

## Contributing Benchmark Results

To contribute your benchmark results:

1. Run the complete benchmark suite
2. Create a results summary with your hardware specifications
3. Submit via GitHub Issues or Pull Request
4. Include system specifications:
   - CPU model and speed
   - RAM amount and speed
   - Storage type (SSD/HDD)
   - .NET version
   - Windows version

## Quick Reference

```powershell
# Quick test (CI mode)
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release ci

# Show available datasets
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release datasets

# Standard benchmarks
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release

# Comparative analysis
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release comparative

# Micro-benchmarks
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release micro

# Complete suite
dotnet run --project src/Liquid.Net.Benchmarks --configuration Release all
```

For additional help or questions, please visit the [GitHub Issues](https://github.com/DevelApp-ai/Liquid.Net/issues) page.