# Weather

For SQL server docker image was used: 
* sudo docker pull mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04 (see more https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash&source=docs)

Additional dotnet packages might be needed (latest release might not available on NuGet):
* dotnet add package --version 5.0.0-rc3 Swashbuckle.AspNetCore
* dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 3.1.0-preview3.19555.2

Swagger spec: https://localhost:5001/swagger/CityWeatherAPISpecification/swagger.json

Swagger UI: https://localhost:5001/swagger/index.html

Migrations
* from Weather.Data project run: dotnet ef migrations add initialcreate -s ../Weather/Weather.csproj --context WeatherDbContext
* from Weather project run dotnet ef migrations add initialcreate -s ../Weather/Weather.csproj --context WeatherIdentityDbContext

Application settings are available at appsettings.json, where you can find default user/password, path to DB (should be changed if running on Windows machine).

To access Api for City or Temperature, please Authenticate first and receive a token by running Account Login request.
