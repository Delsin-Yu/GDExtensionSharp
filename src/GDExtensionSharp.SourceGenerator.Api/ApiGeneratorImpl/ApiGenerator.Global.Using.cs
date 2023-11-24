namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static (string sourceName, string sourceContent) GenerateGlobalUsing()
    {
        const string content =
            """
            global using int8_t = System.SByte;
            global using int16_t = System.Int16;
            global using int32_t = System.Int32;
            global using int64_t = System.Int64;
            global using uint8_t = System.Byte;
            global using uint16_t = System.UInt16;
            global using uint32_t = System.UInt32;
            global using uint64_t = System.UInt64;

            global using size_t = System.UInt64;

            global using wchar_t = System.UInt16;

            #if REAL_T_IS_DOUBLE
            global using real_t = System.Double;
            #else
            global using real_t = System.Single;
            #endif

            """;

        return ("GlobalUsing", content);
    }
}
