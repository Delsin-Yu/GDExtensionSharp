using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GDExtensionSharp.SourceGenerators;
internal static class Extensions
{
    public static T If<T>(this T @this, bool condition, Func<T, T> func) where T : SyntaxNode => condition ? func(@this) : @this;
}
