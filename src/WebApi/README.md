# Overview

This is the 'Masked Emails' API.

When run locally, this project is hosted on port 5001.

## Prerequisites

The WebAPI stored its data into a CosmosDb database.
You can use the [CosmosDb Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) to debug locally.

The WebAPI also sends commands to the mail server using an Azure Storage Account queue. You can use [Azurite Local Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) to debug locally.

The WebAPI is also client to a secondary *inbox* API for which it needs to retrieve a JWT token for authentication. You will need to have a valid
set of credentials that allow authentication to this API. Please, refer to [the documentation](https://github.com/springcomp/masked-emails-inboxapi#configuration) of the *masked-emails-inboxapi* server.

## Initializing

This project makes use of some configuration parameters. Most of which can be stored using the ASP.Net Secrets Manager:

- `TableStorage:ConnectionString`: connection string to an Azure Storage Account.

- `CosmosDb:EndpointUri`: URI to the CosmosDb service.
- `CosmosDb:PrimaryKey`: primary key to the CosmosDb service.
- `CosmosDb:IgnoreSslServerCertificateValidation`: set to `true` to ignore certificate validation, useful when running the CosmosDb emulator from a Docker container, for instance. Defaults to `false`.

- `InboxApi:Endpoint`: URI to the inbox API to retrieve mailbox items.
- `InboxApi:ClientId`: client credentials identifier to the inbox API.
- `InboxApi:ClientSecret`: client secret to request a JWT token for the inbox API.
- `InboxApi:Audience`: client credentials resource grant.
- `InboxApi:IdentityProviderEndpoint`: URI to the identity provider.
- `InboxApi:Authority`: identity provider authority.


The storage account hosts a storage queue names `commands` that is used to communicate with the masked emails backend.

Use the following commands to initialize those configuration parameters:

```
> dotnet user-secrets set "TableStorage:ConnectionString" "<connection-string>"
```

The project comes with sample data that you must seed using the following command:

```
> dotnet run -- /seed
```

This will create the initial CosmosDb database and container, and insert some sample records.
