# CLI for Run Project
## PROD : 
~ dotnet run --environment "Production"
## UAT : 
~ dotnet run --environment "Staging"
## DEV : 
~ dotnet run --environment "Development"

#BUILD
~ dotnet build

#RUN 
~ dotnet restore
~ dotnet dev-certs https --trust 
~ dotnet run