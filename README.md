# CleanArchTemplate
## Install Template
```
dotnet new install .
```
## Start new Project
```
dotnet new cleanarch -o <project-name>
```
## Add migrations
```
dotnet ef  migrations add "Initial" --project src/CleanArchTemplate.Infrastructure/CleanArchTemplate.Infrastructure.csproj -s src/Server  --output-dir Persistences/Migrations
```
