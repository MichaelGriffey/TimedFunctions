@echo off
setlocal

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore

REM Clean previous build artifacts
echo Cleaning previous build artifacts...
dotnet clean --configuration Release

REM Build for .NET 8.0 and .NET 9.0
echo Building for .NET 8.0 and .NET 9.0...
dotnet build --configuration Release

REM Pack for NuGet, targeting both frameworks
echo Packing for NuGet...
dotnet pack --configuration Release --output ".\nupkg"

echo Build and pack process complete.
endlocal
pause