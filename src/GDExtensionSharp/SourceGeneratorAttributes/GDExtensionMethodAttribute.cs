namespace GDExtensionSharp;

[AttributeUsage(AttributeTargets.Method)]
public class GDExtensionMethodAttribute : Attribute
{
    public string NativeName { get; init; }

    public GDExtensionMethodAttribute(string nativeName) => NativeName = nativeName;
}
