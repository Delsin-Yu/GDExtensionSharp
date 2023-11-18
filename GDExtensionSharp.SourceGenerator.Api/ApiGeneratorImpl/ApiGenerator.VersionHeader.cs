using System.Globalization;

namespace GDExtensionSharp.SourceGenerator.Api;

internal static partial class ApiGenerator
{
    private static (string sourceName, string sourceContent) GenerateVersionHeader(Header header)
    {
        var headerContent =
            $$"""
              namespace Godot;

              public static class Header
              {
                  public const uint GODOT_VERSION_MAJOR = {{header.VersionMajor.ToString(CultureInfo.InvariantCulture)}};
                  public const uint GODOT_VERSION_MINOR = {{header.VersionMinor.ToString(CultureInfo.InvariantCulture)}};
                  public const uint GODOT_VERSION_PATCH = {{header.VersionPatch.ToString(CultureInfo.InvariantCulture)}};
                  public const string GODOT_VERSION_STATUS = "{{header.VersionStatus}}";
                  public const string GODOT_VERSION_BUILD = "{{header.VersionBuild}}";
                  public const string GODOT_VERSION_FULL_NAME = "{{header.VersionFullName}}";
              }
              """;

        return ("VersionHeader", headerContent);
    }
}
