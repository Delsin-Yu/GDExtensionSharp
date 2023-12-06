using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static void GenerateProperty(
        StringBuilder stringBuilder,
        IReadOnlyList<Property> engineClassProperties,
        IReadOnlyCollection<string> godotTypes,
        IReadOnlyCollection<string> refCountedGodotTypes,
        ICollection<string> usedPropertyAccessor,
        IReadOnlyCollection<string> virtualAccessors,
        IReadOnlyDictionary<string, string> methodReturnTypeMap)
    {
        if (engineClassProperties == null) return;

        foreach (Property property in engineClassProperties)
        {
            string correctType = CorrectType(property.Type);

            if (!methodReturnTypeMap.TryGetValue(property.Getter, out string propertyType))
            {
                propertyType = GetInteropHandler(correctType, godotTypes, refCountedGodotTypes).CsharpType;
            }

            string propertyGetter = property.Getter.SnakeCaseToPascalCase();
            string propertySetter = property.Setter.SnakeCaseToPascalCase();

            if (virtualAccessors.Contains(property.Getter)) propertyGetter = "_" + propertyGetter;
            if (virtualAccessors.Contains(property.Setter)) propertySetter = "_" + propertySetter;

            usedPropertyAccessor.Add(propertyGetter);
            usedPropertyAccessor.Add(propertySetter);

            stringBuilder
                .AppendIndentLine($$"""
                                    public {{propertyType}} {{property.Name.SnakeCaseToPascalCase()}}
                                    {
                                        get => {{propertyGetter}}();
                                        set => {{propertySetter}}(value);
                                    }

                                    """);
        }
    }
}
