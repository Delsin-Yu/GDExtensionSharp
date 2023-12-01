using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static void GenerateClassSignals(
        StringBuilder stringBuilder,
        IReadOnlyList<Signal> engineClassSignals,
        IReadOnlyCollection<string> godotTypes,
        IReadOnlyCollection<string> refCountedGodotTypes)
    {
        if (engineClassSignals == null) return;


        foreach (Signal engineClassSignal in engineClassSignals)
        {
            string signalName = engineClassSignal.Name.SnakeCaseToPascalCase();

            if (engineClassSignal.Arguments == null)
            {
                stringBuilder.AppendIndentLine($$"""
                                                 // Generated Signal: {{signalName}}
                                                 public event Action {{signalName}}
                                                 {
                                                     add => Connect(SignalName.{{signalName}}, Callable.From(value));
                                                     remove => Disconnect(SignalName.{{signalName}}, Callable.From(value));
                                                 }
                                                 """);
            }
            else
            {
                Argument[] arguments = engineClassSignal.Arguments;
                string argumentsLength = arguments.Length.ToString(CultureInfo.InvariantCulture);

                stringBuilder
                    .AppendIndent($$"""
                                    // Generated Signal with Arguments: {{signalName}}
                                    public unsafe event {{signalName}}EventHandler {{signalName}}
                                    {
                                        add => Connect(SignalName.{{signalName}}, Callable.CreateWithUnsafeTrampoline(value, &{{signalName}}Trampoline));
                                        remove => Disconnect(SignalName.{{signalName}}, Callable.CreateWithUnsafeTrampoline(value, &{{signalName}}Trampoline));
                                    }
                                    public delegate void {{signalName}}EventHandler(
                                    """);

                BuildMethodHeaderArguments(stringBuilder, arguments, godotTypes, refCountedGodotTypes, false);

                stringBuilder.AppendLine(");");

                stringBuilder
                    .AppendIndentLine($$"""
                                        private static void {{signalName}}Trampoline(object delegateObj, NativeVariantPtrArgs args, out godot_variant ret)
                                        {
                                            Callable.ThrowIfArgCountMismatch(args, {{argumentsLength}});
                                            (({{signalName}}EventHandler)delegateObj)
                                                .Invoke(
                                        """);

                using (stringBuilder.AddIndent(3))
                {
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        stringBuilder
                            .AppendIndent($"VariantUtils.ConvertTo<{GetInteropHandler(CorrectType(arguments[i].Type), godotTypes, refCountedGodotTypes).CsharpType}>(args[{i}])");

                        if (i != arguments.Length - 1)
                        {
                            stringBuilder.Append(',');
                        }

                        stringBuilder.AppendLine();
                    }
                }

                stringBuilder
                    .AppendIndentLine("""
                                              );
                                          ret = default;
                                      }
                                      """);
            }

            stringBuilder.AppendIndentLine();
        }
    }
}
