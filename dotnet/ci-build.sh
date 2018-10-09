#! /bin/bash
set -e

cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null

CONFIGURATION="Release"

echo "Building and verifying $CONFIGURATION configuration"
dotnet restore
dotnet build --configuration $CONFIGURATION
dotnet msbuild -p:Configuration=$CONFIGURATION -t:Coverage GlareParserTests/GlareParserTests.csproj

if [ -z "$COVERALLS_REPO_TOKEN" ]; then
    echo "Coveralls repo token not set"
else
    REPO_COMMIT_AUTHOR=$(git show -s --pretty=format:"%cn")
    REPO_COMMIT_AUTHOR_EMAIL=$(git show -s --pretty=format:"%ce")
    REPO_COMMIT_MESSAGE=$(git show -s --pretty=format:"%s")
    tools/csmacnz.Coveralls --opencover -i ./GlareParserTests/bin/$CONFIGURATION/coverage/coverage.opencover.xml --repoToken $COVERALLS_REPO_TOKEN --commitId $TRAVIS_COMMIT --commitBranch $TRAVIS_BRANCH --commitAuthor "$REPO_COMMIT_AUTHOR" --commitEmail "$REPO_COMMIT_AUTHOR_EMAIL" --commitMessage "$REPO_COMMIT_MESSAGE" --jobId $TRAVIS_JOB_ID  --serviceName "travis-ci"  --useRelativePaths
fi

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

