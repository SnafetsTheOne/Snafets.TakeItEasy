# Build the Docker image from src/ui/Dockerfile
$uiDir = Join-Path $PSScriptRoot "..\src\ui" | Resolve-Path
$uiDockerfilePath = Join-Path $uiDir "Dockerfile"
$uiImageName = "snafets-takeiteasy-ui"

Write-Host "docker build -t $uiImageName -f $uiDockerfilePath $uiDir"
docker build -t "$uiImageName" -f "$uiDockerfilePath" "$uiDir"

# Build the Docker image from src/api/Snafets.TakeItEasy.Api/Dockerfile
$apiDir = Join-Path $PSScriptRoot "..\src\api" | Resolve-Path
$dockerfilePath = Join-Path $apiDir "Snafets.TakeItEasy.Api\Dockerfile"
$imageName = "snafets-takeiteasy-api:latest"

dotnet clean $(Join-Path $PSScriptRoot "../Snafets.TakeItEasy.sln")
Write-Host "Building Docker image '$imageName' from '$dockerfilePath'..."
docker build -t $imageName -f $dockerfilePath $apiDir
