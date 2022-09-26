# Assignment03

## Authors

- Andreas Guldborg Hansen - aguh@itu.dk
- Rakul Maria Hjalmarsdóttir Tórgarð - rakt@itu.dk
- William Skou Heidemann - wihe@itu.dk

## Notes

- Our implementation uses the type Task and not WorkItem as this was the case when we forked the repo
- Our implementation uses [In-Memory testing](https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#inmemory-provider), which is not recommended, but saves us from installing SQLite.
- Our implementation uses MSSQL to connect to a database using .NET user-secrets. See section [user-secrets](#user-secrets) on how to configure your own .NET user-secrets to be used in the program.
- We decided on the business rule that a Task goes from State New to State Active once it has been assigned to a user.

## User-secrets

Make sure [Docker](https://www.docker.com) is installed on your machine and run the following command to install the latest MSSQL 2019 version:

```sh
docker pull mcr.microsoft.com/mssql/server:2019-latest
```

Then run the following command to start a docker container, where you can change the password to whatever you like:

```sh
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" --name "Kanban" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

With the active directory set to [Assignment3](/Assignment3/), run the following command in your terminal and change the password if you changed it in the previous command:

```sh
dotnet user-secrets set "ConfigurationsString:ConfigurationString" "Server=Kanban;Database=Kanban;User Id=SA;Password=yourStrong(!)Password;"
```
