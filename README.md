# DreamDay

## Add EF Core Tools and Migrations
```
dotnet ef migrations add InitialCreate -p DreamDay.DAL -s DreamDay.UI -o Data/Migrations
dotnet ef database update -p DreamDay.DAL -s DreamDay.UI
```

## Ensure EF packages installed in DAL project
```
dotnet add DreamDay.DAL package Microsoft.EntityFrameworkCore
dotnet add DreamDay.DAL package Microsoft.EntityFrameworkCore.SqlServer
dotnet add DreamDay.DAL package Microsoft.EntityFrameworkCore.Tools
```