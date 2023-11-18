using GDExtensionSharp.Variants;

namespace GDExtensionSharp.Core;

public struct PropertyInfo
{
    private Variant.Type _type;
    private string _name;
    private string _class_name;
    private uint32_t _hint;
    private string _hint_string;
    private uint32_t _usage;
    
    public PropertyInfo()
    {
        _type = Variant.Type.NIL;
        _name = null;
        _class_name = null;
        _hint = 0;
        _hint_string = null;
        _usage = 7;
    }
}
