using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static void GenerateClassHeader(StringBuilder stringBuilder, bool inherits, string engineClassName, Class engineClass)
    {
        if (inherits)
        {
            stringBuilder
                .AppendIndentLine($"private static readonly global::System.Type CachedType = typeof({engineClassName});")
                .AppendLine()
                .AppendIndentLine("""
                                  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                                  private static readonly unsafe delegate* unmanaged<IntPtr> NativeCtor = ClassDB_get_constructor(NativeName);
                                  """)
                .AppendLine()
                .AppendIndentLine($$"""
                                    public {{engineClassName}}() : this(true)
                                    {
                                        unsafe
                                        {
                                            _ConstructAndInitialize(NativeCtor, NativeName, CachedType, refCounted: {{engineClass.IsRefcounted.ToString().ToLower()}});
                                        }
                                    }
                                    """)
                .AppendLine()
                .AppendIndentLine($"internal {engineClassName}(bool memoryOwn) : base(memoryOwn) {{}}")
                .AppendLine();
        }
    }
}
