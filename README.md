# Web app and Pgbouncer

## Build Status

[![Build Status](https://dev.azure.com/jannemattila/jannemattila/_apis/build/status/JanneMattila.328-webapp-pgbouncer?branchName=master)](https://dev.azure.com/jannemattila/jannemattila/_build/latest?definitionId=48&branchName=master)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Introduction

Web app demonstrating use of Pgbouncer sidecar
for optimizing connections to [Azure Databases for PostgreSQL](https://github.com/JanneMattila/some-questions-and-some-answers/blob/master/q%26a/azure_database_for_postgresql.md#azure-databases-for-postgresql).

Remember also to read these [additional instructions](https://github.com/JanneMattila/some-questions-and-some-answers/blob/master/q%26a/azure_database_for_postgresql.md).

## Trying this locally

Using `docker-compose` in project root folder:

```bash
docker-compose up
```

Then execute `webapp` in your Visual Studio. Then
you're ready to test the performances using `api.http`
requests.

## Trying this in Azure

Create following resources to Azure

- Azure Database for PostgreSQL
- Web App for Containers

Create `docker-compose.yml` and replace following
texts in it:

- `passwordhere` with correct password
- `userhere` with correct user name
- `yourinstancename` with your database instance name

```docker-compose
version: '3'

services:
  web:
    image: jannemattila/webapp-pgbouncer
  pgbouncer:
    image: pgbouncer/pgbouncer
    ports:
      - "5439:5439"
    environment:
      - POSTGRES_PASSWORD=passwordhere
      - DATABASES_HOST=yourinstancename.postgres.database.azure.com
      - DATABASES_PORT=5432
      - DATABASES_USER=userhere@yourinstancename.postgres.database.azure.com
      - DATABASES_PASSWORD=userhere
      - DATABASES_DBNAME=postgres
      - PGBOUNCER_SERVER_TLS_SSLMODE=verify-ca
      - PGBOUNCER_LISTEN_PORT=5439
```

If you now deploy above `docker-compose.yml` to
the created App Service then you can test the pgbouncer using
VS Code.

Open `api.http` in VS Code and try out the requests to
see how the performance changes if using pgbouncer or not.
Here are example requests to try (see [api.http](api.http) for
full examples):

```http
### Execute query using direct connection to Azure Database for PostgreSQL
POST {{endpoint_azure}}/api/query HTTP/1.1
Content-Type: application/json

{
    "connectionString": "Database=postgres;Host=yourinstancename.postgres.database.azure.com;Port=5432;Username=postgres@yourinstancename.postgres.database.azure.com;Ssl Mode=Require;Password=password",
    "query": "SELECT 1",
    "count": 10
}

### Execute query using pgbouncer (using pgbouncer running in App Service)
POST {{endpoint_azure}}/api/query HTTP/1.1
Content-Type: application/json

{
    "connectionString": "Database=postgres;Host=pgbouncer;Port=5439;Username=pgbouncer",
    "query": "SELECT 1",
    "count": 10
}
```

Output of the API requests is array of durations
of the query executions in milliseconds:

```http
HTTP/1.1 200 OK
Connection: close
Date: Mon, 09 Mar 2020 20:40:54 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked

{
  "durations": [
    4.9034,
    1.3396,
    1.2685,
    1.1928,
    1.1278,
    1.1877,
    1.5707,
    1.9082,
    2.4554,
    2.1007
  ]
}
```
