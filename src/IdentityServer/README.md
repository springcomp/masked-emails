# Identity Provider

This project is the Identity Provider used by 'Masked Emails'. When run locally, this project is hosted on port 5000.

It uses Azure Table Storage to store the clients, resources, tokens, consents and claims.

## Initialize

To host this project, you need to provide a self-signed signing certificate in `App_Data\signing.pfx`

This certificate is used for Token signing and validation as outlined [in the documentation](https://identityserver4.readthedocs.io/en/latest/topics/crypto.html#refcrypto).

Use the following commands to create the Token signing certificate:

```
> openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout signing.key -out signing.crt -subj "/CN=<domain.tld>" -days 3650
> openssl pkcs12 -export -out signing.pfx -inkey signing.key -in signing.crt -certfile signing.crt 
``` 

Please, take note of the password for the signing certificate, that will be required as a configuration parameter as described hereafter.

This project makes use of two configuration parameters, using the ASP.Net Secrets Manager:

- `TableStorage:IdentityServerStore`: connection string to an Azure Storage Account.
- `SigningCertificate:Password`: password to an SSL certificate used to sign requests.

Use the following commands to initialize those configuration parameters:

```
> dotnet user-secrets set "TableStorage:IdentityServerStore" "<connection-string>"
> dotnet user-secrets set "SigningCertificate:Password" "<password>"
```

The project comes with sample data that you must seed using the following command:

```
> dotnet run -- /seed
```

This will create the necessary users and API resources. The following resources are created:

- client `client.JS` with redirect uri `http://localhost:5001`

- user `alice`, with password `password`.
- user `bob`, with password `password`.
