namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private interface IInteropHandler
    {
        string CsharpType { get; }
        string ReturnInteropTypeDeclare { get; }
        string ReturnInteropType { get; }
        string ParamInteropTypeDeclare { get; }
        string ParamInteropType { get; }
        string CsharpTypeToInteropTypeArgs { get; }
        string InteropTypeToCSharpTypeArgs { get; }
    }

    private class NonInteropHandler(string type) : IInteropHandler
    {
        public string CsharpType => type;
        public string ReturnInteropTypeDeclare => type;
        public string ReturnInteropType => null;
        public string ParamInteropTypeDeclare => null;
        public string ParamInteropType => null;
        public string CsharpTypeToInteropTypeArgs => "";
        public string InteropTypeToCSharpTypeArgs => "{0}";
    }

    private class InteropHandlerWithConversion(IInteropHandler handler, string convertTarget) : IInteropHandler
    {
        public string CsharpType { get; } = convertTarget;
        public string ReturnInteropTypeDeclare { get; } = handler.ReturnInteropTypeDeclare;
        public string ReturnInteropType { get; } = handler.ReturnInteropType;
        public string ParamInteropTypeDeclare { get; } = handler.ParamInteropTypeDeclare;
        public string ParamInteropType { get; } = handler.ParamInteropType;
        public string CsharpTypeToInteropTypeArgs { get; } = handler.CsharpTypeToInteropTypeArgs;
        public string InteropTypeToCSharpTypeArgs { get; } = $"({convertTarget}){handler.InteropTypeToCSharpTypeArgs}";
    }

    private class ArrayInteropHandler(string csharpArrayType, string interopType, string convertMethodName) : IInteropHandler
    {
        string IInteropHandler.CsharpType { get; } = $"{csharpArrayType}[]";
        string IInteropHandler.ReturnInteropTypeDeclare { get; } = $"using {interopType}";
        string IInteropHandler.ReturnInteropType { get; } = interopType;
        string IInteropHandler.ParamInteropTypeDeclare { get; } = $"using {interopType}";
        string IInteropHandler.ParamInteropType { get; } = interopType;
        string IInteropHandler.CsharpTypeToInteropTypeArgs { get; } = $"Marshaling.ConvertSystemArrayToNativePacked{convertMethodName}Array({{0}})";
        string IInteropHandler.InteropTypeToCSharpTypeArgs { get; } = $"Marshaling.ConvertNativePacked{convertMethodName}ArrayToSystemArray({{0}})";
    }

    private class InteropHandler(string csharpType, string returnInteropType, string csharpTypeToInteropTypeArgs, string interopTypeToCSharpTypeArgs, bool scoped = false, string paramInteropType = null, bool? paramInteropScoped = null) : IInteropHandler
    {
        string IInteropHandler.CsharpType => csharpType;
        string IInteropHandler.ReturnInteropTypeDeclare { get; } = scoped ? $"using {returnInteropType}" : returnInteropType;
        string IInteropHandler.ReturnInteropType { get; } = returnInteropType;
        string IInteropHandler.ParamInteropTypeDeclare { get; } = paramInteropScoped ?? scoped ? $"using {paramInteropType ?? returnInteropType}" : paramInteropType ?? returnInteropType;
        string IInteropHandler.ParamInteropType { get; } = paramInteropType ?? returnInteropType;
        string IInteropHandler.CsharpTypeToInteropTypeArgs => csharpTypeToInteropTypeArgs;
        string IInteropHandler.InteropTypeToCSharpTypeArgs => interopTypeToCSharpTypeArgs;
    }

    private static readonly Dictionary<string, IInteropHandler> _interopHandler = new()
    {
        {
            "GodotObject", new InteropHandler(null,
                "IntPtr",
                "GodotObject.GetPtr({0})",
                "InteropUtils.UnmanagedGetManaged({0})")
        },
        {
            "RefCounted", new InteropHandler(null,
                "godot_ref",
                "GodotObject.GetPtr({0})",
                "InteropUtils.UnmanagedGetManaged({0}.Reference)",
                true,
                "IntPtr",
                false)
        },
        {
            "StringName", new InteropHandler("StringName",
                "godot_string_name",
                "(godot_string_name)({0}?.NativeValue ?? default)",
                "StringName.CreateTakingOwnershipOfDisposableValue({0})")
        },
        {
            "String", new InteropHandler("string",
                "godot_string",
                "Marshaling.ConvertStringToNative({0})",
                "Marshaling.ConvertStringToManaged({0})",
                true)
        },
        {
            "Variant", new InteropHandler("Variant",
                "godot_variant",
                "(godot_variant){0}.NativeVar",
                "Variant.CreateTakingOwnershipOfDisposableValue({0})")
        },
        {
            "Callable", new InteropHandler("Callable",
                "godot_callable",
                "Marshaling.ConvertCallableToNative(in {0})",
                "Marshaling.ConvertCallableToManaged(in {0})")
        },
        {
            "Array", new InteropHandler("Godot.Collections.Array",
                "godot_array",
                "(godot_array)({0} ?? new()).NativeValue",
                "Godot.Collections.Array.CreateTakingOwnershipOfDisposableValue({0})")
        },
        {
            "Dictionary", new InteropHandler("Godot.Collections.Dictionary",
                "godot_dictionary",
                "(godot_dictionary)({0} ?? new()).NativeValue",
                "Godot.Collections.Dictionary.CreateTakingOwnershipOfDisposableValue({0})")
        },
        {
            "NodePath", new InteropHandler("NodePath",
                "godot_node_path",
                "(godot_node_path)({0}?.NativeValue ?? default)",
                "NodePath.CreateTakingOwnershipOfDisposableValue({0})")
        },
        {
            "PackedStringArray", new ArrayInteropHandler("string",
                "godot_packed_string_array",
                "String")
        },
        {
            "PackedFloat32Array", new ArrayInteropHandler("float",
                "godot_packed_float32_array",
                "Float32")
        },
        {
            "PackedByteArray", new ArrayInteropHandler("byte",
                "godot_packed_byte_array",
                "Byte")
        },
        {
            "PackedInt32Array", new ArrayInteropHandler("int",
                "godot_packed_int32_array",
                "Int32")
        },
        {
            "PackedInt64Array", new ArrayInteropHandler("long",
                "godot_packed_int64_array",
                "Int64")
        },
        {
            "PackedVector2Array", new ArrayInteropHandler("Vector2",
                "godot_packed_vector2_array",
                "Vector2")
        },
        {
            "PackedVector3Array", new ArrayInteropHandler("Vector3",
                "godot_packed_vector3_array",
                "Vector3")
        },
    };
}
