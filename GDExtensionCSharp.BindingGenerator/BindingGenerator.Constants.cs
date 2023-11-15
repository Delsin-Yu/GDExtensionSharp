namespace GDExtensionCSharp.BindingGenerator;

public static partial class BindingGenerator
{
    private const string FileHeader =
        """
        // ReSharper disable RedundantUsingDirective.Global
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo
        // ReSharper disable CommentTypo
        // ReSharper disable InvalidXmlDocComment
        // ReSharper disable RedundantUnsafeContext

        #pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        #pragma warning disable CS0169 // Field is never used

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
        global using char16_t = System.UInt16;
        global using char32_t = System.UInt32;

        global using GDExtensionInt = System.Int64;
        global using GDExtensionBool = System.Byte;
        global using GDObjectInstanceID = System.UInt64;

        using System.Runtime.InteropServices;


        """;

    private const string MethodTableStructHeader =
        $$"""
          internal unsafe struct {{MethodTableStructName}}
          {

          """;

    private const string MethodTableStructName = "MethodTable";

    private const string DocumentationCommentReturn = "/// <returns>{0}</returns>";
    private const string GetDelegateCallbackName = "p_get_proc_address";
    private const string UtilityFunctionName = "GetMethod";

    private const string UtilityFunctionBody =
        $$"""
          private static void* {{UtilityFunctionName}}(string methodName, delegate* unmanaged <char*, delegate* unmanaged <void>> {{GetDelegateCallbackName}})
          {
              var bstrString = Marshal.StringToBSTR(methodName);
              var methodPointer = p_get_proc_address((char*)bstrString);
              Marshal.FreeBSTR(bstrString);
              return methodPointer;
          }
          """;

    private const string MethodTableStructConstructorHeader =
        "internal {0}(delegate* unmanaged <char*, delegate* unmanaged <void>> {1})";
}
