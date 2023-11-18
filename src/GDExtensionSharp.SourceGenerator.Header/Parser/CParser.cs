using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt.Parsec;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class CParser
    {
        public TranslationUnit Parse(string source)
        {
            var result = Prim.parse(Parser.TranslationUnit, source);
            if (result.IsFaulted)
            {
                throw new Exception(result.Reply.Error.ToString());
            }

            return result.Reply.Result;
        }
    }
}
