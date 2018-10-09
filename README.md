 [![Build Status](https://travis-ci.org/Aethon/glare.svg?branch=master)](https://travis-ci.org/Aethon/glare) [![Coverage Status](https://coveralls.io/repos/github/Aethon/glare/badge.svg?branch=master)](https://coveralls.io/github/Aethon/glare?branch=master)
# Glare
Glare is intended to collect and refine its author's experience with metaprogramming into a practical, FOSS toolkit.
## Parser
The Glare parser is a parser combinator library implemented as a [GLR](https://en.wikipedia.org/wiki/GLR_parser). The implementation handles left recursion with no effort from the grammar author. Grammar ambiguity is handled by returning all possible matches.

## This Repo

### dotnet
* Platform
  * .NET Core 2.1
* Environment
  * Generally developed using the [JetBrains Rider]() IDE on Linux/Mac (but other than CI, should build anywhere)
  * CI build is provided by [Travis CI](https://travis-ci.org/Aethon/glare)
* Verification
  * Unit tests are structured for [xUnit.net](https://xunit.github.io/) and asserted using [Fluent Assertions](https://fluentassertions.com/)
  * Code coverage is recorded using [coverlet](https://github.com/tonerdo/coverlet)
  * [ReportGenerator](https://github.com/danielpalme/ReportGenerator) visualizes coverage for local workflows
  * CI build coverage is reported to [Coveralls](https://coveralls.io/github/Aethon/glare) using [coveralls.net](https://github.com/csMACnz/coveralls.net)

### Workflow
To generate the coverage report:
```
cd dotnet
dotnet msbuild -t:Coverage
open GlareParserTests/bin/Debug/coverage/report/index.htm
```
