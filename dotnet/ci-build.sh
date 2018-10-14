#! /bin/bash

cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null

dotnet restore
dotnet build
dotnet test GlareParserTests/GlareParserTests.csproj
