# Script para executar testes com cobertura de código
# Execute este script para gerar relatório de cobertura

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "EducationHub - Execução de Testes" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Executar testes unitários
Write-Host "Executando testes unitários..." -ForegroundColor Yellow
dotnet test tests/EducationHub.Tests.Unit/EducationHub.Tests.Unit.csproj `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=./TestResults/ `
    --verbosity minimal

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Testes concluídos!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

Write-Host "Para gerar relatório HTML de cobertura, instale:" -ForegroundColor Yellow
Write-Host "  dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor White
Write-Host ""
Write-Host "E execute:" -ForegroundColor Yellow
Write-Host "  reportgenerator -reports:tests\EducationHub.Tests.Unit\TestResults\coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html" -ForegroundColor White
