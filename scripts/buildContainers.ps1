# Set ErrorActionPreference to stop on error
$ErrorActionPreference = 'Stop'

$repositoryRoot = Join-Path $PSScriptRoot "..\." | Resolve-Path
$apiDir = "$repositoryRoot\src\api"
$uiDir = "$repositoryRoot\src\ui"

# <h1> Build </h1>
# Build the API
Write-Host "dotnet clean ""$repositoryRoot/Snafets.TakeItEasy.sln"""
dotnet clean "$repositoryRoot/Snafets.TakeItEasy.sln"
Write-Host "dotnet build ""$repositoryRoot/Snafets.TakeItEasy.sln"""
dotnet build "$repositoryRoot/Snafets.TakeItEasy.sln"

# Build the UI
Write-Host "npm run build --prefix ""$uiDir"""
npm run build --prefix "$uiDir"

# <h1> Test </h1>
Write-Host "dotnet test ""$repositoryRoot/Snafets.TakeItEasy.sln"""
dotnet test "$repositoryRoot/Snafets.TakeItEasy.sln"

# <h1> Build images </h1>

# Build the Docker image from src/api/Snafets.TakeItEasy.Api/Dockerfile
$dockerfilePath = Join-Path $apiDir "Snafets.TakeItEasy.Api\Dockerfile"
$imageName = "snafets-takeiteasy-api"

Write-Host "dotnet clean ""$repositoryRoot/Snafets.TakeItEasy.sln"""
dotnet clean "$repositoryRoot/Snafets.TakeItEasy.sln"
Write-Host "podman build -t $imageName -f $dockerfilePath $apiDir"
podman build -t $imageName -f $dockerfilePath $apiDir
Write-Host "podman save -o $repositoryRoot\images\$imageName.tar $imageName"
podman save -o "$repositoryRoot\images\$imageName.tar" $imageName

# Build the Docker image from src/ui/Dockerfile
$uiDockerfilePath = Join-Path $uiDir "Dockerfile"
$uiImageName = "snafets-takeiteasy-ui"

Write-Host "podman build -t $uiImageName -f $uiDockerfilePath $uiDir"
podman build -t "$uiImageName" -f "$uiDockerfilePath" "$uiDir"
Write-Host "podman save -o $repositoryRoot\images\$uiImageName.tar $uiImageName"
podman save -o "$repositoryRoot\images\$uiImageName.tar" $uiImageName
