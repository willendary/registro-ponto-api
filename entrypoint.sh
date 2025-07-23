#!/bin/bash

set -e

# Wait for SQL Server to be ready
until /opt/mssql-tools/bin/sqlcmd -S db -U sa -P '@Willian9221' -Q 'SELECT 1'; do
  echo 'Waiting for SQL Server...'
  sleep 5
done

echo 'SQL Server is up - executing migrations'
/root/.dotnet/tools/dotnet ef database update --project registro-ponto-api.csproj

exec "$@"
