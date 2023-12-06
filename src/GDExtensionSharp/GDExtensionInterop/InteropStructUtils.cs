using GDExtensionSharp;

namespace Godot.NativeInterop;

internal static class InteropStructUtils
{
    internal static godot_variant_call_error Bridge(this in GDExtensionCallError value)
    {
        return new()
        {
            Argument = value.argument,
            Error = (godot_variant_call_error_error)value.error,
            Expected = (Variant.Type)value.expected
        };
    }

    internal static godot_variant CreateVariant(Rid from) => VariantUtils.CreateFromRid(from);
    internal static godot_variant CreateVariant(bool from) => VariantUtils.CreateFromBool(from);
    internal static godot_variant CreateVariant(long from) => VariantUtils.CreateFromInt(from);
    internal static godot_variant CreateVariant(ulong from) => VariantUtils.CreateFromInt(from);
    internal static godot_variant CreateVariant(double from) => VariantUtils.CreateFromFloat(from);
    internal static godot_variant CreateVariant(Vector2 from) => VariantUtils.CreateFromVector2(from);
    internal static godot_variant CreateVariant(Vector2I from) => VariantUtils.CreateFromVector2I(from);
    internal static godot_variant CreateVariant(Vector3 from) => VariantUtils.CreateFromVector3(from);
    internal static godot_variant CreateVariant(Vector3I from) => VariantUtils.CreateFromVector3I(from);
    internal static godot_variant CreateVariant(Vector4 from) => VariantUtils.CreateFromVector4(from);
    internal static godot_variant CreateVariant(Vector4I from) => VariantUtils.CreateFromVector4I(from);
    internal static godot_variant CreateVariant(Rect2 from) => VariantUtils.CreateFromRect2(from);
    internal static godot_variant CreateVariant(Rect2I from) => VariantUtils.CreateFromRect2I(from);
    internal static godot_variant CreateVariant(Quaternion from) => VariantUtils.CreateFromQuaternion(from);
    internal static godot_variant CreateVariant(Color from) => VariantUtils.CreateFromColor(from);
    internal static godot_variant CreateVariant(Plane from) => VariantUtils.CreateFromPlane(from);
    internal static godot_variant CreateVariant(Transform2D from) => VariantUtils.CreateFromTransform2D(from);
    internal static godot_variant CreateVariant(Basis from) => VariantUtils.CreateFromBasis(from);
    internal static godot_variant CreateVariant(Transform3D from) => VariantUtils.CreateFromTransform3D(from);
    internal static godot_variant CreateVariant(Projection from) => VariantUtils.CreateFromProjection(from);
    internal static godot_variant CreateVariant(Aabb from) => VariantUtils.CreateFromAabb(from);
    internal static godot_variant CreateVariant(Callable from) => VariantUtils.CreateFromCallable(from);
    internal static godot_variant CreateVariant(Signal from) => VariantUtils.CreateFromSignal(from);
    internal static godot_variant CreateVariant(string from) => VariantUtils.CreateFromString(from);
    internal static godot_variant CreateVariant(Span<byte> from) => VariantUtils.CreateFromPackedByteArray(from);
    internal static godot_variant CreateVariant(Span<int> from) => VariantUtils.CreateFromPackedInt32Array(from);
    internal static godot_variant CreateVariant(Span<long> from) => VariantUtils.CreateFromPackedInt64Array(from);
    internal static godot_variant CreateVariant(Span<float> from) => VariantUtils.CreateFromPackedFloat32Array(from);
    internal static godot_variant CreateVariant(Span<double> from) => VariantUtils.CreateFromPackedFloat64Array(from);
    internal static godot_variant CreateVariant(Span<string> from) => VariantUtils.CreateFromPackedStringArray(from);
    internal static godot_variant CreateVariant(Span<Vector2> from) => VariantUtils.CreateFromPackedVector2Array(from);
    internal static godot_variant CreateVariant(Span<Vector3> from) => VariantUtils.CreateFromPackedVector3Array(from);
    internal static godot_variant CreateVariant(Span<Color> from) => VariantUtils.CreateFromPackedColorArray(from);
    internal static godot_variant CreateVariant(Collections.Array from) => VariantUtils.CreateFromArray(from);
    internal static godot_variant CreateVariant(Collections.Dictionary from) => VariantUtils.CreateFromDictionary(from);
    internal static godot_variant CreateVariant(StringName from) => VariantUtils.CreateFromStringName(from);
    internal static godot_variant CreateVariant(NodePath from) => VariantUtils.CreateFromNodePath(from);
    internal static godot_variant CreateVariant(GodotObject from) => VariantUtils.CreateFromGodotObject(from);
}
