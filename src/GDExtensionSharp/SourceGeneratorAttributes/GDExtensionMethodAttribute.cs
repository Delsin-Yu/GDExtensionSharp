namespace GDExtensionSharp;


[AttributeUsage(AttributeTargets.Parameter)]
public class MarshalToBytePtrAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class GDExtensionMethodAttribute : Attribute
{
    public string NativeName { get; init; }
    public GDExtensionMethodAttribute(string nativeName) => NativeName = nativeName;
}

[AttributeUsage(AttributeTargets.Method)]
public class GDExtensionNotSupportedAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
internal class GenerateUnmanagedCallbacksAttribute : Attribute
{
    public Type FuncStructType { get; }

    public GenerateUnmanagedCallbacksAttribute(Type funcStructType)
    {
        FuncStructType = funcStructType;
    }
}

[AttributeUsage(AttributeTargets.Class)]
internal class GenerateUnmanagedBindingAttribute : Attribute
{
}
