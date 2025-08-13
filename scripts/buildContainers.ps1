# Build the Docker image from src/api/Snafets.TakeItEasy.Api/Dockerfile
$apiDir = Join-Path $PSScriptRoot "..\src\api" | Resolve-Path
$dockerfilePath = Join-Path $apiDir "Snafets.TakeItEasy.Api\Dockerfile"
$imageName = "snafets-takeiteasy-api"

dotnet clean $(Join-Path $PSScriptRoot "../Snafets.TakeItEasy.sln")
Write-Host "podman build -t $imageName -f $dockerfilePath $apiDir"
podman build -t $imageName -f $dockerfilePath $apiDir
Write-Host "podman save -o $PSScriptRoot\..\images\$imageName.tar $imageName"
podman save -o "$PSScriptRoot\..\images\$imageName.tar" $imageName

# Build the Docker image from src/ui/Dockerfile
$uiDir = Join-Path $PSScriptRoot "..\src\ui" | Resolve-Path
$uiDockerfilePath = Join-Path $uiDir "Dockerfile"
$uiImageName = "snafets-takeiteasy-ui"

Write-Host "podman build -t $uiImageName -f $uiDockerfilePath $uiDir"
podman build -t "$uiImageName" -f "$uiDockerfilePath" "$uiDir"
Write-Host "podman save -o $PSScriptRoot\..\images\$uiImageName.tar $uiImageName"
podman save -o "$PSScriptRoot\..\images\$uiImageName.tar" $uiImageName
