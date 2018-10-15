//using System.Collections.Immutable;
//using Aethon.Glare.Parsing;
//using static Aethon.Glare.Parsing.Parsers;
//
//namespace Aethon.Glare.Syntax
//{
//    public class SyntaxParser
//    {
//        public class Token
//        {
//        }
//
//        public abstract class TokenType : IParser<char, Token>
//        {
//            public object Key { get; }
//
//            public WorkList<char> Start(Resolver<char, Token> resolver)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public class AcquiredTokenType : TokenType
//        {
//            public AcquiredTokenType(string name, string value, string thingy)
//            {
//            }
//        }
//
//        public class IdempotentTokenType : TokenType
//        {
//            public IdempotentTokenType(string name, string value, string thingy)
//            {
//            }
//        }
//
//        public class AlternateTokenType : TokenType
//        {
//            public AlternateTokenType(string name, params TokenType[] options)
//            {
//            }
//        }
//
//        public abstract class TypeRefSyntax
//        {
//        }
//
//        public class ScalarTypeRefSyntax : TypeRefSyntax
//        {
//            public ScalarTypeRefSyntax(Token id)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public class AmplifiedTypeRefSyntax : TypeRefSyntax
//        {
//            public AmplifiedTypeRefSyntax(TypeRefSyntax elementType, Token kleeneOp, Maybe<Token> separator)
//            {
//            }
//        }
//
//        // TOKEN parsers
//        public static readonly AcquiredTokenType ABSTRACT_KEYWORD =
//            new AcquiredTokenType("ABSTRACT_KEYWORD", "abstract", "\\babstract\\b");
//
//        public static readonly IdempotentTokenType ARROW = new IdempotentTokenType("ARROW", ("=>"), ("=>"));
//        public static readonly IdempotentTokenType CLOSE_BRACE = new IdempotentTokenType("CLOSE_BRACE", ("}"), ("}"));
//        public static readonly IdempotentTokenType COLON = new IdempotentTokenType("COLON", (":"), (":"));
//        public static readonly IdempotentTokenType COMMA = new IdempotentTokenType("COMMA", (","), (","));
//        public static readonly IdempotentTokenType DOT = new IdempotentTokenType("DOT", ("."), ("."));
//
//        public static readonly AcquiredTokenType DOTTED_IDENTIFIER = new AcquiredTokenType("DOTTED_IDENTIFIER",
//            "DOTTED_IDENTIFIER", "\\b[a-zA-z_][a-zA-z0-9_]*(?:\\.[a-zA-z_][a-zA-z0-9_]*)+\\b");
//
//        public static readonly AcquiredTokenType IDENTIFIER =
//            new AcquiredTokenType("IDENTIFIER", "IDENTIFIER", "\\b[a-zA-Z_][a-zA-Z0-9_]*\\b");
//
//        public static readonly AcquiredTokenType NAME_KEYWORD =
//            new AcquiredTokenType("NAME_KEYWORD", "name", "\\bname\\b");
//
//        public static readonly AcquiredTokenType NAMESPACE_KEYWORD =
//            new AcquiredTokenType("NAMESPACE_KEYWORD", "namespace", "\\bnamespace\\b");
//
//        public static readonly IdempotentTokenType OPEN_BRACE = new IdempotentTokenType("OPEN_BRACE", ("{"), ("{"));
//
//        public static readonly AcquiredTokenType OPTIONS_KEYWORD =
//            new AcquiredTokenType("OPTIONS_KEYWORD", "options", "\\boptions\\b");
//
//        public static readonly AcquiredTokenType PATTERN =
//            new AcquiredTokenType("PATTERN", "PATTERN", "\\/(?:\\\\\\/|[^\\/])*\\/");
//
//        public static readonly IdempotentTokenType PLUS = new IdempotentTokenType("PLUS", ("+"), ("+"));
//
//        public static readonly IdempotentTokenType QUESTION_MARK =
//            new IdempotentTokenType("QUESTION_MARK", ("?"), ("?"));
//
//        public static readonly AcquiredTokenType SEMANTIC_TEXT =
//            new AcquiredTokenType("SEMANTIC_TEXT", "SEMANTIC_TEXT", "(?ms)`(?:``|[^`])*`");
//
//        public static readonly IdempotentTokenType SEMICOLON = new IdempotentTokenType("SEMICOLON", (";"), (";"));
//        public static readonly IdempotentTokenType STAR = new IdempotentTokenType("STAR", ("*"), ("*"));
//
//        public static readonly AcquiredTokenType STRING_LITERAL =
//            new AcquiredTokenType("STRING_LITERAL", "STRING_LITERAL", "\"(?:\\\\\"|[^\"])*\"");
//
//        public static readonly AcquiredTokenType SYNTAX_KEYWORD =
//            new AcquiredTokenType("SYNTAX_KEYWORD", "syntax", "\\bsyntax\\b");
//
//        public static readonly AcquiredTokenType TOKENS_KEYWORD =
//            new AcquiredTokenType("TOKENS_KEYWORD", "tokens", "\\btokens\\b");
//
//        public static readonly AcquiredTokenType TRIVIA_KEYWORD =
//            new AcquiredTokenType("TRIVIA_KEYWORD", "trivia", "\\btrivia\\b");
//
//        public static readonly IdempotentTokenType UPRIGHT = new IdempotentTokenType("UPRIGHT", ("|"), ("|"));
//        public static readonly AlternateTokenType KLEENE = new AlternateTokenType("KLEENE", PLUS, STAR, QUESTION_MARK);
//
////        package com.code42.popsyn.parsers;
////
////import com.code42.domain.primitives.Result;
////import com.code42.popcore.syntax.EofToken;
////import com.code42.popcore.syntax.SyntaxTree;
////import com.code42.popcore.syntax.Token;
////import com.code42.popgen.ParseSource;
////import com.code42.popsyn.syntax.*;
////import org.javafp.data.IList;
////import org.javafp.parsecj.Parser;
////import org.javafp.parsecj.StringStateWithTrivia;
////
////import java.util.regex.Pattern;
////
////import static com.code42.domain.primitives.Result.failure;
////import static com.code42.domain.primitives.Shorthand.iml;
////import static com.code42.domain.primitives.Result.success;
////import static org.javafp.parsecj.Combinators.*;
////
////public class PopSynSyntaxParser extends PopSynParser {
//        public static readonly BasicParser<char, ImmutableList<Token>>
//            semantics =
//                ZeroOrMore(SEMANTIC_TEXT);
//
//        public static readonly BasicParser<char, ScalarTypeRefSyntax>
//            scalarTypeRef =
//                IDENTIFIER.As(id => new ScalarTypeRefSyntax(id));
//
//        public static readonly DeferredParser<char, TypeRefSyntax> typeRef = Deferred<char, TypeRefSyntax>();
//
//        static SyntaxParser()
//        {
//            typeRef.Set(OneOf<char, TypeRefSyntax>(scalarTypeRef, amplifiedTypeRef));
//        }
//
//        public static readonly BasicParser<char, AmplifiedTypeRefSyntax>
//            amplifiedTypeRef =
//                typeRef.Then(elementType =>
//                    KLEENE.Then(amp =>
//                        Optional(IDENTIFIER).As(separator =>
//                            new AmplifiedTypeRefSyntax(elementType, amp, separator))));
//
//        public static readonly BasicParser<char, MemberNameSyntax>
//            memberName =
//                IDENTIFIER.Then(name =>
//                    COLON.As(colon => new MemberNameSyntax(name, colon)));
//
//        public static readonly BasicParser<char, MemberDefSyntax>
//            memberDef =
//                semantics.Then(semantics =>
//                    Optional(memberName).Then(name =>
//                        typeRef.As(type =>
//                            new MemberDefSyntax(semantics, name, type))));
//
//        public class MemberDefSyntax
//        {
//            public MemberDefSyntax(ImmutableList<Token> semantics, Maybe<MemberNameSyntax> name, TypeRefSyntax type)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, BaseSyntaxSyntax>
//            baseSyntax =
//                COLON.Then(colon =>
//                    IDENTIFIER.As(name =>
//                        new BaseSyntaxSyntax(colon, name)));
//
//        public class BaseSyntaxSyntax
//        {
//            public BaseSyntaxSyntax(Token colon, Token name)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, ProductionListSyntax>
//            productionList =
//                ARROW.Then(arrow =>
//                    SeparatedList(memberDef, COMMA).As(members =>
//                        new ProductionListSyntax(arrow, members)));
//
//        public class ProductionListSyntax
//        {
//            public ProductionListSyntax(Token arrow, ImmutableList<MemberDefSyntax> members)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, SyntaxDefSyntax>
//            syntaxDef =
//                semantics.Then(semantics =>
//                    Optional(ABSTRACT_KEYWORD).Then(abstractKeyword =>
//                        IDENTIFIER.Then(name =>
//                            Optional(baseSyntax).Then(baseSyntax =>
//                                Optional(productionList).Then(productionList =>
//                                    SEMICOLON.As(semicolon =>
//                                        new SyntaxDefSyntax(semantics, abstractKeyword, name, baseSyntax,
//                                            productionList, semicolon)))))));
//
//        public class SyntaxDefSyntax
//        {
//            public SyntaxDefSyntax(ImmutableList<Token> semantics, Maybe<Token> abstractKeyword, Token name,
//                Maybe<BaseSyntaxSyntax> baseSyntax, Maybe<ProductionListSyntax> productionListSyntax, Token semicolon)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, SyntaxSetSyntax>
//            syntax =
//                SYNTAX_KEYWORD.Then(syntaxKeyword =>
//                    OPEN_BRACE.Then(leftBrace =>
//                        ZeroOrMore(syntaxDef).Then(syntax =>
//                            CLOSE_BRACE.As(rightBrace =>
//                                new SyntaxSetSyntax(syntaxKeyword, leftBrace, syntax, rightBrace)))));
//
//        public class SyntaxSetSyntax
//        {
//            public SyntaxSetSyntax(Token syntaxKeyword, Token leftBrace, ImmutableList<SyntaxDefSyntax> syntaxDefs,
//                Token rightBrace)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, AlternateTokenContentSyntax>
//            alternateTokenContent =
//                NonEmptySeparatedList(IDENTIFIER, UPRIGHT).As(tokens =>
//                    new AlternateTokenContentSyntax(tokens));
//
//        public class AlternateTokenContentSyntax : TokenContentDefSyntax
//        {
//            public AlternateTokenContentSyntax(ImmutableList<Token> tokens)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, PatternTokenContentSyntax>
//            patternTokenContent =
//                PATTERN.As(pattern =>
//                    new PatternTokenContentSyntax(pattern));
//
//        public class PatternTokenContentSyntax : TokenContentDefSyntax
//        {
//            public PatternTokenContentSyntax(Token pattern)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, LiteralTokenContentSyntax>
//            literalTokenContent =
//                STRING_LITERAL.As(text =>
//                    new LiteralTokenContentSyntax(text));
//
//        public class LiteralTokenContentSyntax : TokenContentDefSyntax
//        {
//            public LiteralTokenContentSyntax(Token text)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public abstract class TokenContentDefSyntax
//        {
//        }
//
//        public static readonly BasicParser<char, TokenContentDefSyntax>
//            tokenContentDef =
//                OneOf<char, TokenContentDefSyntax>(patternTokenContent, literalTokenContent, alternateTokenContent);
//
//        public static readonly BasicParser<char, TokenDefSyntax>
//            tokenDef =
//                semantics.Then(semantics =>
//                    IDENTIFIER.Then(name =>
//                        ARROW.Then(arrow =>
//                            tokenContentDef.Then(content =>
//                                SEMICOLON.As(semicolon =>
//                                    new TokenDefSyntax(semantics, name, arrow, content, semicolon))))));
//
//        public class TokenDefSyntax
//        {
//            public TokenDefSyntax(ImmutableList<Token> semantics, Token name, Token arrow,
//                TokenContentDefSyntax content, Token semicolon)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, TokenSetSyntax>
//            tokens =
//                TOKENS_KEYWORD.Then(tokensKeyword =>
//                    OPEN_BRACE.Then(leftBrace =>
//                        ZeroOrMore(tokenDef).Then(tokens =>
//                            CLOSE_BRACE.As(rightBrace =>
//                                new TokenSetSyntax(tokensKeyword, leftBrace, tokens, rightBrace)))));
//
//        public class TokenSetSyntax
//        {
//            public TokenSetSyntax(Token tokensKeyword, Token leftBrace, ImmutableList<TokenDefSyntax> tokenDefs,
//                Token rightBrace)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, NameOptionSyntax>
//            nameOption =
//                semantics.Then(semantics =>
//                    NAME_KEYWORD.Then(nameKeyword =>
//                        IDENTIFIER.Then(id =>
//                            SEMICOLON.As(semicolon =>
//                                new NameOptionSyntax(semantics, nameKeyword, id, semicolon)))));
//
//        public class NameOptionSyntax : OptionSyntax
//        {
//            public NameOptionSyntax(ImmutableList<Token> semantics, Token nameKeyword, Token id, Token semicolon)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, NamespaceOptionSyntax>
//            namespaceOption =
//                semantics.Then(semantics =>
//                    NAMESPACE_KEYWORD.Then(packageKeyword =>
//                        DOTTED_IDENTIFIER.Then(id =>
//                            SEMICOLON.As(semicolon =>
//                                new NamespaceOptionSyntax(semantics, packageKeyword, id, semicolon)))));
//
//        public class NamespaceOptionSyntax : OptionSyntax
//        {
//            public NamespaceOptionSyntax(ImmutableList<Token> semantics, Token packageKeyword, Token id,
//                Token semicolon)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, TriviaOptionSyntax>
//            triviaOption =
//                semantics.Then(semantics =>
//                    TRIVIA_KEYWORD.Then(triviaKeyword =>
//                        PATTERN.Then(regex =>
//                            SEMICOLON.As(semicolon =>
//                                new TriviaOptionSyntax(semantics, triviaKeyword, regex, semicolon)))));
//
//        public class TriviaOptionSyntax : OptionSyntax
//        {
//            public TriviaOptionSyntax(ImmutableList<Token> semantics, Token triviaKeyword, Token regex, Token semicolon)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, OptionSyntax>
//            option =
//                OneOf<char, OptionSyntax>(triviaOption, nameOption, namespaceOption);
//
//        public abstract class OptionSyntax
//        {
//        }
//
//        public static readonly BasicParser<char, OptionsSyntax>
//            options =
//                OPTIONS_KEYWORD.Then(optionsKeyword =>
//                    OPEN_BRACE.Then(leftBrace =>
//                        ZeroOrMore(option).Then(options =>
//                            CLOSE_BRACE.As(rightBrace =>
//                                new OptionsSyntax(optionsKeyword, leftBrace, options, rightBrace)))));
//
//        public class OptionsSyntax
//        {
//            public OptionsSyntax(Token optionsKeyword, Token leftBrace, ImmutableList<OptionSyntax> options,
//                Token rightBrace)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//        public static readonly BasicParser<char, CompilationUnitSyntax>
//            compilationUnit =
//                Optional(options).Then(options =>
//                    Optional(tokens).Then(tokens =>
//                        Optional(syntax).As(syntax =>
//                            new CompilationUnitSyntax(options, tokens, syntax))));
//
//        public class CompilationUnitSyntax
//        {
//            public CompilationUnitSyntax(Maybe<OptionsSyntax> options, Maybe<TokenSetSyntax> tokens,
//                Maybe<SyntaxSetSyntax> syntax)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//
//
////  private static final Pattern triviaPattern = Pattern.compile("(?ms)(?:(?:\\s+)|(?://[^\\r\\n]*$)|(?:/\\*.*\\*/))+");
////
////  public Result<SyntaxTree<CompilationUnitSyntax>> parse(ParseSource parseSource) {
////    return compilationUnit.parse(StringStateWithTrivia.of(parseSource.text, triviaPattern)).match(
////        ok => success(new SyntaxTree<>(ok.result, parseSource)),
////        error => failure(makeUsableMessage(parseSource, error.msg))
////    );
////  }
////}
//
//
//        public class MemberNameSyntax
//        {
//            public MemberNameSyntax(Token name, Token colon)
//            {
//            }
//        }
//    }
//}