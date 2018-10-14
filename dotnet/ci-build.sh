#! /bin/bash
set -e

cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null

CONFIGURATION="Release"

echo "Building and verifying $CONFIGURATION configuration"
dotnet restore
dotnet build --configuration $CONFIGURATION
dotnet test --configuration $CONFIGURATION GlareParserTests/GlareParserTests.csproj

VERSION_SUFFIX="preview-$TRAVIS_BUILD_NUMBER-$TRAVIS_BRANCH"
echo "Packaging and publishing $VERSION_SUFFIX"
dotnet pack --no-build --configuration $CONFIGURATION GlareParser/ --version-suffix $VERSION_SUFFIX
if [ -z "$NUGET_API_KEY" ]; then
    echo "NuGet API key not set"
    exit 1
fi
if [ -z "$NUGET_PUSH_SOURCE" ]; then
    echo "NuGet push source not set"
fi
dotnet nuget push GlareParser/bin/$CONFIGURATION/Aethon.GlareParser.*.nupkg --force-english-output -k $NUGET_API_KEY -s $NUGET_PUSH_SOURCE