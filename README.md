# Firely SDK JSON Performance Benchmark

This solution compares the performance of JSON serialization and deserialization between Firely SDK versions 4.3.0 and 5.11.4 using BenchmarkDotNet.

## Project Structure

- **Common**: Contains test data shared by both benchmark projects
- **FirelySdk430.Benchmark**: Benchmark project for Firely SDK version 4.3.0
- **FirelySdk5114.Benchmark**: Benchmark project for Firely SDK version 5.11.4

## Benchmark Tests

The following operations are benchmarked:

1. **DeserializePatient**: Deserialize a single Patient resource
2. **DeserializeBundle**: Deserialize a Bundle containing multiple Patient resources
3. **SerializePatient**: Serialize a Patient resource to JSON
4. **SerializeBundle**: Serialize a Bundle to JSON

## Running the Benchmarks

### Using PowerShell

```powershell
.\RunBenchmarks.ps1
```

### Using Command Prompt

```cmd
RunBenchmarks.bat
```

### Manual Execution

To run the benchmarks individually:

```
dotnet run --project .\src\FirelySdk430.Benchmark\FirelySdk430.Benchmark.csproj -c Release
dotnet run --project .\src\FirelySdk5114.Benchmark\FirelySdk5114.Benchmark.csproj -c Release
```

## Results

After running the benchmarks, the results will be displayed in the console and saved to the `BenchmarkDotNet.Artifacts` folder in each project.

## Notes

- Both projects use the same FHIR version (R4) to ensure a fair comparison
- The projects are separated because the Firely SDK namespaces conflict between versions
- Memory diagnostics are included to measure allocation differences between versions
