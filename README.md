# Assignment03

## Authors

- Andreas Guldborg Hansen - aguh@itu.dk
- Rakul Maria Hjalmarsdóttir Tórgarð - rakt@itu.dk
- William Skou Heidemann - wihe@itu.dk

## BE AWARE

We just continued in the same group for assignment-04, which means that the current state of the repository resembles assignment-04. To checkout assignment-03, please see [this specific commit](https://github.com/A-Guldborg/assignment-03/tree/baf0451e2dd36126a68c7170349574916d41a2d2) for the assignment.

## Notes

- Our implementation uses the type Task and not WorkItem as this was the type name when we forked the repo
- Our implementation uses [In-Memory testing](https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#inmemory-provider), which is not recommended, but saves us from installing SQLite.
- Our implementation uses MSSQL to connect to a database using .NET user-secrets. See section [user-secrets](#user-secrets) on how to configure your own .NET user-secrets to be used in the program.
- We decided on the business rule that a Task goes from State New to State Active once it has been assigned to a user.
- We decided to interpret the [assignment description](Assignment-description.md) business rule 1.2. "Create, Read, and Update should return a proper `Response`." as only counting for Create and Update, as the interface expects a TagDTO as return type and not a response. This also aligns with business rule 1.5 "If a task, tag, or user is not found, return `null`."

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
