Write-Host "Building and running Firely SDK Performance Benchmarks..." -ForegroundColor Cyan

Write-Host "`nBuilding FirelySdk430.Benchmark..." -ForegroundColor Yellow
dotnet build .\src\FirelySdk430.Benchmark\FirelySdk430.Benchmark.csproj -c Release

Write-Host "`nBuilding FirelySdk5114.Benchmark..." -ForegroundColor Yellow
dotnet build .\src\FirelySdk5114.Benchmark\FirelySdk5114.Benchmark.csproj -c Release

Write-Host "`nBuilding FirelySdk6Alpha.Benchmark..." -ForegroundColor Yellow
dotnet build .\src\FirelySdk6Alpha.Benchmark\FirelySdk6Alpha.Benchmark.csproj -c Release

Write-Host "`nRunning FirelySdk430.Benchmark..." -ForegroundColor Green
dotnet run --project .\src\FirelySdk430.Benchmark\FirelySdk430.Benchmark.csproj -c Release

Write-Host "`nRunning FirelySdk5114.Benchmark..." -ForegroundColor Green
dotnet run --project .\src\FirelySdk5114.Benchmark\FirelySdk5114.Benchmark.csproj -c Release

Write-Host "`nRunning FirelySdk6Alpha.Benchmark..." -ForegroundColor Green
dotnet run --project .\src\FirelySdk6Alpha.Benchmark\FirelySdk6Alpha.Benchmark.csproj -c Release

Write-Host "`nAll benchmarks completed." -ForegroundColor Cyan
