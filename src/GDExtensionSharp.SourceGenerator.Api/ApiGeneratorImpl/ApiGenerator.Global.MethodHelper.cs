﻿using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string MethodHelper = "MethodHelper";

    private const string StaticCast = "StaticCast";
    private const string CallBuiltinConstructor = "CallBuiltinConstructor";
    private const string CallBuiltinMethodPointerReturn = "CallBuiltinMethodPointerReturn";
    private const string CallBuiltinMethodPointerNoReturn = "CallBuiltinMethodPointerNoReturn";
    private const string CallBuiltinOperatorPointer = "CallBuiltinOperatorPointer";
    private const string CallBuiltinPointerGetter = "CallBuiltinPointerGetter";

    private const string ExpandableArgs = "<EXPANDABLE_ARGS>";
    private const string ExpandableArgsLength = "<EXPANDABLE_ARGS_LENGTH>";
    private const string ExpandableArgsInit = "<ARG_INIT>";

    private const string ExpandableArgListTemplate =
        $"""
         {Indents}{Indents}var argList = stackalloc void*[{ExpandableArgsLength}];
         {ExpandableArgsInit}
         """;

    private const string CallBuiltinCtorExpandableTemplate =
        $$"""
          {{Indents}}internal static void {{CallBuiltinConstructor}}({{GDExtensionPtrConstructor}} constructor, {{GDExtensionTypePtr}} @base{{ExpandableArgs}})
          {{Indents}}{
          {{ExpandableArgListTemplate}}

          {{Indents}}{{Indents}}constructor(@base, argList);
          {{Indents}}}
          """;

    private const string CallBuiltinMethodPtrReturnExpandableTemplate =
        $$"""
          {{Indents}}internal static void {{CallBuiltinMethodPointerReturn}}<T>(T* returnValue, {{GDExtensionPtrBuiltInMethod}} method, {{GDExtensionTypePtr}} @base{{ExpandableArgs}}) where T : unmanaged
          {{Indents}}{
          {{ExpandableArgListTemplate}}

          {{Indents}}{{Indents}}method(@base, argList, returnValue, {{ExpandableArgsLength}});
          {{Indents}}}
          """;

    private const string CallBuiltinMethodPtrNoReturnExpandableTemplate =
        $$"""
          {{Indents}}internal static void {{CallBuiltinMethodPointerNoReturn}}({{GDExtensionPtrBuiltInMethod}} method, {{GDExtensionTypePtr}} @base{{ExpandableArgs}})
          {{Indents}}{
          {{ExpandableArgListTemplate}}

          {{Indents}}{{Indents}}method(@base, argList, null, {{ExpandableArgsLength}});
          {{Indents}}}
          """;

    private static readonly StringBuilder _expandArgListStringBuilder = new();
    private static readonly StringBuilder _expandArgListStringBuilder2 = new();
    private static readonly StringBuilder _expandArgListStringBuilder3 = new();

    private static string ExpandArgList(string template)
    {
        for (var i = 0; i < 16; i++)
        {
            // build args
            for (int argCount = 0; argCount < i + 1; argCount++)
            {
                const string argName = "__arg";

                _expandArgListStringBuilder2
                    .Append($", void* {argName}")
                    .Append(argCount);

                _expandArgListStringBuilder3
                    .Append($"{Indents}{Indents}argList[")
                    .Append(argCount + 1)
                    .Append($"] = {argName}")
                    .Append(argCount)
                    .AppendLine(";");
            }

            var args = _expandArgListStringBuilder2.ToStringAndClear();

            _expandArgListStringBuilder
                .AppendLine(template
                    .Replace(ExpandableArgs, args)
                    .Replace(ExpandableArgsLength, i.ToString(CultureInfo.InvariantCulture))
                    .Replace(ExpandableArgsInit, _expandArgListStringBuilder3.ToStringAndClear()))
                .AppendLine();
        }

        return _expandArgListStringBuilder.ToStringAndClear();
    }

    private static (string sourceName, string sourceContent) GenerateMethodHelper()
    {
        var content =
            $$"""
              {{NamespaceHeader}}

              internal static unsafe class {{MethodHelper}}
              {

                  internal static int8_t {{StaticCast}}(bool self) => Convert.ToSByte(self);
                  internal static bool {{StaticCast}}(int8_t self) => Convert.ToBoolean(self);

                  internal static T {{StaticCast}}<T>(T self) => self;

                  internal static void {{CallBuiltinConstructor}}({{GDExtensionPtrConstructor}} constructor, {{GDExtensionTypePtr}} @base)
                  {
                      constructor(@base, null);
                  }

                  internal static void {{CallBuiltinMethodPointerReturn}}<T>(T* returnValue, {{GDExtensionPtrBuiltInMethod}} method, {{GDExtensionTypePtr}} @base) where T : unmanaged
                  {
                      method(@base, null, returnValue, 0);
                  }

                  internal static void {{CallBuiltinMethodPointerNoReturn}}({{GDExtensionPtrBuiltInMethod}} method, {{GDExtensionTypePtr}} @base)
                  {
                      method(@base, null, null, 0);
                  }

                  internal static T {{CallBuiltinOperatorPointer}}<T>({{GDExtensionPtrOperatorEvaluator}} op, {{GDExtensionConstTypePtr}} left, {{GDExtensionConstTypePtr}} right) where T : unmanaged
                  {
                      T ret;
                      op(left, right, &ret);
                      return ret;
                  }

                  internal static T {{CallBuiltinPointerGetter}}<T>({{GDExtensionPtrGetter}} getter, {{GDExtensionConstTypePtr}} @base) where T : unmanaged
                  {
                      T ret;
                      getter(@base, &ret);
                      return ret;
                  }


              {{ExpandArgList(CallBuiltinCtorExpandableTemplate)}}

              {{ExpandArgList(CallBuiltinMethodPtrReturnExpandableTemplate)}}

              {{ExpandArgList(CallBuiltinMethodPtrNoReturnExpandableTemplate)}}

              }
              """;

        return ("MethodHelper", content);
    }
}