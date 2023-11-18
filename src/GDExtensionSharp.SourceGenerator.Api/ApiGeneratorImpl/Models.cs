
namespace GDExtensionSharp.SourceGenerator.Api;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class GDExtensionApi
{
    [JsonPropertyName("header")] public Header Header { get; set; }

    [JsonPropertyName("builtin_class_sizes")] public BuiltinClassSize[] BuiltinClassSizes { get; set; }

    [JsonPropertyName("builtin_class_member_offsets")] public BuiltinClassMemberOffset[] BuiltinClassMemberOffsets { get; set; }

    [JsonPropertyName("global_constants")] public GlobalConstantElement[] GlobalConstants { get; set; }

    [JsonPropertyName("global_enums")] public GlobalEnumElement[] GlobalEnums { get; set; }

    [JsonPropertyName("utility_functions")] public UtilityFunction[] UtilityFunctions { get; set; }

    [JsonPropertyName("builtin_classes")] public BuiltinClass[] BuiltinClasses { get; set; }

    [JsonPropertyName("classes")] public Class[] Classes { get; set; }

    [JsonPropertyName("singletons")] public Singleton[] Singletons { get; set; }

    [JsonPropertyName("native_structures")] public NativeStructure[] NativeStructures { get; set; }
}

public class BuiltinClassMemberOffset
{
    [JsonPropertyName("build_configuration")] public string BuildConfiguration { get; set; }

    [JsonPropertyName("classes")] public BuiltinClassMemberOffsetClass[] Classes { get; set; }
}

public class BuiltinClassMemberOffsetClass
{
    [JsonPropertyName("name")] public TypeEnum Name { get; set; }

    [JsonPropertyName("members")] public Member[] Members { get; set; }
}

public class Member
{
    [JsonPropertyName("member")] public string MemberMember { get; set; }

    [JsonPropertyName("offset")] public long Offset { get; set; }

    [JsonPropertyName("meta")] public MemberMeta Meta { get; set; }
}

public class BuiltinClassSize
{
    [JsonPropertyName("build_configuration")] public string BuildConfiguration { get; set; }

    [JsonPropertyName("sizes")] public Size[] Sizes { get; set; }
}

public class Size
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("size")] public long SizeSize { get; set; }
}

public class BuiltinClass
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("is_keyed")] public bool IsKeyed { get; set; }

    [JsonPropertyName("operators")] public Operator[] Operators { get; set; }

    [JsonPropertyName("constructors")] public Constructor[] Constructors { get; set; }

    [JsonPropertyName("has_destructor")] public bool HasDestructor { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("indexing_return_type")]
    public string IndexingReturnType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("methods")]
    public BuiltinClassMethod[] Methods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("members")]
    public Singleton[] Members { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("constants")]
    public BuiltinClassConstant[] Constants { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("enums")]
    public BuiltinClassEnum[] Enums { get; set; }
}

public class BuiltinClassConstant
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("type")] public TypeEnum Type { get; set; }

    [JsonPropertyName("value")] public string Value { get; set; }
}

public class Constructor
{
    [JsonPropertyName("index")] public long Index { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("arguments")]
    public Singleton[] Arguments { get; set; }
}

public class Singleton
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }
    
    [JsonPropertyName("meta")] public string Meta { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("default_value")] public string DefaultValue { get; set; }
}

public class BuiltinClassEnum
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("values")] public ValueElement[] Values { get; set; }
}

public class ValueElement
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("value")] public long Value { get; set; }
}

public class BuiltinClassMethod
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("return_type")]
    public string ReturnType { get; set; }

    [JsonPropertyName("is_vararg")] public bool IsVararg { get; set; }

    [JsonPropertyName("is_const")] public bool IsConst { get; set; }

    [JsonPropertyName("is_static")] public bool IsStatic { get; set; }

    [JsonPropertyName("hash")] public long Hash { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("arguments")]
    public Argument[] Arguments { get; set; }
}

public class Argument
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("default_value")]
    public string DefaultValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("meta")]
    public ArgumentMeta? Meta { get; set; }
}

public class Operator
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("right_type")]
    public string RightType { get; set; }

    [JsonPropertyName("return_type")] public string ReturnType { get; set; }
}

public class Class
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("is_refcounted")] public bool IsRefcounted { get; set; }

    [JsonPropertyName("is_instantiable")] public bool IsInstantiable { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("inherits")]
    public string Inherits { get; set; }

    [JsonPropertyName("api_type")] public ApiType ApiType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("enums")]
    public GlobalEnumElement[] Enums { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("methods")]
    public ClassMethod[] Methods { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("properties")]
    public Property[] Properties { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("signals")]
    public Signal[] Signals { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("constants")]
    public ValueElement[] Constants { get; set; }
}

public class GlobalConstantElement
{
    [JsonPropertyName("name")] public string Name { get; set; }
    
    [JsonPropertyName("value")] public long Value { get; set; }
}

public class GlobalEnumElement
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("is_bitfield")] public bool IsBitfield { get; set; }

    [JsonPropertyName("values")] public ValueElement[] Values { get; set; }
}

public class ClassMethod
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("is_const")] public bool IsConst { get; set; }

    [JsonPropertyName("is_vararg")] public bool IsVararg { get; set; }

    [JsonPropertyName("is_static")] public bool IsStatic { get; set; }

    [JsonPropertyName("is_virtual")] public bool IsVirtual { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("hash")]
    public long? Hash { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("return_value")]
    public ReturnValue ReturnValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("arguments")]
    public Argument[] Arguments { get; set; }
}

public class ReturnValue
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("meta")]
    public ArgumentMeta? Meta { get; set; }
}

public class Property
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("setter")]
    public string Setter { get; set; }

    [JsonPropertyName("getter")] public string Getter { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("index")]
    public long? Index { get; set; }
}

public class Signal
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("arguments")]
    public Singleton[] Arguments { get; set; }
}

public class Header
{
    [JsonPropertyName("version_major")] public long VersionMajor { get; set; }

    [JsonPropertyName("version_minor")] public long VersionMinor { get; set; }

    [JsonPropertyName("version_patch")] public long VersionPatch { get; set; }

    [JsonPropertyName("version_status")] public string VersionStatus { get; set; }

    [JsonPropertyName("version_build")] public string VersionBuild { get; set; }

    [JsonPropertyName("version_full_name")] public string VersionFullName { get; set; }
}

public class NativeStructure
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("format")] public string Format { get; set; }
}

public class UtilityFunction
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("return_type")]
    public ReturnType? ReturnType { get; set; }

    [JsonPropertyName("category")] public Category Category { get; set; }

    [JsonPropertyName("is_vararg")] public bool IsVararg { get; set; }

    [JsonPropertyName("hash")] public long Hash { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("arguments")]
    public Singleton[] Arguments { get; set; }
}

public enum MemberMeta
{
    Basis,
    Double,
    Float,
    Int32,
    Vector2,
    Vector2I,
    Vector3,
    Vector4
};

public enum TypeEnum
{
    Aabb,
    Basis,
    Color,
    Int,
    Plane,
    Projection,
    Quaternion,
    Rect2,
    Rect2I,
    Transform2D,
    Transform3D,
    Vector2,
    Vector2I,
    Vector3,
    Vector3I,
    Vector4,
    Vector4I
};

public enum ArgumentMeta
{
    Double,
    Float,
    Int16,
    Int32,
    Int64,
    Int8,
    Uint16,
    Uint32,
    Uint64,
    Uint8
};

public enum ApiType { Core, Editor };

public enum Category { General, Math, Random };

public enum ReturnType
{
    Bool,
    Float,
    Int,
    Object,
    PackedByteArray,
    PackedInt64Array,
    Rid,
    String,
    Variant
};

internal static class Converter
{
    public static readonly JsonSerializerOptions Settings =
        new(JsonSerializerDefaults.General)
        {
            Converters =
            {
                MemberMetaConverter.Singleton,
                TypeEnumConverter.Singleton,
                ArgumentMetaConverter.Singleton,
                ApiTypeConverter.Singleton,
                CategoryConverter.Singleton,
                ReturnTypeConverter.Singleton,
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
                IsoDateTimeOffsetConverter.Singleton
            }
        };
}

internal class MemberMetaConverter : JsonConverter<MemberMeta>
{
    public override bool CanConvert(Type t) => t == typeof(MemberMeta);

    public override MemberMeta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "Basis":
                return MemberMeta.Basis;
            case "Vector2":
                return MemberMeta.Vector2;
            case "Vector2i":
                return MemberMeta.Vector2I;
            case "Vector3":
                return MemberMeta.Vector3;
            case "Vector4":
                return MemberMeta.Vector4;
            case "double":
                return MemberMeta.Double;
            case "float":
                return MemberMeta.Float;
            case "int32":
                return MemberMeta.Int32;
        }

        throw new Exception("Cannot unmarshal type MemberMeta");
    }

    public override void Write(Utf8JsonWriter writer, MemberMeta value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case MemberMeta.Basis:
                JsonSerializer.Serialize(writer, "Basis", options);
                return;
            case MemberMeta.Vector2:
                JsonSerializer.Serialize(writer, "Vector2", options);
                return;
            case MemberMeta.Vector2I:
                JsonSerializer.Serialize(writer, "Vector2i", options);
                return;
            case MemberMeta.Vector3:
                JsonSerializer.Serialize(writer, "Vector3", options);
                return;
            case MemberMeta.Vector4:
                JsonSerializer.Serialize(writer, "Vector4", options);
                return;
            case MemberMeta.Double:
                JsonSerializer.Serialize(writer, "double", options);
                return;
            case MemberMeta.Float:
                JsonSerializer.Serialize(writer, "float", options);
                return;
            case MemberMeta.Int32:
                JsonSerializer.Serialize(writer, "int32", options);
                return;
        }

        throw new Exception("Cannot marshal type MemberMeta");
    }

    public static readonly MemberMetaConverter Singleton = new MemberMetaConverter();
}

internal class TypeEnumConverter : JsonConverter<TypeEnum>
{
    public override bool CanConvert(Type t) => t == typeof(TypeEnum);

    public override TypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "AABB":
                return TypeEnum.Aabb;
            case "Basis":
                return TypeEnum.Basis;
            case "Color":
                return TypeEnum.Color;
            case "Plane":
                return TypeEnum.Plane;
            case "Projection":
                return TypeEnum.Projection;
            case "Quaternion":
                return TypeEnum.Quaternion;
            case "Rect2":
                return TypeEnum.Rect2;
            case "Rect2i":
                return TypeEnum.Rect2I;
            case "Transform2D":
                return TypeEnum.Transform2D;
            case "Transform3D":
                return TypeEnum.Transform3D;
            case "Vector2":
                return TypeEnum.Vector2;
            case "Vector2i":
                return TypeEnum.Vector2I;
            case "Vector3":
                return TypeEnum.Vector3;
            case "Vector3i":
                return TypeEnum.Vector3I;
            case "Vector4":
                return TypeEnum.Vector4;
            case "Vector4i":
                return TypeEnum.Vector4I;
            case "int":
                return TypeEnum.Int;
        }

        throw new Exception("Cannot unmarshal type TypeEnum");
    }

    public override void Write(Utf8JsonWriter writer, TypeEnum value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case TypeEnum.Aabb:
                JsonSerializer.Serialize(writer, "AABB", options);
                return;
            case TypeEnum.Basis:
                JsonSerializer.Serialize(writer, "Basis", options);
                return;
            case TypeEnum.Color:
                JsonSerializer.Serialize(writer, "Color", options);
                return;
            case TypeEnum.Plane:
                JsonSerializer.Serialize(writer, "Plane", options);
                return;
            case TypeEnum.Projection:
                JsonSerializer.Serialize(writer, "Projection", options);
                return;
            case TypeEnum.Quaternion:
                JsonSerializer.Serialize(writer, "Quaternion", options);
                return;
            case TypeEnum.Rect2:
                JsonSerializer.Serialize(writer, "Rect2", options);
                return;
            case TypeEnum.Rect2I:
                JsonSerializer.Serialize(writer, "Rect2i", options);
                return;
            case TypeEnum.Transform2D:
                JsonSerializer.Serialize(writer, "Transform2D", options);
                return;
            case TypeEnum.Transform3D:
                JsonSerializer.Serialize(writer, "Transform3D", options);
                return;
            case TypeEnum.Vector2:
                JsonSerializer.Serialize(writer, "Vector2", options);
                return;
            case TypeEnum.Vector2I:
                JsonSerializer.Serialize(writer, "Vector2i", options);
                return;
            case TypeEnum.Vector3:
                JsonSerializer.Serialize(writer, "Vector3", options);
                return;
            case TypeEnum.Vector3I:
                JsonSerializer.Serialize(writer, "Vector3i", options);
                return;
            case TypeEnum.Vector4:
                JsonSerializer.Serialize(writer, "Vector4", options);
                return;
            case TypeEnum.Vector4I:
                JsonSerializer.Serialize(writer, "Vector4i", options);
                return;
            case TypeEnum.Int:
                JsonSerializer.Serialize(writer, "int", options);
                return;
        }

        throw new Exception("Cannot marshal type TypeEnum");
    }

    public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
}

internal class ArgumentMetaConverter : JsonConverter<ArgumentMeta>
{
    public override bool CanConvert(Type t) => t == typeof(ArgumentMeta);

    public override ArgumentMeta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "double":
                return ArgumentMeta.Double;
            case "float":
                return ArgumentMeta.Float;
            case "int16":
                return ArgumentMeta.Int16;
            case "int32":
                return ArgumentMeta.Int32;
            case "int64":
                return ArgumentMeta.Int64;
            case "int8":
                return ArgumentMeta.Int8;
            case "uint16":
                return ArgumentMeta.Uint16;
            case "uint32":
                return ArgumentMeta.Uint32;
            case "uint64":
                return ArgumentMeta.Uint64;
            case "uint8":
                return ArgumentMeta.Uint8;
        }

        throw new Exception("Cannot unmarshal type ArgumentMeta");
    }

    public override void Write(Utf8JsonWriter writer, ArgumentMeta value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ArgumentMeta.Double:
                JsonSerializer.Serialize(writer, "double", options);
                return;
            case ArgumentMeta.Float:
                JsonSerializer.Serialize(writer, "float", options);
                return;
            case ArgumentMeta.Int16:
                JsonSerializer.Serialize(writer, "int16", options);
                return;
            case ArgumentMeta.Int32:
                JsonSerializer.Serialize(writer, "int32", options);
                return;
            case ArgumentMeta.Int64:
                JsonSerializer.Serialize(writer, "int64", options);
                return;
            case ArgumentMeta.Int8:
                JsonSerializer.Serialize(writer, "int8", options);
                return;
            case ArgumentMeta.Uint16:
                JsonSerializer.Serialize(writer, "uint16", options);
                return;
            case ArgumentMeta.Uint32:
                JsonSerializer.Serialize(writer, "uint32", options);
                return;
            case ArgumentMeta.Uint64:
                JsonSerializer.Serialize(writer, "uint64", options);
                return;
            case ArgumentMeta.Uint8:
                JsonSerializer.Serialize(writer, "uint8", options);
                return;
        }

        throw new Exception("Cannot marshal type ArgumentMeta");
    }

    public static readonly ArgumentMetaConverter Singleton = new ArgumentMetaConverter();
}

internal class ApiTypeConverter : JsonConverter<ApiType>
{
    public override bool CanConvert(Type t) => t == typeof(ApiType);

    public override ApiType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "core":
                return ApiType.Core;
            case "editor":
                return ApiType.Editor;
        }

        throw new Exception("Cannot unmarshal type ApiType");
    }

    public override void Write(Utf8JsonWriter writer, ApiType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ApiType.Core:
                JsonSerializer.Serialize(writer, "core", options);
                return;
            case ApiType.Editor:
                JsonSerializer.Serialize(writer, "editor", options);
                return;
        }

        throw new Exception("Cannot marshal type ApiType");
    }

    public static readonly ApiTypeConverter Singleton = new ApiTypeConverter();
}

internal class CategoryConverter : JsonConverter<Category>
{
    public override bool CanConvert(Type t) => t == typeof(Category);

    public override Category Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "general":
                return Category.General;
            case "math":
                return Category.Math;
            case "random":
                return Category.Random;
        }

        throw new Exception("Cannot unmarshal type Category");
    }

    public override void Write(Utf8JsonWriter writer, Category value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case Category.General:
                JsonSerializer.Serialize(writer, "general", options);
                return;
            case Category.Math:
                JsonSerializer.Serialize(writer, "math", options);
                return;
            case Category.Random:
                JsonSerializer.Serialize(writer, "random", options);
                return;
        }

        throw new Exception("Cannot marshal type Category");
    }

    public static readonly CategoryConverter Singleton = new CategoryConverter();
}

internal class ReturnTypeConverter : JsonConverter<ReturnType>
{
    public override bool CanConvert(Type t) => t == typeof(ReturnType);

    public override ReturnType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "Object":
                return ReturnType.Object;
            case "PackedByteArray":
                return ReturnType.PackedByteArray;
            case "PackedInt64Array":
                return ReturnType.PackedInt64Array;
            case "RID":
                return ReturnType.Rid;
            case "String":
                return ReturnType.String;
            case "Variant":
                return ReturnType.Variant;
            case "bool":
                return ReturnType.Bool;
            case "float":
                return ReturnType.Float;
            case "int":
                return ReturnType.Int;
        }

        throw new Exception("Cannot unmarshal type ReturnType");
    }

    public override void Write(Utf8JsonWriter writer, ReturnType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ReturnType.Object:
                JsonSerializer.Serialize(writer, "Object", options);
                return;
            case ReturnType.PackedByteArray:
                JsonSerializer.Serialize(writer, "PackedByteArray", options);
                return;
            case ReturnType.PackedInt64Array:
                JsonSerializer.Serialize(writer, "PackedInt64Array", options);
                return;
            case ReturnType.Rid:
                JsonSerializer.Serialize(writer, "RID", options);
                return;
            case ReturnType.String:
                JsonSerializer.Serialize(writer, "String", options);
                return;
            case ReturnType.Variant:
                JsonSerializer.Serialize(writer, "Variant", options);
                return;
            case ReturnType.Bool:
                JsonSerializer.Serialize(writer, "bool", options);
                return;
            case ReturnType.Float:
                JsonSerializer.Serialize(writer, "float", options);
                return;
            case ReturnType.Int:
                JsonSerializer.Serialize(writer, "int", options);
                return;
        }

        throw new Exception("Cannot marshal type ReturnType");
    }

    public static readonly ReturnTypeConverter Singleton = new ReturnTypeConverter();
}

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private readonly string serializationFormat;
    public DateOnlyConverter() : this(null) { }

    public DateOnlyConverter(string serializationFormat) => this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string serializationFormat;

    public TimeOnlyConverter() : this(null) { }

    public TimeOnlyConverter(string serializationFormat) => this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return TimeOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}

internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

    private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private string _dateTimeFormat;
    private CultureInfo _culture;

    public DateTimeStyles DateTimeStyles { get => _dateTimeStyles; set => _dateTimeStyles = value; }

    public string DateTimeFormat { get => _dateTimeFormat ?? string.Empty; set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value; }

    public CultureInfo Culture { get => _culture ?? CultureInfo.InvariantCulture; set => _culture = value; }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        string text;


        if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
        {
            value = value.ToUniversalTime();
        }

        text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

        writer.WriteStringValue(text);
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateText = reader.GetString();

        if (string.IsNullOrEmpty(dateText) == false)
        {
            if (!string.IsNullOrEmpty(_dateTimeFormat))
            {
                return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
            }
            else
            {
                return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
            }
        }
        else
        {
            return default(DateTimeOffset);
        }
    }


    public static readonly IsoDateTimeOffsetConverter Singleton = new IsoDateTimeOffsetConverter();
}
