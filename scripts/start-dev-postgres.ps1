param()

$ErrorActionPreference = "Stop"

Write-Host "Starting PostgreSQL with Docker Compose..."
docker compose up -d postgres

Write-Host "PostgreSQL container status:"
docker compose ps postgres
