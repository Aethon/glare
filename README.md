 [![Build Status](https://travis-ci.org/Aethon/glare.svg?branch=master)](https://travis-ci.org/Aethon/glare)
# Glare
Glare is intended to collect and refine its author's experience with metaprogramming into a practical, FOSS toolkit.
## Parser
The Glare parser is a parser combinator library implemented as a [GLR](https://en.wikipedia.org/wiki/GLR_parser). The implementation handles left recursion with no effort from the grammar author. Grammar ambiguity is handled by returning all possible matches.