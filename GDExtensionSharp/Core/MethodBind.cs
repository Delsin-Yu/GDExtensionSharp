using GDExtensionSharp.Bindings.Header;
using GDExtensionSharp.Variants;

namespace GDExtensionSharp.Core;

internal abstract class MethodBind
{
    public string Name { get; set; }

    protected bool IsStatic {private get; set;} = false;
    protected bool IsConst {private get; set;} = false;
    protected bool HasReturn {private get; set;} = false;
    protected bool VarArg {private get; set;} = false;
    protected int ArgumentCount { private get; set; } = 0;
    
    private string _instance_class;
    private GDExtensionClassMethodFlags _hint_flags = GDExtensionClassMethodFlags.GDEXTENSION_METHOD_FLAGS_DEFAULT;

    private string[] _argument_names;
    private GDExtensionVariantType[] _argument_types = Array.Empty<GDExtensionVariantType>();
    private Variant[] _default_arguments = Array.Empty<Variant>();
    
    protected abstract GDExtensionVariantType gen_argument_type(int p_arg);
    protected abstract PropertyInfo gen_argument_type_info(int p_arg);

    protected void generate_argument_types(int p_count)
    {
        ArgumentCount = p_count;

        Array.Resize(ref _argument_types, p_count + 1);

        for (int i = -1; i < p_count; i++)
        {
            _argument_types[i + 1] = gen_argument_type(i);
        }
    }

    public int get_default_argument_count() => _default_arguments.Length;
    public IReadOnlyList<Variant> get_default_arguments() => _default_arguments;
    
    // public Variant has_default_argument(int p_arg)
    // {
    //     int num_default_args = (_default_arguments.Length);
    //     int idx = p_arg - (ArgumentCount - num_default_args);
    //
    //     if (idx < 0 || idx >= num_default_args) {
    //         return false;
    //     } else {
    //         return true;
    //     }
    // }
}
