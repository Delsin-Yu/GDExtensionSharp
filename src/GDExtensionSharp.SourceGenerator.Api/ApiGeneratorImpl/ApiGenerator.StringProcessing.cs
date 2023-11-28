using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static readonly Dictionary<StringBuilder, uint> _indentationData = new();
    private static readonly char[] _separator = ['\n'];


    private static readonly Regex _pascalToCamelRegex = new("^([A-Z]*)(?=[A-Z])", RegexOptions.Compiled);

    private static readonly Regex _pascalToSnakeRegex = new("(?<=[a-z0-9])([A-Z])|([A-Z][a-z])", RegexOptions.Compiled);

    private static readonly Regex _snakeToPascalRegex = new("(^|_)([a-z])", RegexOptions.Compiled);

    private static string SnakeCaseToPascalCase(this string input) => string.IsNullOrEmpty(input)
        ? input
        : _snakeToPascalRegex
            .Replace(input, m => m.Groups[2].Value.ToUpperInvariant());

    private static string PascalCaseToSnakeCase(this string input) => string.IsNullOrEmpty(input)
        ? input
        : _pascalToSnakeRegex
            .Replace(input, "_$0")
            .ToLower();

    private static string PascalCaseToCamelCase(this string input) => string.IsNullOrEmpty(input)
        ? input
        : _pascalToCamelRegex
            .Replace(input, match => match.Value.ToLowerInvariant());


    private readonly struct IndentDisposable(StringBuilder builder) : IDisposable
    {
        public void Dispose()
        {
            if (!_indentationData.ContainsKey(builder)) return;
            _indentationData[builder]--;

            if (_indentationData[builder] == 0)
            {
                _indentationData.Remove(builder);
            }
        }
    }

    private static IndentDisposable AddIndent(this StringBuilder stringBuilder)
    {
        if (!_indentationData.ContainsKey(stringBuilder))
        {
            _indentationData.Add(stringBuilder, 1);
        }
        else
        {
            _indentationData[stringBuilder]++;
        }

        return new(stringBuilder);
    }

    private static StringBuilder AppendIndentLine(this StringBuilder stringBuilder, string value = null) =>
        AppendIndent(stringBuilder, value).AppendLine();

    private static StringBuilder AppendIndent(this StringBuilder stringBuilder, string value = null)
    {
        value ??= string.Empty;

        if (!_indentationData.TryGetValue(stringBuilder, out uint indentation))
        {
            indentation = 0;
        }

        string[] lines = value.Replace("\r", string.Empty).Split(_separator);

        for (int index = 0; index < lines.Length; index++)
        {
            string line = lines[index];
            for (int i = 0; i < indentation; i++)
            {
                stringBuilder.Append("    ");
            }

            if (index == lines.Length - 1)
            {
                stringBuilder.Append(line);
            }
            else
            {
                stringBuilder.AppendLine(line);
            }
        }

        return stringBuilder;
    }

    private static string ToStringAndClear(this StringBuilder stringBuilder)
    {
        if (stringBuilder.Length == 0) return string.Empty;

        while (stringBuilder[^1] == '\n' || stringBuilder[^1] == '\r')
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        string result = stringBuilder.ToString();
        stringBuilder.Clear();
        return result;
    }
}
