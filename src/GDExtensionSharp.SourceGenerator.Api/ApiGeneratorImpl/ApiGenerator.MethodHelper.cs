namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string MethodHelper = "MethodHelper";

    private const string CallBuiltinConstructor = "CallBuiltinConstructor";
    private const string CallBuiltinMethodPointerReturn = "CallBuiltinMethodPointerReturn";
    private const string CallBuiltinMethodPointerNoReturn = "CallBuiltinMethodPointerNoReturn";
    private const string CallBuiltinOperatorPointer = "CallBuiltinOperatorPointer";
    private const string CallBuiltinPointerGetter = "CallBuiltinPointerGetter";

    private static (string sourceName, string sourceContent) GenerateMethodHelper()
    {
        const string content =
            $$"""
              namespace Godot;

              internal static unsafe class MethodHelper
              {
                  internal static void {{CallBuiltinConstructor}}(delegate* unmanaged<void*, void**, void> constructor, void* @base, params void*[] args)
                  {
                      var argList = stackalloc void*[args.Length];
                      for (var index = 0; index < args.Length; index++)
                      {
                          argList[index] = args[index];
                      }
                      constructor(@base, argList);
                  }
              
                  internal static T {{CallBuiltinMethodPointerReturn}}<T>(delegate* unmanaged<void*, void**, void*, int, void> method, void* @base, params void*[] args) where T : struct
                  {
                      
                      var argList = stackalloc void*[args.Length];
                      for (var index = 0; index < args.Length; index++)
                      {
                          argList[index] = args[index];
                      }
                      
                      T ret = default;
                      
                      method(@base, argList, &ret, args.Length);
                      return ret;
                  }
              
                  internal static void {{CallBuiltinMethodPointerNoReturn}}(delegate* unmanaged<void*, void**, void*, int, void> method, void* @base, params void*[] args)
                  {
                      var argList = stackalloc void*[args.Length];
                      for (var index = 0; index < args.Length; index++)
                      {
                          argList[index] = args[index];
                      }
                      method(@base, argList, null, args.Length);
                  }
              
                  internal static T {{CallBuiltinOperatorPointer}}<T>(delegate* unmanaged<void*, void*, void*, void> op, void* left, void* right) where T : struct
                  {
                      T ret;
                      op(left, right, &ret);
                      return ret;
                  }
              
                  internal static T {{CallBuiltinPointerGetter}}<T>(delegate* unmanaged<void*, void*, void> getter, void* @base) where T : struct
                  {
                      T ret;
                      getter(@base, &ret);
                      return ret;
                  }
              }
              """;

        return ("MethodHelper", content);
    }
}
