# RentalCar.Manufacturer
Microserviço para gestão de fabricantes da loja de aluguer de carros.

# Language
1. C#

# Framework
1. .NET CORE 8.0

# Data Base
1. MySql

# Arquitectura
1. Arquitetura Limpa (Clean Architecture)

# Padrões
1. CQRS
2. Repository

# Container
1. Docker
2. docker-compose

# Testes
1. Unitario (Fluent Assertions)
2. Integração

# Messageria
1. RabbitMq

# CI/CD
1.  GitHub Actions

# Logs
1. Serilog

# Observabilidade
1. OpenTelemetry

# Monitoramento
1. Prometheus
2. Grafana

# Tracing 
1. Jeager

# Analise Código
1. SonarQube

# Migration
dotnet ef migrations add FirstMigration --project RentalCar.Manufacturer.Infrastructure -o Persistence/Migrations -s RentalCar.Manufacturer.API
dotnet ef database update --project RentalCar.Manufacturer.Infrastructure -s RentalCar.Manufacturer.API
