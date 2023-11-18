using GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective;
using LanguageExt.Parsec;
using static LanguageExt.Prelude;
using static LanguageExt.Parsec.Prim;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Token;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class Parser
    {
        public static Parser<CSyntaxNode> _identifier = lazyp(() => from i in Lexer.Identifier
                                                                    select new Identifier(i) as CSyntaxNode);

        private static Parser<CSyntaxNode> _blockItem =
            lazyp(() => choice(LinkageSpecification, TypeDefinition, PreprocessorCall));

        private static Parser<CSyntaxNode> _declarator =
            lazyp(() => choice(PointerDeclarator, FunctionDeclarator, ParenthesizedDeclarator, _identifier));

        private static Parser<CSyntaxNode> _typeDeclarator =
            lazyp(() => choice(PointerDeclarator, FunctionDeclarator, ParenthesizedDeclarator, _identifier));

        private static Parser<CSyntaxNode> _fieldDeclarator =
            lazyp(() => choice(PointerDeclarator, FunctionDeclarator, ParenthesizedDeclarator, _identifier));


        private static Parser<CSyntaxNode> _declarationModifier = lazyp(() => choice(TypeQualifier));

        public static GenLanguageDef Def = GenLanguageDef.Empty.With(
            CommentStart: "/*",
            CommentEnd: "*/",
            CommentLine: "//",
            NestedComments: true,
            IdentStart: either(letter, ch('_')),
            IdentLetter: choice(alphaNum, ch('_')),
            OpStart: oneOf("+-*/"),
            OpLetter: oneOf("+-*/"),
            ReservedNames: List(
                "#include", "#ifndef", "#ifdef", "#endif",
                "return", "if", "else", "for", "while", "int", "sizeof", "char",
                "struct", "union", "short", "long", "void", "typedef", "_Bool",
                "enum", "static", "goto", "break", "continue", "switch", "case",
                "default", "extern", "_Alignof", "_Alignas", "do", "signed",
                "unsigned", "const", "volatile", "auto", "register", "restrict",
                "__restrict", "__restrict__", "_Noreturn", "float", "double",
                "typeof", "asm", "_Thread_local", "__thread", "_Atomic",
                "__attribute__"),
            ReservedOpNames: List("+", "-", "*", "/", "="),
            CaseSensitive: true);

        public static GenTokenParser Lexer = makeTokenParser(Def);


        public static Parser<CSyntaxNode> IncludeDirective = from include in Lexer.Reserved("#include")
                                                             from path in asString(manyUntil(anyChar, endOfLine))
                                                             select new IncludeDirective(path) as CSyntaxNode;

        public static Parser<CSyntaxNode> DefineDirective = from define in Lexer.Reserved("#define")
                                                            from defined in Lexer.Identifier
                                                            from args in many(Lexer.Identifier)
                                                            select new DefineDirective(defined, args) as CSyntaxNode;

        public static Parser<CSyntaxNode> EndifDirective = from endif in Lexer.Reserved("#endif")
                                                           select new EndifDirective() as CSyntaxNode;

        public static Parser<CSyntaxNode> IfndefDirective = from ifndef in Lexer.Reserved("#ifndef")
                                                            from c in Lexer.Identifier
                                                            select new IfndefDirective(c) as CSyntaxNode;

        public static Parser<CSyntaxNode> IfdefDirective = from ifdef in Lexer.Reserved("#ifdef")
                                                           from c in Lexer.Identifier
                                                           select new IfdefDirective(c) as CSyntaxNode;

        public static Parser<CSyntaxNode> DeclarationList = from a in Lexer.Braces(many(_blockItem))
                                                            select new DeclarationList(a) as CSyntaxNode;

        public static Parser<CSyntaxNode> LinkageSpecification = from @extern in Lexer.Reserved("extern")
                                                                 from value in Lexer.StringLiteral
                                                                 from body in choice(DeclarationList)
                                                                 select new LinkageSpecification(value, body) as CSyntaxNode;

        public static Parser<CSyntaxNode> PreprocessorDirective = lazyp(() => from w in Lexer.WhiteSpace
                                                                              from p in choice(IfndefDirective, IfdefDirective, IncludeDirective, DefineDirective, EndifDirective)
                                                                              select p);

        public static Parser<CSyntaxNode> PreprocessorCall = lazyp(() => from preproc in PreprocessorDirective
                                                                         select new PreprocessorCall(preproc as PreprocessorDirective.PreprocessorDirective) as CSyntaxNode);

        public static Parser<CSyntaxNode> PrimitiveType = from t in choice(List("void", "bool", "char", "int", "float", "double").Map(Lexer.Reserved).ToSeq())
                                                          select new PrimitiveType(t) as CSyntaxNode;

        public static Parser<CSyntaxNode> TypeQualifier = from t in choice(List("const", "constexpr", "volatile", "restrict", "__restrict__", "__extension__", "_Atomic", "_Noreturn", "noreturn").Map(Lexer.Reserved).ToSeq())
                                                          select new TypeQualifier(t) as CSyntaxNode;

        public static Parser<CSyntaxNode> Declarator = from i in _identifier
                                                       select new Declarator(i as Identifier) as CSyntaxNode;

        public static Parser<CSyntaxNode> FieldDeclarator = from i in _identifier
                                                            select new FieldDeclarator(i as Identifier) as CSyntaxNode;

        public static Parser<CSyntaxNode> PointerDeclarator = from a in Lexer.ReservedOp("*")
                                                              from declarator in optional(_declarator)
                                                              select new PointerDeclarator(declarator.Case as Declarator) as CSyntaxNode;

        public static Parser<CSyntaxNode> FieldPointerDeclarator = from a in Lexer.ReservedOp("*")
                                                                   from declarator in FieldDeclarator
                                                                   select new PointerFieldDeclarator(declarator as FieldDeclarator) as CSyntaxNode;

        public static Parser<CSyntaxNode> ParenthesizedDeclarator = from declarator in Lexer.Parens(_typeDeclarator)
                                                                    select new ParenthesizedDeclarator(declarator as Declarator) as CSyntaxNode;

        public static Parser<CSyntaxNode> FunctionDeclarator = from declarator in ParenthesizedDeclarator
                                                               from parameterList in ParameterListDeclaration
                                                               select new FunctionDeclarator(declarator as Declarator, parameterList) as CSyntaxNode;

        public static Parser<CSyntaxNode> IntegerLiteral = from i in Lexer.Integer
                                                           select new IntegerLiteral(i) as CSyntaxNode;

        public static Parser<CSyntaxNode> EnumConstant = from name in _identifier
                                                         from value in optional(from s in Lexer.Symbol("=")
                                                                                from v in choice(IntegerLiteral, _identifier)
                                                                                select v)
                                                         from s in optional(Lexer.Symbol(","))
                                                         select new EnumConstant(name as Identifier, value.IfNoneUnsafe(null as CSyntaxNode)) as CSyntaxNode;

        public static Parser<CSyntaxNode> TypeIdentifier = lazyp(() => choice(PrimitiveType, _identifier));


        public static Parser<CSyntaxNode> FieldDeclaration = from q1 in many(_declarationModifier)
                                                             from fieldType in TypeIdentifier
                                                             from q2 in many(_declarationModifier)
                                                             from declarator in _fieldDeclarator
                                                             from s in Lexer.Symbol(";")
                                                             select new FieldDeclaration(declarator as IFieldDeclarator, fieldType as ITypeIdentifier) as CSyntaxNode;

        public static Parser<CSyntaxNode> StructSpecifier = from a in Lexer.Reserved("struct")
                                                            from name in optional(_identifier)
                                                            from b in Lexer.Reserved("{")
                                                            from fields in many(FieldDeclaration)
                                                            from c in Lexer.Reserved("}")
                                                            select new StructSpecifier(name.IfNoneUnsafe(null as CSyntaxNode) as ITypeIdentifier, fields.OfType<FieldDeclaration>()) as CSyntaxNode;

        public static Parser<CSyntaxNode> UnionSpecifier = from a in Lexer.Reserved("union")
                                                           from name in optional(_identifier)
                                                           from b in Lexer.Reserved("{")
                                                           from fields in many(FieldDeclaration)
                                                           from c in Lexer.Reserved("}")
                                                           select new UnionSpecifier(name.IfNoneUnsafe(null as CSyntaxNode) as ITypeIdentifier, fields.OfType<FieldDeclaration>()) as CSyntaxNode;

        public static Parser<CSyntaxNode> EnumSpecifier = from a in Lexer.Reserved("enum")
                                                          from name in optional(_identifier)
                                                          from b in Lexer.Reserved("{")
                                                          from constants in many(EnumConstant)
                                                          from c in Lexer.Reserved("}")
                                                          select new EnumSpecifier(name.IfNoneUnsafe(null as CSyntaxNode) as ITypeIdentifier, constants.OfType<EnumConstant>()) as CSyntaxNode;


        public static Parser<CSyntaxNode> TypeSpecifier = from type in TypeIdentifier
                                                          select new TypeSpecifier(type as ITypeIdentifier) as CSyntaxNode;

        public static Parser<ParameterDeclaration> ParameterDeclaration = from q1 in many(_declarationModifier)
                                                                          from type in TypeIdentifier
                                                                          from q2 in many(_declarationModifier)
                                                                          from declarator in optional(_declarator)
                                                                          select new ParameterDeclaration(declarator.IfNoneUnsafe((CSyntaxNode?)null) as Declarator, type as ITypeIdentifier);

        public static Parser<ParameterListDeclaration> ParameterListDeclaration = from parameters in Lexer.ParensCommaSep(ParameterDeclaration)
                                                                                  select new ParameterListDeclaration(parameters);

        public static Parser<CSyntaxNode> TypeDefinition = from a in Lexer.Reserved("typedef")
                                                           from q1 in many(TypeQualifier)
                                                           from specifier in choice(TypeSpecifier, StructSpecifier, EnumSpecifier, UnionSpecifier)
                                                           from q2 in many(TypeQualifier)
                                                           from declarator in Lexer.CommaSep(_typeDeclarator)
                                                           from b in Lexer.Symbol(";")
                                                           select new TypeDefinition(declarator.OfType<Declarator>(), specifier as TypeSpecifier) as CSyntaxNode;

        public static Parser<TranslationUnit> TranslationUnit = from w in Lexer.WhiteSpace
                                                                from a in many(_blockItem)
                                                                from e in eof
                                                                select new TranslationUnit(a);
    }
}
