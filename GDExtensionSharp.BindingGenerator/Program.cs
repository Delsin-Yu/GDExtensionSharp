// See https://aka.ms/new-console-template for more information

namespace GDExtensionCSharp.BindingGenerator;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // ReSharper disable once StringLiteralTypo
        const string sourceFileName = "gdextension_interface.h";
        const string generatedFileName = "gdextension_interface.cs";

        string sourceFilePath;

        if (args.Length == 0)
        {
            if (!File.Exists(sourceFileName))
            {
                WriteError($"{sourceFileName} not found!");
                return;
            }

            sourceFilePath = sourceFileName;
        }
        else
        {
            foreach (var arg in args.Where(File.Exists))
            {
                if (Path.GetFileName(arg) != sourceFileName) continue;
                sourceFilePath = arg;
                break;
            }

            WriteError($"{sourceFileName} not found!");
            return;
        }


        string fileContent;
        try
        {
            fileContent = await File.ReadAllTextAsync(sourceFilePath);
        }
        catch (Exception e)
        {
            WriteError(e);
            return;
        }

        string generatedCSharpBindings;
        try
        {
            generatedCSharpBindings = BindingGenerator.Generate(fileContent);
        }
        catch (Exception e)
        {
            WriteError(e);
            return;
        }

        await File.WriteAllTextAsync(generatedFileName, generatedCSharpBindings);
    }

    private static void WriteError(object error)
    {
        Console.Error.WriteLine(error);
        Console.ReadKey(true);
    }
}
