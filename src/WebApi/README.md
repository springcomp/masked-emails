# Overview

This is the 'Masked Emails' API.

When run locally, this project is hosted on port 5001.

## Initializing

This project makes use of a configuration parameter, using the ASP.Net Secrets Manager:

- `TableStorage:ConnectionString`: connection string to an Azure Storage Account.

The storage account hosts a storage queue names `commands` that is used to communicate with the masked emails backend.

Use the following commands to initialize those configuration parameters:

```
> dotnet user-secrets set "TableStorage:ConnectionString" "<connection-string>"
```

The project comes with sample data that you must seed using the following command:

```
> dotnet run -- /seed
```

This will create the initial SQLite database file in `App_Data\Profiles.db`.
