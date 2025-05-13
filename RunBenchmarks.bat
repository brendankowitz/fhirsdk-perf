@echo off
echo Building and running Firely SDK Performance Benchmarks...

echo.
echo Building FirelySdk430.Benchmark...
dotnet build .\src\FirelySdk430.Benchmark\FirelySdk430.Benchmark.csproj -c Release

echo.
echo Building FirelySdk5114.Benchmark...
dotnet build .\src\FirelySdk5114.Benchmark\FirelySdk5114.Benchmark.csproj -c Release

echo.
echo Building FirelySdk6Alpha.Benchmark...
dotnet build .\src\FirelySdk6Alpha.Benchmark\FirelySdk6Alpha.Benchmark.csproj -c Release

echo.
echo Running FirelySdk430.Benchmark...
dotnet run --project .\src\FirelySdk430.Benchmark\FirelySdk430.Benchmark.csproj -c Release

echo.
echo Running FirelySdk5114.Benchmark...
dotnet run --project .\src\FirelySdk5114.Benchmark\FirelySdk5114.Benchmark.csproj -c Release

echo.
echo Running FirelySdk6Alpha.Benchmark...
dotnet run --project .\src\FirelySdk6Alpha.Benchmark\FirelySdk6Alpha.Benchmark.csproj -c Release

echo.
echo All benchmarks completed.
