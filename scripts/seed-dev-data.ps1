param(
    [string]$BaseUrl = "http://localhost:5280"
)

$ErrorActionPreference = "Stop"

Write-Host "Triggering development seed endpoint..."
Invoke-RestMethod -Uri "$BaseUrl/dev/seed" -Method Post
