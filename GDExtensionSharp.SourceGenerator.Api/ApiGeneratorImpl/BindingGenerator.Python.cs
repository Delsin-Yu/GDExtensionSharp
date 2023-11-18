// using System;
// using System.Linq;
// using System.Collections.Generic;
// using System.Text.Json;
// using GDExtensionSharp.SourceGenerator.Api;
//
// public static class Program
// {
//     //!/usr/bin/env python
//     public static object generate_mod_version(int argcount, bool @const = false, bool returns = false)
//     {
//         var s = @"
// #define MODBIND$VER($RETTYPE m_name$ARG) " +
//                 "\\" +
//                 @"
// virtual $RETVAL _##m_name($FUNCARGS) $CONST override; " +
//                 "\\" +
//                 @"
// ";
//         var sproto = argcount.ToString();
//         if (returns)
//         {
//             sproto += "R";
//             s = s.Replace("$RETTYPE", "m_ret, ");
//             s = s.Replace("$RETVAL", "m_ret");
//         }
//         else
//         {
//             s = s.Replace("$RETTYPE", "");
//             s = s.Replace("$RETVAL", "void");
//         }
//
//         if (@const)
//         {
//             sproto += "C";
//             s = s.Replace("$CONST", "const");
//         }
//         else
//         {
//             s = s.Replace("$CONST", "");
//         }
//
//         s = s.Replace("$VER", sproto);
//         var argtext = "";
//         var funcargs = "";
//         foreach (var i in Enumerable.Range(0, argcount))
//         {
//             if (i > 0)
//             {
//                 funcargs += ", ";
//             }
//
//             argtext += ", m_type" + (i + 1).ToString();
//             funcargs += "m_type" + (i + 1).ToString() + " arg" + (i + 1).ToString();
//         }
//
//         if (argcount > 0)
//         {
//             s = s.Replace("$ARG", argtext);
//             s = s.Replace("$FUNCARGS", funcargs);
//         }
//         else
//         {
//             s = s.Replace("$ARG", "");
//             s = s.Replace("$FUNCARGS", funcargs);
//         }
//
//         return s;
//     }
//
//     public static void generate_wrappers(string target)
//     {
//         var max_versions = 12;
//         var txt = @"
// #ifndef GDEXTENSION_WRAPPERS_GEN_H
// #define GDEXTENSION_WRAPPERS_GEN_H
//
// ";
//         foreach (var i in Enumerable.Range(0, max_versions + 1))
//         {
//             txt += "\n/* Module Wrapper " + i.ToString() + " Arguments */\n";
//             txt += generate_mod_version(i, false, false);
//             txt += generate_mod_version(i, false, true);
//             txt += generate_mod_version(i, true, false);
//             txt += generate_mod_version(i, true, true);
//         }
//
//         txt += "\n#endif\n";
// #warning File.WriteAllText(target, txt);
//     }
//
//     public static void get_file_list(string api_filepath, string output_dir, bool headers = false, bool sources = false)
//     {
//         string source_filename;
//         string header_filename;
//         var files = new List<object>();
//         var api = JsonSerializer.Deserialize<GDExtensionApi>(api_filepath, Converter.Settings);
//
//         var core_gen_folder = Path.Combine(output_dir, "gen", "include", "godot_cpp", "core");
//         var include_gen_folder = Path.Combine(output_dir, "gen", "include", "godot_cpp");
//         var source_gen_folder = Path.Combine(output_dir, "gen", "src");
//
//         foreach (var builtin_class in api.BuiltinClasses)
//         {
//             if (is_pod_type(builtin_class.Name))
//             {
//                 continue;
//             }
//
//             if (is_included_type(builtin_class.Name))
//             {
//                 continue;
//             }
//
//             header_filename = Path.Combine(include_gen_folder,  "variant" , (camel_to_snake(builtin_class.Name) + ".hpp"));
//             source_filename = Path.Combine(source_gen_folder,  "variant" , (camel_to_snake(builtin_class.Name) + ".cpp"));
//             if (headers)
//             {
//                 files.Add(header_filename.as_posix());
//             }
//
//             if (sources)
//             {
//                 files.Add(source_filename.as_posix());
//             }
//         }
//
//         foreach (var engine_class in api.Classes)
//         {
//             // Generate code for the ClassDB singleton under a different name.
//             if (engine_class["name"] == "ClassDB")
//             {
//                 engine_class["name"] = "ClassDBSingleton";
//                 engine_class["alias_for"] = "ClassDB";
//             }
//
//             header_filename = include_gen_folder / "classes" / (camel_to_snake(engine_class["name"]) + ".hpp");
//             source_filename = source_gen_folder / "classes" / (camel_to_snake(engine_class["name"]) + ".cpp");
//             if (headers)
//             {
//                 files.append(header_filename.as_posix());
//             }
//
//             if (sources)
//             {
//                 files.append(source_filename.as_posix());
//             }
//         }
//
//         foreach (var native_struct in api["native_structures"])
//         {
//             var struct_name = native_struct["name"];
//             var snake_struct_name = camel_to_snake(struct_name);
//             header_filename = include_gen_folder / "classes" / (snake_struct_name + ".hpp");
//             if (headers)
//             {
//                 files.append(header_filename.as_posix());
//             }
//         }
//
//         if (headers)
//         {
//             foreach (var path in new List<object>
//                                  {
//                                      include_gen_folder / "variant" / "builtin_types.hpp",
//                                      include_gen_folder / "variant" / "builtin_binds.hpp",
//                                      include_gen_folder / "variant" / "utility_functions.hpp",
//                                      include_gen_folder / "variant" / "variant_size.hpp",
//                                      include_gen_folder / "classes" / "global_constants.hpp",
//                                      include_gen_folder / "classes" / "global_constants_binds.hpp",
//                                      include_gen_folder / "core" / "version.hpp"
//                                  })
//             {
//                 files.append(path.as_posix().ToString());
//             }
//         }
//
//         if (sources)
//         {
//             var utility_functions_source_path = source_gen_folder / "variant" / "utility_functions.cpp";
//             files.append(utility_functions_source_path.as_posix().ToString());
//         }
//
//         return files;
//     }
//
//     public static object print_file_list(object api_filepath, object output_dir, object headers = false, object sources = false)
//     {
//         var end = ";";
//         foreach (var f in get_file_list(api_filepath, output_dir, headers, sources))
//         {
//             Console.WriteLine(f, end : end);
//         }
//     }
//     
//     public static string as_posix(this string self) => self.Replace("\\", "/");
//
//     public static object scons_emit_files(object target, object source, object env)
//     {
//         var files = (from f in get_file_list(source[0].ToString(), target[0].abspath, true, true)
//                      select env.File(f)).ToList();
//         env.Clean(target, files);
//         env["godot_cpp_gen_dir"] = target[0].abspath;
//         return (files, source);
//     }
//
//     public static object scons_generate_bindings(object target, object source, object env)
//     {
//         generate_bindings(source[0].ToString(), env["generate_template_get_node"], env["arch"].Contains("32") ? "32" : "64", env["precision"], env["godot_cpp_gen_dir"]);
//         return null;
//     }
//
//     public static object generate_bindings
//         (
//             object api_filepath,
//             object use_template_get_node,
//             object bits = "64",
//             object precision = "single",
//             object output_dir = "."
//         )
//     {
//         object api = null;
//         var target_dir = Path(output_dir) / "gen";
//         using (var api_file = open(api_filepath, encoding : "utf-8"))
//         {
//             api = json.load(api_file);
//         }
//
//         shutil.rmtree(target_dir, ignore_errors : true);
//         target_dir.mkdir(parents : true);
//         var real_t = precision == "double" ? "double" : "float";
//         Console.WriteLine("Built-in type config: " + real_t + "_" + bits);
//         generate_global_constants(api, target_dir);
//         generate_version_header(api, target_dir);
//         generate_global_constant_binds(api, target_dir);
//         generate_builtin_bindings(api, target_dir, real_t + "_" + bits);
//         generate_engine_classes_bindings(api, target_dir, use_template_get_node);
//         generate_utility_functions(api, target_dir);
//     }
//
//     public static List<string> builtin_classes = new();
//
//     public static Dictionary<string, string> engine_classes = new();
//
//     public static List<string> native_structures = new();
//
//     public static List<string> singletons = new();
//
//     public static object generate_builtin_bindings(object api, object output_dir, object build_config)
//     {
//         var core_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "core";
//         var include_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "variant";
//         var source_gen_folder = Path(output_dir) / "src" / "variant";
//         core_gen_folder.mkdir(parents : true, exist_ok : true);
//         include_gen_folder.mkdir(parents : true, exist_ok : true);
//         source_gen_folder.mkdir(parents : true, exist_ok : true);
//         generate_wrappers(core_gen_folder / "ext_wrappers.gen.inc");
//         // Store types beforehand.
//         foreach (var builtin_api in api["builtin_classes"])
//         {
//             if (is_pod_type(builtin_api["name"]))
//             {
//                 continue;
//             }
//
//             builtin_classes.append(builtin_api["name"]);
//         }
//
//         var builtin_sizes = new Dictionary<object, object>
//                             { };
//         foreach (var size_list in api["builtin_class_sizes"])
//         {
//             if (size_list["build_configuration"] == build_config)
//             {
//                 foreach (var size in size_list["sizes"])
//                 {
//                     builtin_sizes[size["name"]] = size["size"];
//                 }
//
//                 break;
//             }
//         }
//
//         // Create a file for Variant size, since that class isn't generated.
//         var variant_size_filename = include_gen_folder / "variant_size.hpp";
//         using (var variant_size_file = variant_size_filename.open("+w", encoding : "utf-8"))
//         {
//             variant_size_source = new List<object>();
//             add_header("variant_size.hpp", variant_size_source);
//             header_guard = "GODOT_CPP_VARIANT_SIZE_HPP";
//             variant_size_source.append($"#ifndef {header_guard}");
//             variant_size_source.append($"#define {header_guard}");
//             variant_size_source.append($"#define GODOT_CPP_VARIANT_SIZE {builtin_sizes[\"Variant\"]}");
//             variant_size_source.append($"#endif // ! {header_guard}");
//             variant_size_file.write("\n".join(variant_size_source));
//         }
//
//         foreach (var builtin_api in api["builtin_classes"])
//         {
//             if (is_pod_type(builtin_api["name"]))
//             {
//                 continue;
//             }
//
//             if (is_included_type(builtin_api["name"]))
//             {
//                 continue;
//             }
//
//             var size = builtin_sizes[builtin_api["name"]];
//             var header_filename = include_gen_folder / (camel_to_snake(builtin_api["name"]) + ".hpp");
//             var source_filename = source_gen_folder / (camel_to_snake(builtin_api["name"]) + ".cpp");
//             // Check used classes for header include
//             var used_classes = new HashSet<object>();
//             var fully_used_classes = new HashSet<object>();
//             var class_name = builtin_api["name"];
//             if (builtin_api.Contains("constructors"))
//             {
//                 foreach (var constructor in builtin_api["constructors"])
//                 {
//                     if (constructor.Contains("arguments"))
//                     {
//                         foreach (var argument in constructor["arguments"])
//                         {
//                             if (is_included(argument["type"], class_name))
//                             {
//                                 if (argument.Contains("default_value") && argument["type"] != "Variant")
//                                 {
//                                     fully_used_classes.add(argument["type"]);
//                                 }
//                                 else
//                                 {
//                                     used_classes.add(argument["type"]);
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//
//             if (builtin_api.Contains("methods"))
//             {
//                 foreach (var method in builtin_api["methods"])
//                 {
//                     if (method.Contains("arguments"))
//                     {
//                         foreach (var argument in method["arguments"])
//                         {
//                             if (is_included(argument["type"], class_name))
//                             {
//                                 if (argument.Contains("default_value") && argument["type"] != "Variant")
//                                 {
//                                     fully_used_classes.add(argument["type"]);
//                                 }
//                                 else
//                                 {
//                                     used_classes.add(argument["type"]);
//                                 }
//                             }
//                         }
//                     }
//
//                     if (method.Contains("return_type"))
//                     {
//                         if (is_included(method["return_type"], class_name))
//                         {
//                             used_classes.add(method["return_type"]);
//                         }
//                     }
//                 }
//             }
//
//             if (builtin_api.Contains("members"))
//             {
//                 foreach (var member in builtin_api["members"])
//                 {
//                     if (is_included(member["type"], class_name))
//                     {
//                         used_classes.add(member["type"]);
//                     }
//                 }
//             }
//
//             if (builtin_api.Contains("indexing_return_type"))
//             {
//                 if (is_included(builtin_api["indexing_return_type"], class_name))
//                 {
//                     used_classes.add(builtin_api["indexing_return_type"]);
//                 }
//             }
//
//             if (builtin_api.Contains("operators"))
//             {
//                 foreach (var @operator in builtin_api["operators"])
//                 {
//                     if (@operator.Contains("right_type"))
//                     {
//                         if (is_included(@operator["right_type"], class_name))
//                         {
//                             used_classes.add(@operator["right_type"]);
//                         }
//                     }
//                 }
//             }
//
//             foreach (var type_name in fully_used_classes)
//             {
//                 if (used_classes.Contains(type_name))
//                 {
//                     used_classes.remove(type_name);
//                 }
//             }
//
//             used_classes = used_classes.ToList();
//             used_classes.sort();
//             fully_used_classes = fully_used_classes.ToList();
//             fully_used_classes.sort();
//             using (var header_file = header_filename.open("w+", encoding : "utf-8"))
//             {
//                 header_file.write(generate_builtin_class_header(builtin_api, size, used_classes, fully_used_classes));
//             }
//
//             using (var source_file = source_filename.open("w+", encoding : "utf-8"))
//             {
//                 source_file.write(generate_builtin_class_source(builtin_api, size, used_classes, fully_used_classes));
//             }
//         }
//
//         // Create a header with all builtin types for convenience.
//         var builtin_header_filename = include_gen_folder / "builtin_types.hpp";
//         using (var builtin_header_file = builtin_header_filename.open("w+", encoding : "utf-8"))
//         {
//             builtin_header = new List<object>();
//             add_header("builtin_types.hpp", builtin_header);
//             builtin_header.append("#ifndef GODOT_CPP_BUILTIN_TYPES_HPP");
//             builtin_header.append("#define GODOT_CPP_BUILTIN_TYPES_HPP");
//             builtin_header.append("");
//             foreach (var builtin in builtin_classes)
//             {
//                 builtin_header.append($"#include <godot_cpp/variant/{camel_to_snake(builtin)}.hpp>");
//             }
//
//             builtin_header.append("");
//             builtin_header.append("#endif // ! GODOT_CPP_BUILTIN_TYPES_HPP");
//             builtin_header_file.write("\n".join(builtin_header));
//         }
//
//         // Create a header with bindings for builtin types.
//         var builtin_binds_filename = include_gen_folder / "builtin_binds.hpp";
//         using (var builtin_binds_file = builtin_binds_filename.open("w+", encoding : "utf-8"))
//         {
//             builtin_binds = new List<object>();
//             add_header("builtin_binds.hpp", builtin_binds);
//             builtin_binds.append("#ifndef GODOT_CPP_BUILTIN_BINDS_HPP");
//             builtin_binds.append("#define GODOT_CPP_BUILTIN_BINDS_HPP");
//             builtin_binds.append("");
//             builtin_binds.append("#include <godot_cpp/variant/builtin_types.hpp>");
//             builtin_binds.append("");
//             foreach (var builtin_api in api["builtin_classes"])
//             {
//                 if (is_included_type(builtin_api["name"]))
//                 {
//                     if (builtin_api.Contains("enums"))
//                     {
//                         foreach (var enum_api in builtin_api["enums"])
//                         {
//                             builtin_binds.append($"VARIANT_ENUM_CAST({builtin_api['name']}::{enum_api['name']});");
//                         }
//                     }
//                 }
//             }
//
//             builtin_binds.append("");
//             builtin_binds.append("#endif // ! GODOT_CPP_BUILTIN_BINDS_HPP");
//             builtin_binds_file.write("\n".join(builtin_binds));
//         }
//     }
//
//     public static object generate_builtin_class_header(object builtin_api, object size, object used_classes, object fully_used_classes)
//     {
//         object method_signature;
//         var result = new List<object>();
//         var class_name = builtin_api["name"];
//         var snake_class_name = camel_to_snake(class_name).upper();
//         var header_guard = $"GODOT_CPP_{snake_class_name}_HPP";
//         add_header($"{snake_class_name.lower()}.hpp", result);
//         result.append($"#ifndef {header_guard}");
//         result.append($"#define {header_guard}");
//         result.append("");
//         result.append("#include <godot_cpp/core/defs.hpp>");
//         result.append("");
//         // Special cases.
//         if (class_name == "String")
//         {
//             result.append("#include <godot_cpp/variant/char_string.hpp>");
//             result.append("#include <godot_cpp/variant/char_utils.hpp>");
//         }
//
//         if (class_name == "PackedStringArray")
//         {
//             result.append("#include <godot_cpp/variant/string.hpp>");
//         }
//
//         if (class_name == "PackedColorArray")
//         {
//             result.append("#include <godot_cpp/variant/color.hpp>");
//         }
//
//         if (class_name == "PackedVector2Array")
//         {
//             result.append("#include <godot_cpp/variant/vector2.hpp>");
//         }
//
//         if (class_name == "PackedVector3Array")
//         {
//             result.append("#include <godot_cpp/variant/vector3.hpp>");
//         }
//
//         if (class_name == "Array")
//         {
//             result.append("#include <godot_cpp/variant/array_helpers.hpp>");
//         }
//
//         foreach (var include in fully_used_classes)
//         {
//             if (include == "TypedArray")
//             {
//                 result.append("#include <godot_cpp/variant/typed_array.hpp>");
//             }
//             else
//             {
//                 result.append($"#include <godot_cpp/{get_include_path(include)}>");
//             }
//         }
//
//         if (fully_used_classes.Count > 0)
//         {
//             result.append("");
//         }
//
//         result.append("#include <gdextension_interface.h>");
//         result.append("");
//         result.append("namespace godot {");
//         result.append("");
//         foreach (var type_name in used_classes)
//         {
//             if (is_struct_type(type_name))
//             {
//                 result.append($"struct {type_name};");
//             }
//             else
//             {
//                 result.append($"class {type_name};");
//             }
//         }
//
//         if (used_classes.Count > 0)
//         {
//             result.append("");
//         }
//
//         result.append($"class {class_name} {{");
//         result.append($"\tstatic constexpr size_t {snake_class_name}_SIZE = {size};");
//         result.append($"\tuint8_t opaque[{snake_class_name}_SIZE] = {{}};");
//         result.append("");
//         result.append("\tfriend class Variant;");
//         if (class_name == "String")
//         {
//             result.append("\tfriend class StringName;");
//         }
//
//         result.append("");
//         result.append("\tstatic struct _MethodBindings {");
//         if (builtin_api.Contains("constructors"))
//         {
//             foreach (var constructor in builtin_api["constructors"])
//             {
//                 result.append($"\t\tGDExtensionPtrConstructor constructor_{constructor[\"index\"]};");
//             }
//         }
//
//         if (builtin_api["has_destructor"])
//         {
//             result.append("\t\tGDExtensionPtrDestructor destructor;");
//         }
//
//         if (builtin_api.Contains("methods"))
//         {
//             foreach (var method in builtin_api["methods"])
//             {
//                 result.append($"\t\tGDExtensionPtrBuiltInMethod method_{method[\"name\"]};");
//             }
//         }
//
//         if (builtin_api.Contains("members"))
//         {
//             foreach (var member in builtin_api["members"])
//             {
//                 result.append($"\t\tGDExtensionPtrSetter member_{member[\"name\"]}_setter;");
//                 result.append($"\t\tGDExtensionPtrGetter member_{member[\"name\"]}_getter;");
//             }
//         }
//
//         if (builtin_api.Contains("indexing_return_type"))
//         {
//             result.append("\t\tGDExtensionPtrIndexedSetter indexed_setter;");
//             result.append("\t\tGDExtensionPtrIndexedGetter indexed_getter;");
//         }
//
//         if (builtin_api.Contains("is_keyed") && builtin_api["is_keyed"])
//         {
//             result.append("\t\tGDExtensionPtrKeyedSetter keyed_setter;");
//             result.append("\t\tGDExtensionPtrKeyedGetter keyed_getter;");
//             result.append("\t\tGDExtensionPtrKeyedChecker keyed_checker;");
//         }
//
//         if (builtin_api.Contains("operators"))
//         {
//             foreach (var @operator in builtin_api["operators"])
//             {
//                 if (@operator.Contains("right_type"))
//                 {
//                     result.append($"\t\tGDExtensionPtrOperatorEvaluator operator_{get_operator_id_name(operator[\"name\"])}_{operator[\"right_type\"]};");
//                 }
//                 else
//                 {
//                     result.append($"\t\tGDExtensionPtrOperatorEvaluator operator_{get_operator_id_name(operator[\"name\"])};");
//                 }
//             }
//         }
//
//         result.append("\t} _method_bindings;");
//         result.append("");
//         result.append("\tstatic void init_bindings();");
//         result.append("\tstatic void _init_bindings_constructors_destructor();");
//         result.append("");
//         result.append("public:");
//         result.append($"\t_FORCE_INLINE_ GDExtensionTypePtr _native_ptr() const {{ return const_cast<uint8_t (*)[{snake_class_name}_SIZE]>(&opaque); }}");
//         var copy_constructor_index = -1;
//         if (builtin_api.Contains("constructors"))
//         {
//             foreach (var constructor in builtin_api["constructors"])
//             {
//                 method_signature = $"\t{class_name}(";
//                 if (constructor.Contains("arguments"))
//                 {
//                     method_signature += make_function_parameters(constructor["arguments"], include_default : true, for_builtin : true);
//                     if (constructor["arguments"].Count == 1 && constructor["arguments"][0]["type"] == class_name)
//                     {
//                         copy_constructor_index = constructor["index"];
//                     }
//                 }
//
//                 method_signature += ");";
//                 result.append(method_signature);
//             }
//         }
//
//         // Move constructor.
//         result.append($"\t{class_name}({class_name} &&other);");
//         // Special cases.
//         if (class_name == "String" || class_name == "StringName" || class_name == "NodePath")
//         {
//             result.append($"\t{class_name}(const char *from);");
//             result.append($"\t{class_name}(const wchar_t *from);");
//             result.append($"\t{class_name}(const char16_t *from);");
//             result.append($"\t{class_name}(const char32_t *from);");
//         }
//
//         if (builtin_api.Contains("constants"))
//         {
//             var axis_constants_count = 0;
//             foreach (var constant in builtin_api["constants"])
//             {
//                 // Special case: Vector3.Axis is the only enum in the bindings.
//                 // It's technically not supported by Variant but works for direct access.
//                 if (class_name == "Vector3" && constant["name"].StartsWith("AXIS"))
//                 {
//                     if (axis_constants_count == 0)
//                     {
//                         result.append("\tenum Axis {");
//                     }
//
//                     result.append($"\t\t{constant[\"name\"]} = {constant[\"value\"]},");
//                     axis_constants_count += 1;
//                     if (axis_constants_count == 3)
//                     {
//                         result.append("\t};");
//                     }
//                 }
//                 else
//                 {
//                     result.append($"\tstatic const {correct_type(constant[\"type\"])} {constant[\"name\"]};");
//                 }
//             }
//         }
//
//         if (builtin_api["has_destructor"])
//         {
//             result.append($"\t~{class_name}();");
//         }
//
//         var method_list = new List<object>();
//         if (builtin_api.Contains("methods"))
//         {
//             foreach (var method in builtin_api["methods"])
//             {
//                 method_list.append(method["name"]);
//                 var vararg = method["is_vararg"];
//                 if (vararg)
//                 {
//                     result.append("\ttemplate<class... Args>");
//                 }
//
//                 method_signature = "\t";
//                 if (method.Contains("is_static") && method["is_static"])
//                 {
//                     method_signature += "static ";
//                 }
//
//                 if (method.Contains("return_type"))
//                 {
//                     method_signature += $"{correct_type(method[\"return_type\"])} ";
//                 }
//                 else
//                 {
//                     method_signature += "void ";
//                 }
//
//                 method_signature += $"{method[\"name\"]}(";
//                 var method_arguments = new List<object>();
//                 if (method.Contains("arguments"))
//                 {
//                     method_arguments = method["arguments"];
//                 }
//
//                 method_signature += make_function_parameters(method_arguments, include_default : true, for_builtin : true, is_vararg : vararg);
//                 method_signature += ")";
//                 if (method["is_const"])
//                 {
//                     method_signature += " const";
//                 }
//
//                 method_signature += ";";
//                 result.append(method_signature);
//             }
//         }
//
//         // Special cases.
//         if (class_name == "String")
//         {
//             result.append("\tstatic String utf8(const char *from, int len = -1);");
//             result.append("\tvoid parse_utf8(const char *from, int len = -1);");
//             result.append("\tstatic String utf16(const char16_t *from, int len = -1);");
//             result.append("\tvoid parse_utf16(const char16_t *from, int len = -1);");
//             result.append("\tCharString utf8() const;");
//             result.append("\tCharString ascii() const;");
//             result.append("\tChar16String utf16() const;");
//             result.append("\tChar32String utf32() const;");
//             result.append("\tCharWideString wide_string() const;");
//             result.append("\tstatic String num_real(double p_num, bool p_trailing = true);");
//         }
//
//         if (builtin_api.Contains("members"))
//         {
//             foreach (var member in builtin_api["members"])
//             {
//                 if (!method_list.Contains($"get_{member[\"name\"]}"))
//                 {
//                     result.append($"\t{correct_type(member[\"type\"])} get_{member[\"name\"]}() const;");
//                 }
//
//                 if (!method_list.Contains($"set_{member[\"name\"]}"))
//                 {
//                     result.append($"\tvoid set_{member[\"name\"]}({type_for_parameter(member[\"type\"])}value);");
//                 }
//             }
//         }
//
//         if (builtin_api.Contains("operators"))
//         {
//             foreach (var @operator in builtin_api["operators"])
//             {
//                 if (!new List<object>
//                      {
//                          "in",
//                          "xor"
//                      }.Contains(@operator["name"]))
//                 {
//                     if (@operator.Contains("right_type"))
//                     {
//                         result.append($"\t{correct_type(operator[\"return_type\"])} operator{operator[\"name\"]}({type_for_parameter(operator[\"right_type\"])}other) const;");
//                     }
//                     else
//                     {
//                         result.append($"\t{correct_type(operator[\"return_type\"])} operator{operator[\"name\"].replace(\"unary\", \"\")}() const;");
//                     }
//                 }
//             }
//         }
//
//         // Copy assignment.
//         if (copy_constructor_index >= 0)
//         {
//             result.append($"\t{class_name} &operator=(const {class_name} &other);");
//         }
//
//         // Move assignment.
//         result.append($"\t{class_name} &operator=({class_name} &&other);");
//         // Special cases.
//         if (class_name == "String")
//         {
//             result.append("\tString &operator=(const char *p_str);");
//             result.append("\tString &operator=(const wchar_t *p_str);");
//             result.append("\tString &operator=(const char16_t *p_str);");
//             result.append("\tString &operator=(const char32_t *p_str);");
//             result.append("\tbool operator==(const char *p_str) const;");
//             result.append("\tbool operator==(const wchar_t *p_str) const;");
//             result.append("\tbool operator==(const char16_t *p_str) const;");
//             result.append("\tbool operator==(const char32_t *p_str) const;");
//             result.append("\tbool operator!=(const char *p_str) const;");
//             result.append("\tbool operator!=(const wchar_t *p_str) const;");
//             result.append("\tbool operator!=(const char16_t *p_str) const;");
//             result.append("\tbool operator!=(const char32_t *p_str) const;");
//             result.append("\tString operator+(const char *p_str);");
//             result.append("\tString operator+(const wchar_t *p_str);");
//             result.append("\tString operator+(const char16_t *p_str);");
//             result.append("\tString operator+(const char32_t *p_str);");
//             result.append("\tString operator+(char32_t p_char);");
//             result.append("\tString &operator+=(const String &p_str);");
//             result.append("\tString &operator+=(char32_t p_char);");
//             result.append("\tString &operator+=(const char *p_str);");
//             result.append("\tString &operator+=(const wchar_t *p_str);");
//             result.append("\tString &operator+=(const char32_t *p_str);");
//             result.append("\tconst char32_t &operator[](int p_index) const;");
//             result.append("\tchar32_t &operator[](int p_index);");
//             result.append("\tconst char32_t *ptr() const;");
//             result.append("\tchar32_t *ptrw();");
//         }
//
//         if (class_name == "Array")
//         {
//             result.append("\ttemplate <class... Args>");
//             result.append("\tstatic Array make(Args... args) {");
//             result.append("\t\treturn helpers::append_all(Array(), args...);");
//             result.append("\t}");
//         }
//
//         if (is_packed_array(class_name))
//         {
//             var return_type = correct_type(builtin_api["indexing_return_type"]);
//             if (class_name == "PackedByteArray")
//             {
//                 return_type = "uint8_t";
//             }
//             else if (class_name == "PackedInt32Array")
//             {
//                 return_type = "int32_t";
//             }
//             else if (class_name == "PackedFloat32Array")
//             {
//                 return_type = "float";
//             }
//
//             result.append($"\tconst {return_type} &operator[](int p_index) const;");
//             result.append($"\t{return_type} &operator[](int p_index);");
//             result.append($"\tconst {return_type} *ptr() const;");
//             result.append($"\t{return_type} *ptrw();");
//             var iterators = @"
//     struct Iterator {
// 		_FORCE_INLINE_ $TYPE &operator*() const {
// 			return *elem_ptr;
// 		}
// 		_FORCE_INLINE_ $TYPE *operator->() const { return elem_ptr; }
// 		_FORCE_INLINE_ Iterator &operator++() {
// 			elem_ptr++;
// 			return *this;
// 		}
// 		_FORCE_INLINE_ Iterator &operator--() {
// 			elem_ptr--;
// 			return *this;
// 		}
//
// 		_FORCE_INLINE_ bool operator==(const Iterator &b) const { return elem_ptr == b.elem_ptr; }
// 		_FORCE_INLINE_ bool operator!=(const Iterator &b) const { return elem_ptr != b.elem_ptr; }
//
// 		Iterator($TYPE *p_ptr) { elem_ptr = p_ptr; }
// 		Iterator() {}
// 		Iterator(const Iterator &p_it) { elem_ptr = p_it.elem_ptr; }
//
// 	private:
// 		$TYPE *elem_ptr = nullptr;
// 	};
//
// 	struct ConstIterator {
// 		_FORCE_INLINE_ const $TYPE &operator*() const {
// 			return *elem_ptr;
// 		}
// 		_FORCE_INLINE_ const $TYPE *operator->() const { return elem_ptr; }
// 		_FORCE_INLINE_ ConstIterator &operator++() {
// 			elem_ptr++;
// 			return *this;
// 		}
// 		_FORCE_INLINE_ ConstIterator &operator--() {
// 			elem_ptr--;
// 			return *this;
// 		}
//
// 		_FORCE_INLINE_ bool operator==(const ConstIterator &b) const { return elem_ptr == b.elem_ptr; }
// 		_FORCE_INLINE_ bool operator!=(const ConstIterator &b) const { return elem_ptr != b.elem_ptr; }
//
// 		ConstIterator(const $TYPE *p_ptr) { elem_ptr = p_ptr; }
// 		ConstIterator() {}
// 		ConstIterator(const ConstIterator &p_it) { elem_ptr = p_it.elem_ptr; }
//
// 	private:
// 		const $TYPE *elem_ptr = nullptr;
// 	};
//
// 	_FORCE_INLINE_ Iterator begin() {
// 		return Iterator(ptrw());
// 	}
// 	_FORCE_INLINE_ Iterator end() {
// 		return Iterator(ptrw() + size());
// 	}
//
// 	_FORCE_INLINE_ ConstIterator begin() const {
// 		return ConstIterator(ptr());
// 	}
// 	_FORCE_INLINE_ ConstIterator end() const {
// 		return ConstIterator(ptr() + size());
// 	}
// ";
//             result.append(iterators.Replace("$TYPE", return_type));
//         }
//
//         if (class_name == "Array")
//         {
//             result.append("\tconst Variant &operator[](int p_index) const;");
//             result.append("\tVariant &operator[](int p_index);");
//             result.append("\tvoid set_typed(uint32_t p_type, const StringName &p_class_name, const Variant &p_script);");
//             result.append("\tvoid _ref(const Array &p_from) const;");
//         }
//
//         if (class_name == "Dictionary")
//         {
//             result.append("\tconst Variant &operator[](const Variant &p_key) const;");
//             result.append("\tVariant &operator[](const Variant &p_key);");
//         }
//
//         result.append("};");
//         if (class_name == "String")
//         {
//             result.append("");
//             result.append("bool operator==(const char *p_chr, const String &p_str);");
//             result.append("bool operator==(const wchar_t *p_chr, const String &p_str);");
//             result.append("bool operator==(const char16_t *p_chr, const String &p_str);");
//             result.append("bool operator==(const char32_t *p_chr, const String &p_str);");
//             result.append("bool operator!=(const char *p_chr, const String &p_str);");
//             result.append("bool operator!=(const wchar_t *p_chr, const String &p_str);");
//             result.append("bool operator!=(const char16_t *p_chr, const String &p_str);");
//             result.append("bool operator!=(const char32_t *p_chr, const String &p_str);");
//             result.append("String operator+(const char *p_chr, const String &p_str);");
//             result.append("String operator+(const wchar_t *p_chr, const String &p_str);");
//             result.append("String operator+(const char16_t *p_chr, const String &p_str);");
//             result.append("String operator+(const char32_t *p_chr, const String &p_str);");
//             result.append("String operator+(char32_t p_char, const String &p_str);");
//             result.append("String itos(int64_t p_val);");
//             result.append("String uitos(uint64_t p_val);");
//             result.append("String rtos(double p_val);");
//             result.append("String rtoss(double p_val);");
//         }
//
//         result.append("");
//         result.append("} // namespace godot");
//         result.append($"#endif // ! {header_guard}");
//         return "\n".join(result);
//     }
//
//     public static object generate_builtin_class_source(object builtin_api, object size, object used_classes, object fully_used_classes)
//     {
//         object arguments;
//         object method_call;
//         object method_signature;
//         object right_type_variant_type;
//         var result = new List<object>();
//         var class_name = builtin_api["name"];
//         var snake_class_name = camel_to_snake(class_name);
//         var enum_type_name = $"GDEXTENSION_VARIANT_TYPE_{snake_class_name.upper()}";
//         add_header($"{snake_class_name}.cpp", result);
//         result.append("");
//         result.append($"#include <godot_cpp/variant/{snake_class_name}.hpp>");
//         result.append("");
//         result.append("#include <godot_cpp/core/binder_common.hpp>");
//         result.append("");
//         result.append("#include <godot_cpp/godot.hpp>");
//         result.append("");
//         // Only used since the "fully used" is included in header already.
//         foreach (var include in used_classes)
//         {
//             result.append($"#include <godot_cpp/{get_include_path(include)}>");
//         }
//
//         if (used_classes.Count > 0)
//         {
//             result.append("");
//         }
//
//         result.append("#include <godot_cpp/core/builtin_ptrcall.hpp>");
//         result.append("");
//         result.append("#include <utility>");
//         result.append("");
//         result.append("namespace godot {");
//         result.append("");
//         result.append($"{class_name}::_MethodBindings {class_name}::_method_bindings;");
//         result.append("");
//         result.append($"void {class_name}::_init_bindings_constructors_destructor() {{");
//         if (builtin_api.Contains("constructors"))
//         {
//             foreach (var constructor in builtin_api["constructors"])
//             {
//                 result.append($"\t_method_bindings.constructor_{constructor[\"index\"]} = internal::gdextension_interface_variant_get_ptr_constructor({enum_type_name}, {constructor[\"index\"]});");
//             }
//         }
//
//         if (builtin_api["has_destructor"])
//         {
//             result.append($"\t_method_bindings.destructor = internal::gdextension_interface_variant_get_ptr_destructor({enum_type_name});");
//         }
//
//         result.append("}");
//         result.append($"void {class_name}::init_bindings() {{");
//         // StringName's constructor internally uses String, so it constructor must be ready !
//         if (class_name == "StringName")
//         {
//             result.append("\tString::_init_bindings_constructors_destructor();");
//         }
//
//         result.append($"\t{class_name}::_init_bindings_constructors_destructor();");
//         result.append("\tStringName _gde_name;");
//         if (builtin_api.Contains("methods"))
//         {
//             foreach (var method in builtin_api["methods"])
//             {
//                 // TODO: Add error check for hash mismatch.
//                 result.append($"\t_gde_name = StringName(\"{method[\"name\"]}\");");
//                 result.append($"\t_method_bindings.method_{method[\"name\"]} = internal::gdextension_interface_variant_get_ptr_builtin_method({enum_type_name}, _gde_name._native_ptr(), {method[\"hash\"]});");
//             }
//         }
//
//         if (builtin_api.Contains("members"))
//         {
//             foreach (var member in builtin_api["members"])
//             {
//                 result.append($"\t_gde_name = StringName(\"{member[\"name\"]}\");");
//                 result.append($"\t_method_bindings.member_{member[\"name\"]}_setter = internal::gdextension_interface_variant_get_ptr_setter({enum_type_name}, _gde_name._native_ptr());");
//                 result.append($"\t_method_bindings.member_{member[\"name\"]}_getter = internal::gdextension_interface_variant_get_ptr_getter({enum_type_name}, _gde_name._native_ptr());");
//             }
//         }
//
//         if (builtin_api.Contains("indexing_return_type"))
//         {
//             result.append($"\t_method_bindings.indexed_setter = internal::gdextension_interface_variant_get_ptr_indexed_setter({enum_type_name});");
//             result.append($"\t_method_bindings.indexed_getter = internal::gdextension_interface_variant_get_ptr_indexed_getter({enum_type_name});");
//         }
//
//         if (builtin_api.Contains("is_keyed") && builtin_api["is_keyed"])
//         {
//             result.append($"\t_method_bindings.keyed_setter = internal::gdextension_interface_variant_get_ptr_keyed_setter({enum_type_name});");
//             result.append($"\t_method_bindings.keyed_getter = internal::gdextension_interface_variant_get_ptr_keyed_getter({enum_type_name});");
//             result.append($"\t_method_bindings.keyed_checker = internal::gdextension_interface_variant_get_ptr_keyed_checker({enum_type_name});");
//         }
//
//         if (builtin_api.Contains("operators"))
//         {
//             foreach (var @operator in builtin_api["operators"])
//             {
//                 if (@operator.Contains("right_type"))
//                 {
//                     if (@operator["right_type"] == "Variant")
//                     {
//                         right_type_variant_type = "GDEXTENSION_VARIANT_TYPE_NIL";
//                     }
//                     else
//                     {
//                         right_type_variant_type = $"GDEXTENSION_VARIANT_TYPE_{camel_to_snake(operator['right_type']).upper()}";
//                     }
//
//                     result.append($"\t_method_bindings.operator_{get_operator_id_name(operator[\"name\"])}_{operator[\"right_type\"]} = internal::gdextension_interface_variant_get_ptr_operator_evaluator(GDEXTENSION_VARIANT_OP_{get_operator_id_name(operator[\"name\"]).upper()}, {enum_type_name}, {right_type_variant_type});");
//                 }
//                 else
//                 {
//                     result.append($"\t_method_bindings.operator_{get_operator_id_name(operator[\"name\"])} = internal::gdextension_interface_variant_get_ptr_operator_evaluator(GDEXTENSION_VARIANT_OP_{get_operator_id_name(operator[\"name\"]).upper()}, {enum_type_name}, GDEXTENSION_VARIANT_TYPE_NIL);");
//                 }
//             }
//         }
//
//         result.append("}");
//         result.append("");
//         var copy_constructor_index = -1;
//         if (builtin_api.Contains("constructors"))
//         {
//             foreach (var constructor in builtin_api["constructors"])
//             {
//                 method_signature = $"{class_name}::{class_name}(";
//                 if (constructor.Contains("arguments"))
//                 {
//                     method_signature += make_function_parameters(constructor["arguments"], include_default : false, for_builtin : true);
//                 }
//
//                 method_signature += ") {";
//                 result.append(method_signature);
//                 method_call = $"\tinternal::_call_builtin_constructor(_method_bindings.constructor_{constructor[\"index\"]}, &opaque";
//                 if (constructor.Contains("arguments"))
//                 {
//                     if (constructor["arguments"].Count == 1 && constructor["arguments"][0]["type"] == class_name)
//                     {
//                         copy_constructor_index = constructor["index"];
//                     }
//
//                     method_call += ", ";
//                     arguments = new List<object>();
//                     foreach (var argument in constructor["arguments"])
//                     {
//                         (encode, arg_name) = get_encoded_arg(argument["name"], argument["type"], argument.Contains("meta") ? argument["meta"] : null);
//                         result += encode;
//                         arguments.append(arg_name);
//                     }
//
//                     method_call += ", ".join(arguments);
//                 }
//
//                 method_call += ");";
//                 result.append(method_call);
//                 result.append("}");
//                 result.append("");
//             }
//         }
//
//         // Move constructor.
//         result.append($"{class_name}::{class_name}({class_name} &&other) {{");
//         if (needs_copy_instead_of_move(class_name) && copy_constructor_index >= 0)
//         {
//             result.append($"\tinternal::_call_builtin_constructor(_method_bindings.constructor_{copy_constructor_index}, &opaque, &other);");
//         }
//         else
//         {
//             result.append("\tstd::swap(opaque, other.opaque);");
//         }
//
//         result.append("}");
//         result.append("");
//         if (builtin_api["has_destructor"])
//         {
//             result.append($"{class_name}::~{class_name}() {{");
//             result.append("\t_method_bindings.destructor(&opaque);");
//             result.append("}");
//             result.append("");
//         }
//
//         var method_list = new List<object>();
//         if (builtin_api.Contains("methods"))
//         {
//             foreach (var method in builtin_api["methods"])
//             {
//                 method_list.append(method["name"]);
//                 if (method.Contains("is_vararg") && method["is_vararg"])
//                 {
//                     // Done in the header because of the template.
//                     continue;
//                 }
//
//                 method_signature = make_signature(class_name, method, for_builtin : true);
//                 result.append(method_signature + "{");
//                 method_call = "\t";
//                 if (method.Contains("return_type"))
//                 {
//                     method_call += $"return internal::_call_builtin_method_ptr_ret<{correct_type(method[\"return_type\"])}>(";
//                 }
//                 else
//                 {
//                     method_call += "internal::_call_builtin_method_ptr_no_ret(";
//                 }
//
//                 method_call += $"_method_bindings.method_{method[\"name\"]}, ";
//                 if (method.Contains("is_static") && method["is_static"])
//                 {
//                     method_call += "nullptr";
//                 }
//                 else
//                 {
//                     method_call += "(GDExtensionTypePtr)&opaque";
//                 }
//
//                 if (method.Contains("arguments"))
//                 {
//                     arguments = new List<object>();
//                     method_call += ", ";
//                     foreach (var argument in method["arguments"])
//                     {
//                         (encode, arg_name) = get_encoded_arg(argument["name"], argument["type"], argument.Contains("meta") ? argument["meta"] : null);
//                         result += encode;
//                         arguments.append(arg_name);
//                     }
//
//                     method_call += ", ".join(arguments);
//                 }
//
//                 method_call += ");";
//                 result.append(method_call);
//                 result.append("}");
//                 result.append("");
//             }
//         }
//
//         if (builtin_api.Contains("members"))
//         {
//             foreach (var member in builtin_api["members"])
//             {
//                 if (!method_list.Contains($"get_{member[\"name\"]}"))
//                 {
//                     result.append($"{correct_type(member[\"type\"])} {class_name}::get_{member[\"name\"]}() const {{");
//                     result.append($"\treturn internal::_call_builtin_ptr_getter<{correct_type(member[\"type\"])}>(_method_bindings.member_{member[\"name\"]}_getter, (GDExtensionConstTypePtr)&opaque);");
//                     result.append("}");
//                 }
//
//                 if (!method_list.Contains($"set_{member[\"name\"]}"))
//                 {
//                     result.append($"void {class_name}::set_{member[\"name\"]}({type_for_parameter(member[\"type\"])}value) {{");
//                     (encode, arg_name) = get_encoded_arg("value", member["type"], null);
//                     result += encode;
//                     result.append($"\t_method_bindings.member_{member[\"name\"]}_setter((GDExtensionConstTypePtr)&opaque, (GDExtensionConstTypePtr){arg_name});");
//                     result.append("}");
//                 }
//
//                 result.append("");
//             }
//         }
//
//         if (builtin_api.Contains("operators"))
//         {
//             foreach (var @operator in builtin_api["operators"])
//             {
//                 if (!new List<object>
//                      {
//                          "in",
//                          "xor"
//                      }.Contains(@operator["name"]))
//                 {
//                     if (@operator.Contains("right_type"))
//                     {
//                         result.append($"{correct_type(operator[\"return_type\"])} {class_name}::operator{operator[\"name\"]}({type_for_parameter(operator[\"right_type\"])}other) const {{");
//                         (encode, arg_name) = get_encoded_arg("other", @operator["right_type"], null);
//                         result += encode;
//                         result.append($"\treturn internal::_call_builtin_operator_ptr<{get_gdextension_type(correct_type(operator[\"return_type\"]))}>(_method_bindings.operator_{get_operator_id_name(operator[\"name\"])}_{operator[\"right_type\"]}, (GDExtensionConstTypePtr)&opaque, (GDExtensionConstTypePtr){arg_name});");
//                         result.append("}");
//                     }
//                     else
//                     {
//                         result.append($"{correct_type(operator[\"return_type\"])} {class_name}::operator{operator[\"name\"].replace(\"unary\", \"\")}() const {{");
//                         result.append($"\treturn internal::_call_builtin_operator_ptr<{get_gdextension_type(correct_type(operator[\"return_type\"]))}>(_method_bindings.operator_{get_operator_id_name(operator[\"name\"])}, (GDExtensionConstTypePtr)&opaque, (GDExtensionConstTypePtr)nullptr);");
//                         result.append("}");
//                     }
//
//                     result.append("");
//                 }
//             }
//         }
//
//         // Copy assignment.
//         if (copy_constructor_index >= 0)
//         {
//             result.append($"{class_name} &{class_name}::operator=(const {class_name} &other) {{");
//             if (builtin_api["has_destructor"])
//             {
//                 result.append("\t_method_bindings.destructor(&opaque);");
//             }
//
//             (encode, arg_name) = get_encoded_arg("other", class_name, null);
//             result += encode;
//             result.append($"\tinternal::_call_builtin_constructor(_method_bindings.constructor_{copy_constructor_index}, &opaque, {arg_name});");
//             result.append("\treturn *this;");
//             result.append("}");
//             result.append("");
//         }
//
//         // Move assignment.
//         result.append($"{class_name} &{class_name}::operator=({class_name} &&other) {{");
//         if (needs_copy_instead_of_move(class_name) && copy_constructor_index >= 0)
//         {
//             result.append($"\tinternal::_call_builtin_constructor(_method_bindings.constructor_{copy_constructor_index}, &opaque, &other);");
//         }
//         else
//         {
//             result.append("\tstd::swap(opaque, other.opaque);");
//         }
//
//         result.append("\treturn *this;");
//         result.append("}");
//         result.append("");
//         result.append("} //namespace godot");
//         return "\n".join(result);
//     }
//
//     public static object generate_engine_classes_bindings(object api, object output_dir, object use_template_get_node)
//     {
//         object array_type_name;
//         object type_name;
//         object header_filename;
//         object used_classes;
//         var include_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "classes";
//         var source_gen_folder = Path(output_dir) / "src" / "classes";
//         include_gen_folder.mkdir(parents : true, exist_ok : true);
//         source_gen_folder.mkdir(parents : true, exist_ok : true);
//         // First create map of classes and singletons.
//         foreach (var class_api in api["classes"])
//         {
//             // Generate code for the ClassDB singleton under a different name.
//             if (class_api["name"] == "ClassDB")
//             {
//                 class_api["name"] = "ClassDBSingleton";
//                 class_api["alias_for"] = "ClassDB";
//             }
//
//             engine_classes[class_api["name"]] = class_api["is_refcounted"];
//         }
//
//         foreach (var native_struct in api["native_structures"])
//         {
//             engine_classes[native_struct["name"]] = false;
//             native_structures.append(native_struct["name"]);
//         }
//
//         foreach (var singleton in api["singletons"])
//         {
//             // Generate code for the ClassDB singleton under a different name.
//             if (singleton["name"] == "ClassDB")
//             {
//                 singleton["name"] = "ClassDBSingleton";
//                 singleton["alias_for"] = "ClassDB";
//             }
//
//             singletons.append(singleton["name"]);
//         }
//
//         foreach (var class_api in api["classes"])
//         {
//             // Check used classes for header include.
//             used_classes = new HashSet<object>();
//             var fully_used_classes = new HashSet<object>();
//             var class_name = class_api["name"];
//             header_filename = include_gen_folder / (camel_to_snake(class_api["name"]) + ".hpp");
//             var source_filename = source_gen_folder / (camel_to_snake(class_api["name"]) + ".cpp");
//             if (class_api.Contains("methods"))
//             {
//                 foreach (var method in class_api["methods"])
//                 {
//                     if (method.Contains("arguments"))
//                     {
//                         foreach (var argument in method["arguments"])
//                         {
//                             type_name = argument["type"];
//                             if (type_name.StartsWith("const "))
//                             {
//                                 type_name = type_name[6];
//                             }
//
//                             if (type_name.endswith("*"))
//                             {
//                                 type_name = type_name[: -1:];
//                             }
//
//                             if (is_included(type_name, class_name))
//                             {
//                                 if (type_name.StartsWith("typedarray::"))
//                                 {
//                                     fully_used_classes.add("TypedArray");
//                                     array_type_name = type_name.Replace("typedarray::", "");
//                                     if (array_type_name.StartsWith("const "))
//                                     {
//                                         array_type_name = array_type_name[6];
//                                     }
//
//                                     if (array_type_name.endswith("*"))
//                                     {
//                                         array_type_name = array_type_name[: -1:];
//                                     }
//
//                                     if (is_included(array_type_name, class_name))
//                                     {
//                                         if (is_enum(array_type_name))
//                                         {
//                                             fully_used_classes.add(get_enum_class(array_type_name));
//                                         }
//                                         else if (argument.Contains("default_value"))
//                                         {
//                                             fully_used_classes.add(array_type_name);
//                                         }
//                                         else
//                                         {
//                                             used_classes.add(array_type_name);
//                                         }
//                                     }
//                                 }
//                                 else if (is_enum(type_name))
//                                 {
//                                     fully_used_classes.add(get_enum_class(type_name));
//                                 }
//                                 else if (argument.Contains("default_value"))
//                                 {
//                                     fully_used_classes.add(type_name);
//                                 }
//                                 else
//                                 {
//                                     used_classes.add(type_name);
//                                 }
//
//                                 if (is_refcounted(type_name))
//                                 {
//                                     fully_used_classes.add("Ref");
//                                 }
//                             }
//                         }
//                     }
//
//                     if (method.Contains("return_value"))
//                     {
//                         type_name = method["return_value"]["type"];
//                         if (type_name.StartsWith("const "))
//                         {
//                             type_name = type_name[6];
//                         }
//
//                         if (type_name.endswith("*"))
//                         {
//                             type_name = type_name[: -1:];
//                         }
//
//                         if (is_included(type_name, class_name))
//                         {
//                             if (type_name.StartsWith("typedarray::"))
//                             {
//                                 fully_used_classes.add("TypedArray");
//                                 array_type_name = type_name.Replace("typedarray::", "");
//                                 if (array_type_name.StartsWith("const "))
//                                 {
//                                     array_type_name = array_type_name[6];
//                                 }
//
//                                 if (array_type_name.endswith("*"))
//                                 {
//                                     array_type_name = array_type_name[: -1:];
//                                 }
//
//                                 if (is_included(array_type_name, class_name))
//                                 {
//                                     if (is_enum(array_type_name))
//                                     {
//                                         fully_used_classes.add(get_enum_class(array_type_name));
//                                     }
//                                     else if (is_variant(array_type_name))
//                                     {
//                                         fully_used_classes.add(array_type_name);
//                                     }
//                                     else
//                                     {
//                                         used_classes.add(array_type_name);
//                                     }
//                                 }
//                             }
//                             else if (is_enum(type_name))
//                             {
//                                 fully_used_classes.add(get_enum_class(type_name));
//                             }
//                             else if (is_variant(type_name))
//                             {
//                                 fully_used_classes.add(type_name);
//                             }
//                             else
//                             {
//                                 used_classes.add(type_name);
//                             }
//
//                             if (is_refcounted(type_name))
//                             {
//                                 fully_used_classes.add("Ref");
//                             }
//                         }
//                     }
//                 }
//             }
//
//             if (class_api.Contains("members"))
//             {
//                 foreach (var member in class_api["members"])
//                 {
//                     if (is_included(member["type"], class_name))
//                     {
//                         if (is_enum(member["type"]))
//                         {
//                             fully_used_classes.add(get_enum_class(member["type"]));
//                         }
//                         else
//                         {
//                             used_classes.add(member["type"]);
//                         }
//
//                         if (is_refcounted(member["type"]))
//                         {
//                             fully_used_classes.add("Ref");
//                         }
//                     }
//                 }
//             }
//
//             if (class_api.Contains("inherits"))
//             {
//                 if (is_included(class_api["inherits"], class_name))
//                 {
//                     fully_used_classes.add(class_api["inherits"]);
//                 }
//
//                 if (is_refcounted(class_api["name"]))
//                 {
//                     fully_used_classes.add("Ref");
//                 }
//             }
//             else
//             {
//                 fully_used_classes.add("Wrapped");
//             }
//
//             // In order to ensure that PtrToArg specializations for native structs are
//             // always used, let's move any of them into 'fully_used_classes'.
//             foreach (var type_name in used_classes)
//             {
//                 if (is_struct_type(type_name) && !is_included_struct_type(type_name))
//                 {
//                     fully_used_classes.add(type_name);
//                 }
//             }
//
//             foreach (var type_name in fully_used_classes)
//             {
//                 if (used_classes.Contains(type_name))
//                 {
//                     used_classes.remove(type_name);
//                 }
//             }
//
//             used_classes = used_classes.ToList();
//             used_classes.sort();
//             fully_used_classes = fully_used_classes.ToList();
//             fully_used_classes.sort();
//             using (var header_file = header_filename.open("w+", encoding : "utf-8"))
//             {
//                 header_file.write(generate_engine_class_header(class_api, used_classes, fully_used_classes, use_template_get_node));
//             }
//
//             using (var source_file = source_filename.open("w+", encoding : "utf-8"))
//             {
//                 source_file.write(generate_engine_class_source(class_api, used_classes, fully_used_classes, use_template_get_node));
//             }
//         }
//
//         foreach (var native_struct in api["native_structures"])
//         {
//             var struct_name = native_struct["name"];
//             var snake_struct_name = camel_to_snake(struct_name);
//             header_filename = include_gen_folder / (snake_struct_name + ".hpp");
//             var result = new List<object>();
//             add_header($"{snake_struct_name}.hpp", result);
//             var header_guard = $"GODOT_CPP_{snake_struct_name.upper()}_HPP";
//             result.append($"#ifndef {header_guard}");
//             result.append($"#define {header_guard}");
//             used_classes = new List<object>();
//             var expanded_format = native_struct["format"].replace("(", " ").replace(")", ";").Replace(",", ";");
//             foreach (var field in expanded_format.Split(";"))
//             {
//                 var field_type = field.strip().split(" ")[0].Split("::")[0];
//                 if (field_type != "" && !is_included_type(field_type) && !is_pod_type(field_type))
//                 {
//                     if (!used_classes.Contains(field_type))
//                     {
//                         used_classes.append(field_type);
//                     }
//                 }
//             }
//
//             result.append("");
//             foreach (var included in used_classes)
//             {
//                 result.append($"#include <godot_cpp/{get_include_path(included)}>");
//             }
//
//             if (used_classes.Count == 0)
//             {
//                 result.append("#include <godot_cpp/core/method_ptrcall.hpp>");
//             }
//
//             result.append("");
//             result.append("namespace godot {");
//             result.append("");
//             result.append($"struct {struct_name} {{");
//             foreach (var field in native_struct["format"].Split(";"))
//             {
//                 if (field != "")
//                 {
//                     result.append($"\t{field};");
//                 }
//             }
//
//             result.append("};");
//             result.append("");
//             result.append($"GDVIRTUAL_NATIVE_PTR({struct_name});");
//             result.append("");
//             result.append("} // namespace godot");
//             result.append("");
//             result.append($"#endif // ! {header_guard}");
//             using (var header_file = header_filename.open("w+", encoding : "utf-8"))
//             {
//                 header_file.write("\n".join(result));
//             }
//         }
//     }
//
//     public static object generate_engine_class_header(object class_api, object used_classes, object fully_used_classes, object use_template_get_node)
//     {
//         object method_signature;
//         var result = new List<object>();
//         var class_name = class_api["name"];
//         var snake_class_name = camel_to_snake(class_name).upper();
//         var is_singleton = singletons.Contains(class_name);
//         add_header($"{snake_class_name.lower()}.hpp", result);
//         var header_guard = $"GODOT_CPP_{snake_class_name}_HPP";
//         result.append($"#ifndef {header_guard}");
//         result.append($"#define {header_guard}");
//         result.append("");
//         foreach (var included in fully_used_classes)
//         {
//             if (included == "TypedArray")
//             {
//                 result.append("#include <godot_cpp/variant/typed_array.hpp>");
//             }
//             else
//             {
//                 result.append($"#include <godot_cpp/{get_include_path(included)}>");
//             }
//         }
//
//         if (class_name == "EditorPlugin")
//         {
//             result.append("#include <godot_cpp/classes/editor_plugin_registration.hpp>");
//         }
//
//         if (fully_used_classes.Count > 0)
//         {
//             result.append("");
//         }
//
//         if (class_name != "Object" && class_name != "ClassDBSingleton")
//         {
//             result.append("#include <godot_cpp/core/class_db.hpp>");
//             result.append("");
//             result.append("#include <type_traits>");
//             result.append("");
//         }
//
//         result.append("namespace godot {");
//         result.append("");
//         foreach (var type_name in used_classes)
//         {
//             if (is_struct_type(type_name))
//             {
//                 result.append($"struct {type_name};");
//             }
//             else
//             {
//                 result.append($"class {type_name};");
//             }
//         }
//
//         if (used_classes.Count > 0)
//         {
//             result.append("");
//         }
//
//         var inherits = class_api.Contains("inherits") ? class_api["inherits"] : "Wrapped";
//         result.append($"class {class_name} : public {inherits} {{");
//         if (class_api.Contains("alias_for"))
//         {
//             result.append($"\tGDEXTENSION_CLASS_ALIAS({class_name}, {class_api['alias_for']}, {inherits})");
//         }
//         else
//         {
//             result.append($"\tGDEXTENSION_CLASS({class_name}, {inherits})");
//         }
//
//         result.append("");
//         result.append("public:");
//         result.append("");
//         if (class_api.Contains("enums"))
//         {
//             foreach (var enum_api in class_api["enums"])
//             {
//                 result.append($"\tenum {enum_api[\"name\"]} {{");
//                 foreach (var value in enum_api["values"])
//                 {
//                     result.append($"\t\t{value[\"name\"]} = {value[\"value\"]},");
//                 }
//
//                 result.append("\t};");
//                 result.append("");
//             }
//         }
//
//         if (class_api.Contains("constants"))
//         {
//             foreach (var value in class_api["constants"])
//             {
//                 if (!value.Contains("type"))
//                 {
//                     value["type"] = "int";
//                 }
//
//                 result.append($"\tstatic const {value[\"type\"]} {value[\"name\"]} = {value[\"value\"]};");
//             }
//
//             result.append("");
//         }
//
//         if (is_singleton)
//         {
//             result.append($"\tstatic {class_name} *get_singleton();");
//             result.append("");
//         }
//
//         if (class_api.Contains("methods"))
//         {
//             foreach (var method in class_api["methods"])
//             {
//                 if (method["is_virtual"])
//                 {
//                     // Will be done later.
//                     continue;
//                 }
//
//                 var vararg = method.Contains("is_vararg") && method["is_vararg"];
//                 method_signature = "\t";
//                 if (vararg)
//                 {
//                     method_signature += "private: ";
//                 }
//
//                 method_signature += make_signature(class_name, method, for_header : true, use_template_get_node : use_template_get_node);
//                 result.append(method_signature + ";");
//                 if (vararg)
//                 {
//                     // Add templated version.
//                     result += make_varargs_template(method);
//                 }
//             }
//
//             // Virtuals now.
//             foreach (var method in class_api["methods"])
//             {
//                 if (!method["is_virtual"])
//                 {
//                     continue;
//                 }
//
//                 method_signature = "\t";
//                 method_signature += make_signature(class_name, method, for_header : true, use_template_get_node : use_template_get_node);
//                 result.append(method_signature + ";");
//             }
//         }
//
//         result.append("protected:");
//         // T is the custom class we want to register (from which the call initiates, going up the inheritance chain),
//         // B is its base class (can be a custom class too, that's why we pass it).
//         result.append("\ttemplate <class T, class B>");
//         result.append("\tstatic void register_virtuals() {");
//         if (class_name != "Object")
//         {
//             result.append($"\t\t{inherits}::register_virtuals<T, B>();");
//             if (class_api.Contains("methods"))
//             {
//                 foreach (var method in class_api["methods"])
//                 {
//                     if (!method["is_virtual"])
//                     {
//                         continue;
//                     }
//
//                     var method_name = escape_identifier(method["name"]);
//                     result.append($"\t\tif constexpr (!std::is_same_v<decltype(&B::{method_name}),decltype(&T::{method_name})>) {{");
//                     result.append($"\t\t\tBIND_VIRTUAL_METHOD(T, {method_name});");
//                     result.append("\t\t}");
//                 }
//             }
//         }
//
//         result.append("\t}");
//         result.append("");
//         result.append("public:");
//         // Special cases.
//         if (class_name == "XMLParser")
//         {
//             result.append("\tError _open_buffer(const uint8_t *p_buffer, size_t p_size);");
//         }
//
//         if (class_name == "FileAccess")
//         {
//             result.append("\tuint64_t get_buffer(uint8_t *p_dst, uint64_t p_length) const;");
//             result.append("\tvoid store_buffer(const uint8_t *p_src, uint64_t p_length);");
//         }
//
//         if (class_name == "WorkerThreadPool")
//         {
//             result.append("\tenum {");
//             result.append("\tINVALID_TASK_ID = -1");
//             result.append("\t};");
//             result.append("\ttypedef int64_t TaskID;");
//             result.append("\ttypedef int64_t GroupID;");
//             result.append("\tTaskID add_native_task(void (*p_func)(void *), void *p_userdata, bool p_high_priority = false, const String &p_description = String());");
//             result.append("\tGroupID add_native_group_task(void (*p_func)(void *, uint32_t), void *p_userdata, int p_elements, int p_tasks = -1, bool p_high_priority = false, const String &p_description = String());");
//         }
//
//         if (class_name == "Object")
//         {
//             result.append("");
//             result.append("\ttemplate<class T>");
//             result.append("\tstatic T *cast_to(Object *p_object);");
//             result.append("\ttemplate<class T>");
//             result.append("\tstatic const T *cast_to(const Object *p_object);");
//             result.append("\tvirtual ~Object() = default;");
//         }
//         else if (use_template_get_node && class_name == "Node")
//         {
//             result.append("\ttemplate<class T>");
//             result.append("\tT *get_node(const NodePath &p_path) const { return Object::cast_to<T>(get_node_internal(p_path)); }");
//         }
//
//         result.append("");
//         result.append("};");
//         result.append("");
//         result.append("} // namespace godot");
//         result.append("");
//         if (class_api.Contains("enums") && class_name != "Object")
//         {
//             foreach (var enum_api in class_api["enums"])
//             {
//                 if (enum_api["is_bitfield"])
//                 {
//                     result.append($"VARIANT_BITFIELD_CAST({class_name}::{enum_api[\"name\"]});");
//                 }
//                 else
//                 {
//                     result.append($"VARIANT_ENUM_CAST({class_name}::{enum_api[\"name\"]});");
//                 }
//             }
//
//             result.append("");
//         }
//
//         if (class_name == "ClassDBSingleton")
//         {
//             result.append("#define CLASSDB_SINGLETON_FORWARD_METHODS \\");
//             foreach (var method in class_api["methods"])
//             {
//                 // ClassDBSingleton shouldn't have any static or vararg methods, but if some appear later, lets skip them.
//                 if (vararg)
//                 {
//                     continue;
//                 }
//
//                 if (method.Contains("is_static") && method["is_static"])
//                 {
//                     continue;
//                 }
//
//                 method_signature = "\tstatic ";
//                 if (method.Contains("return_type"))
//                 {
//                     method_signature += $"{correct_type(method[\"return_type\"])} ";
//                 }
//                 else if (method.Contains("return_value"))
//                 {
//                     method_signature += correct_type(method["return_value"]["type"], method["return_value"].get("meta", null)) + " ";
//                 }
//                 else
//                 {
//                     method_signature += "void ";
//                 }
//
//                 method_signature += $"{method[\"name\"]}(";
//                 var method_arguments = new List<object>();
//                 if (method.Contains("arguments"))
//                 {
//                     method_arguments = method["arguments"];
//                 }
//
//                 method_signature += make_function_parameters(method_arguments, include_default : true, for_builtin : true, is_vararg : false);
//                 method_signature += ") { \\";
//                 result.append(method_signature);
//                 var method_body = "\t\t";
//                 if (method.Contains("return_type") || method.Contains("return_value"))
//                 {
//                     method_body += "return ";
//                 }
//
//                 method_body += $"ClassDBSingleton::get_singleton()->{method[\"name\"]}(";
//                 method_body += ", ".join(map(x => escape_identifier(x["name"]), method_arguments));
//                 method_body += "); \\";
//                 result.append(method_body);
//                 result.append("\t} \\");
//             }
//
//             result.append("\t;");
//             result.append("");
//         }
//
//         result.append($"#endif // ! {header_guard}");
//         return "\n".join(result);
//     }
//
//     public static object generate_engine_class_source(object class_api, object used_classes, object fully_used_classes, object use_template_get_node)
//     {
//         object return_type;
//         object method_signature;
//         var result = new List<object>();
//         var class_name = class_api["name"];
//         var snake_class_name = camel_to_snake(class_name);
//         var is_singleton = singletons.Contains(class_name);
//         add_header($"{snake_class_name}.cpp", result);
//         result.append($"#include <godot_cpp/classes/{snake_class_name}.hpp>");
//         result.append("");
//         result.append("#include <godot_cpp/core/engine_ptrcall.hpp>");
//         result.append("#include <godot_cpp/core/error_macros.hpp>");
//         result.append("");
//         foreach (var included in used_classes)
//         {
//             result.append($"#include <godot_cpp/{get_include_path(included)}>");
//         }
//
//         if (used_classes.Count > 0)
//         {
//             result.append("");
//         }
//
//         result.append("namespace godot {");
//         result.append("");
//         if (is_singleton)
//         {
//             result.append($"{class_name} *{class_name}::get_singleton() {{");
//             // We assume multi-threaded access is OK because each assignment will assign the same value every time
//             result.append($"\tstatic {class_name} *singleton = nullptr;");
//             result.append("\tif (unlikely(singleton == nullptr)) {");
//             result.append($"\t\tGDExtensionObjectPtr singleton_obj = internal::gdextension_interface_global_get_singleton({class_name}::get_class_static()._native_ptr());");
//             result.append("#ifdef DEBUG_ENABLED");
//             result.append("\t\tERR_FAIL_NULL_V(singleton_obj, nullptr);");
//             result.append("#endif // DEBUG_ENABLED");
//             result.append($"\t\tsingleton = reinterpret_cast<{class_name} *>(internal::gdextension_interface_object_get_instance_binding(singleton_obj, internal::token, &{class_name}::_gde_binding_callbacks));");
//             result.append("#ifdef DEBUG_ENABLED");
//             result.append("\t\tERR_FAIL_NULL_V(singleton, nullptr);");
//             result.append("#endif // DEBUG_ENABLED");
//             result.append("\t}");
//             result.append("\treturn singleton;");
//             result.append("}");
//             result.append("");
//         }
//
//         if (class_api.Contains("methods"))
//         {
//             foreach (var method in class_api["methods"])
//             {
//                 if (method["is_virtual"])
//                 {
//                     // Will be done later
//                     continue;
//                 }
//
//                 var vararg = method.Contains("is_vararg") && method["is_vararg"];
//                 // Method signature.
//                 method_signature = make_signature(class_name, method, use_template_get_node : use_template_get_node);
//                 result.append(method_signature + " {");
//                 // Method body.
//                 result.append($"\tstatic GDExtensionMethodBindPtr _gde_method_bind = internal::gdextension_interface_classdb_get_method_bind({class_name}::get_class_static()._native_ptr(), StringName(\"{method[\"name\"]}\")._native_ptr(), {method[\"hash\"]});");
//                 var method_call = "\t";
//                 var has_return = method.Contains("return_value") && method["return_value"]["type"] != "void";
//                 if (has_return)
//                 {
//                     result.append($"\tCHECK_METHOD_BIND_RET(_gde_method_bind, {get_default_value_for_type(method[\"return_value\"][\"type\"])});");
//                 }
//                 else
//                 {
//                     result.append("\tCHECK_METHOD_BIND(_gde_method_bind);");
//                 }
//
//                 var is_ref = false;
//                 if (!vararg)
//                 {
//                     if (has_return)
//                     {
//                         return_type = method["return_value"]["type"];
//                         var meta_type = method["return_value"].Contains("meta") ? method["return_value"]["meta"] : null;
//                         if (is_enum(return_type))
//                         {
//                             if (method["is_static"])
//                             {
//                                 method_call += $"return ({get_gdextension_type(correct_type(return_type, meta_type))})internal::_call_native_mb_ret<int64_t>(_gde_method_bind, nullptr";
//                             }
//                             else
//                             {
//                                 method_call += $"return ({get_gdextension_type(correct_type(return_type, meta_type))})internal::_call_native_mb_ret<int64_t>(_gde_method_bind, _owner";
//                             }
//                         }
//                         else if (is_pod_type(return_type) || is_variant(return_type))
//                         {
//                             if (method["is_static"])
//                             {
//                                 method_call += $"return internal::_call_native_mb_ret<{get_gdextension_type(correct_type(return_type, meta_type))}>(_gde_method_bind, nullptr";
//                             }
//                             else
//                             {
//                                 method_call += $"return internal::_call_native_mb_ret<{get_gdextension_type(correct_type(return_type, meta_type))}>(_gde_method_bind, _owner";
//                             }
//                         }
//                         else if (is_refcounted(return_type))
//                         {
//                             if (method["is_static"])
//                             {
//                                 method_call += $"return Ref<{return_type}>::_gde_internal_constructor(internal::_call_native_mb_ret_obj<{return_type}>(_gde_method_bind, nullptr";
//                             }
//                             else
//                             {
//                                 method_call += $"return Ref<{return_type}>::_gde_internal_constructor(internal::_call_native_mb_ret_obj<{return_type}>(_gde_method_bind, _owner";
//                             }
//
//                             is_ref = true;
//                         }
//                         else if (method["is_static"])
//                         {
//                             method_call += $"return internal::_call_native_mb_ret_obj<{return_type}>(_gde_method_bind, nullptr";
//                         }
//                         else
//                         {
//                             method_call += $"return internal::_call_native_mb_ret_obj<{return_type}>(_gde_method_bind, _owner";
//                         }
//                     }
//                     else if (method["is_static"])
//                     {
//                         method_call += "internal::_call_native_mb_no_ret(_gde_method_bind, nullptr";
//                     }
//                     else
//                     {
//                         method_call += "internal::_call_native_mb_no_ret(_gde_method_bind, _owner";
//                     }
//
//                     if (method.Contains("arguments"))
//                     {
//                         method_call += ", ";
//                         var arguments = new List<object>();
//                         foreach (var argument in method["arguments"])
//                         {
//                             (encode, arg_name) = get_encoded_arg(argument["name"], argument["type"], argument.Contains("meta") ? argument["meta"] : null);
//                             result += encode;
//                             arguments.append(arg_name);
//                         }
//
//                         method_call += ", ".join(arguments);
//                     }
//                 }
//                 else
//                 {
//                     // vararg.
//                     result.append("\tGDExtensionCallError error;");
//                     result.append("\tVariant ret;");
//                     method_call += "internal::gdextension_interface_object_method_bind_call(_gde_method_bind, _owner, reinterpret_cast<GDExtensionConstVariantPtr *>(args), arg_count, &ret, &error";
//                 }
//
//                 if (is_ref)
//                 {
//                     method_call += ")";
//                 }
//
//                 method_call += ");";
//                 result.append(method_call);
//                 if (vararg && (method.Contains("return_value") && method["return_value"]["type"] != "void"))
//                 {
//                     return_type = get_enum_fullname(method["return_value"]["type"]);
//                     if (return_type != "Variant")
//                     {
//                         result.append($"\treturn VariantCaster<{return_type}>::cast(ret);");
//                     }
//                     else
//                     {
//                         result.append("\treturn ret;");
//                     }
//                 }
//
//                 result.append("}");
//                 result.append("");
//             }
//
//             // Virtuals now.
//             foreach (var method in class_api["methods"])
//             {
//                 if (!method["is_virtual"])
//                 {
//                     continue;
//                 }
//
//                 method_signature = make_signature(class_name, method, use_template_get_node : use_template_get_node);
//                 method_signature += " {";
//                 if (method.Contains("return_value") && correct_type(method["return_value"]["type"]) != "void")
//                 {
//                     result.append(method_signature);
//                     result.append($"\treturn {get_default_value_for_type(method[\"return_value\"][\"type\"])};");
//                     result.append("}");
//                 }
//                 else
//                 {
//                     method_signature += "}";
//                     result.append(method_signature);
//                 }
//
//                 result.append("");
//             }
//         }
//
//         result.append("");
//         result.append("} // namespace godot ");
//         return "\n".join(result);
//     }
//
//     public static object generate_global_constants(object api, object output_dir)
//     {
//         var include_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "classes";
//         var source_gen_folder = Path(output_dir) / "src" / "classes";
//         include_gen_folder.mkdir(parents : true, exist_ok : true);
//         source_gen_folder.mkdir(parents : true, exist_ok : true);
//         // Generate header
//         var header = new List<object>();
//         add_header("global_constants.hpp", header);
//         var header_filename = include_gen_folder / "global_constants.hpp";
//         var header_guard = "GODOT_CPP_GLOBAL_CONSTANTS_HPP";
//         header.append($"#ifndef {header_guard}");
//         header.append($"#define {header_guard}");
//         header.append("");
//         header.append("namespace godot {");
//         header.append("");
//         foreach (var constant in api["global_constants"])
//         {
//             header.append($"\tconst int {escape_identifier(constant[\"name\"])} = {constant[\"value\"]};");
//         }
//
//         header.append("");
//         foreach (var enum_def in api["global_enums"])
//         {
//             if (enum_def["name"].StartsWith("Variant."))
//             {
//                 continue;
//             }
//
//             header.append($"\tenum {enum_def[\"name\"]} {{");
//             foreach (var value in enum_def["values"])
//             {
//                 header.append($"\t\t{value[\"name\"]} = {value[\"value\"]},");
//             }
//
//             header.append("\t};");
//             header.append("");
//         }
//
//         header.append("} // namespace godot");
//         header.append("");
//         header.append($"#endif // ! {header_guard}");
//         using (var header_file = header_filename.open("w+", encoding : "utf-8"))
//         {
//             header_file.write("\n".join(header));
//         }
//     }
//
//     public static object generate_version_header(object api, object output_dir)
//     {
//         var header = new List<object>();
//         var header_filename = "version.hpp";
//         add_header(header_filename, header);
//         var include_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "core";
//         include_gen_folder.mkdir(parents : true, exist_ok : true);
//         var header_file_path = include_gen_folder / header_filename;
//         var header_guard = "GODOT_CPP_VERSION_HPP";
//         header.append($"#ifndef {header_guard}");
//         header.append($"#define {header_guard}");
//         header.append("");
//         header.append($"#define GODOT_VERSION_MAJOR {api['header']['version_major']}");
//         header.append($"#define GODOT_VERSION_MINOR {api['header']['version_minor']}");
//         header.append($"#define GODOT_VERSION_PATCH {api['header']['version_patch']}");
//         header.append($"#define GODOT_VERSION_STATUS \"{api['header']['version_status']}\"");
//         header.append($"#define GODOT_VERSION_BUILD \"{api['header']['version_build']}\"");
//         header.append("");
//         header.append($"#endif // {header_guard}");
//         header.append("");
//         using (var header_file = header_file_path.open("w+", encoding : "utf-8"))
//         {
//             header_file.write("\n".join(header));
//         }
//     }
//
//     public static object generate_global_constant_binds(object api, object output_dir)
//     {
//         var include_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "classes";
//         var source_gen_folder = Path(output_dir) / "src" / "classes";
//         include_gen_folder.mkdir(parents : true, exist_ok : true);
//         source_gen_folder.mkdir(parents : true, exist_ok : true);
//         // Generate header
//         var header = new List<object>();
//         add_header("global_constants_binds.hpp", header);
//         var header_filename = include_gen_folder / "global_constants_binds.hpp";
//         var header_guard = "GODOT_CPP_GLOBAL_CONSTANTS_BINDS_HPP";
//         header.append($"#ifndef {header_guard}");
//         header.append($"#define {header_guard}");
//         header.append("");
//         header.append("#include <godot_cpp/classes/global_constants.hpp>");
//         header.append("");
//         foreach (var enum_def in api["global_enums"])
//         {
//             if (enum_def["name"].StartsWith("Variant."))
//             {
//                 continue;
//             }
//
//             if (enum_def["is_bitfield"])
//             {
//                 header.append($"VARIANT_BITFIELD_CAST(godot::{enum_def[\"name\"]});");
//             }
//             else
//             {
//                 header.append($"VARIANT_ENUM_CAST(godot::{enum_def[\"name\"]});");
//             }
//         }
//
//         // Variant::Type is not a global enum, but only one line, it is worth to place in this file instead of creating new file.
//         header.append($"VARIANT_ENUM_CAST(godot::Variant::Type);");
//         header.append("");
//         header.append($"#endif // ! {header_guard}");
//         using (var header_file = header_filename.open("w+", encoding : "utf-8"))
//         {
//             header_file.write("\n".join(header));
//         }
//     }
//
//     public static object generate_utility_functions(object api, object output_dir)
//     {
//         object function_signature;
//         object vararg;
//         var include_gen_folder = Path(output_dir) / "include" / "godot_cpp" / "variant";
//         var source_gen_folder = Path(output_dir) / "src" / "variant";
//         include_gen_folder.mkdir(parents : true, exist_ok : true);
//         source_gen_folder.mkdir(parents : true, exist_ok : true);
//         // Generate header.
//         var header = new List<object>();
//         add_header("utility_functions.hpp", header);
//         var header_filename = include_gen_folder / "utility_functions.hpp";
//         var header_guard = "GODOT_CPP_UTILITY_FUNCTIONS_HPP";
//         header.append($"#ifndef {header_guard}");
//         header.append($"#define {header_guard}");
//         header.append("");
//         header.append("#include <godot_cpp/variant/builtin_types.hpp>");
//         header.append("#include <godot_cpp/variant/variant.hpp>");
//         header.append("");
//         header.append("#include <array>");
//         header.append("");
//         header.append("namespace godot {");
//         header.append("");
//         header.append("class UtilityFunctions {");
//         header.append("public:");
//         foreach (var function in api["utility_functions"])
//         {
//             vararg = function.Contains("is_vararg") && function["is_vararg"];
//             function_signature = "\t";
//             function_signature += make_signature("UtilityFunctions", function, for_header : true, @static : true);
//             header.append(function_signature + ";");
//             if (vararg)
//             {
//                 // Add templated version.
//                 header += make_varargs_template(function, @static : true);
//             }
//         }
//
//         header.append("};");
//         header.append("");
//         header.append("} // namespace godot");
//         header.append("");
//         header.append($"#endif // ! {header_guard}");
//         using (var header_file = header_filename.open("w+", encoding : "utf-8"))
//         {
//             header_file.write("\n".join(header));
//         }
//
//         // Generate source.
//         var source = new List<object>();
//         add_header("utility_functions.cpp", source);
//         var source_filename = source_gen_folder / "utility_functions.cpp";
//         source.append("#include <godot_cpp/variant/utility_functions.hpp>");
//         source.append("");
//         source.append("#include <godot_cpp/core/error_macros.hpp>");
//         source.append("#include <godot_cpp/core/engine_ptrcall.hpp>");
//         source.append("");
//         source.append("namespace godot {");
//         source.append("");
//         foreach (var function in api["utility_functions"])
//         {
//             vararg = function.Contains("is_vararg") && function["is_vararg"];
//             function_signature = make_signature("UtilityFunctions", function);
//             source.append(function_signature + " {");
//             // Function body.
//             source.append($"\tstatic GDExtensionPtrUtilityFunction _gde_function = internal::gdextension_interface_variant_get_ptr_utility_function(StringName(\"{function[\"name\"]}\")._native_ptr(), {function[\"hash\"]});");
//             var has_return = function.Contains("return_type") && function["return_type"] != "void";
//             if (has_return)
//             {
//                 source.append($"\tCHECK_METHOD_BIND_RET(_gde_function, {get_default_value_for_type(function[\"return_type\"])});");
//             }
//             else
//             {
//                 source.append("\tCHECK_METHOD_BIND(_gde_function);");
//             }
//
//             var function_call = "\t";
//             if (!vararg)
//             {
//                 if (has_return)
//                 {
//                     function_call += "return ";
//                     if (function["return_type"] == "Object")
//                     {
//                         function_call += "internal::_call_utility_ret_obj(_gde_function";
//                     }
//                     else
//                     {
//                         function_call += $"internal::_call_utility_ret<{get_gdextension_type(correct_type(function[\"return_type\"]))}>(_gde_function";
//                     }
//                 }
//                 else
//                 {
//                     function_call += "internal::_call_utility_no_ret(_gde_function";
//                 }
//
//                 if (function.Contains("arguments"))
//                 {
//                     function_call += ", ";
//                     var arguments = new List<object>();
//                     foreach (var argument in function["arguments"])
//                     {
//                         (encode, arg_name) = get_encoded_arg(argument["name"], argument["type"], argument.Contains("meta") ? argument["meta"] : null);
//                         source += encode;
//                         arguments.append(arg_name);
//                     }
//
//                     function_call += ", ".join(arguments);
//                 }
//             }
//             else
//             {
//                 if (has_return)
//                 {
//                     source.append($"\t{get_gdextension_type(correct_type(function[\"return_type\"]))} ret;");
//                 }
//                 else
//                 {
//                     source.append("\tVariant ret;");
//                 }
//
//                 function_call += "_gde_function(&ret, reinterpret_cast<GDExtensionConstVariantPtr *>(args), arg_count";
//             }
//
//             function_call += ");";
//             source.append(function_call);
//             if (vararg && has_return)
//             {
//                 source.append("\treturn ret;");
//             }
//
//             source.append("}");
//             source.append("");
//         }
//
//         source.append("} // namespace godot");
//         using (var source_file = source_filename.open("w+", encoding : "utf-8"))
//         {
//             source_file.write("\n".join(source));
//         }
//     }
//
//     // Helper functions.
//     public static object camel_to_snake(object name)
//     {
//         name = re.sub("(.)([A-Z][a-z]+)", @"\1_\2", name);
//         name = re.sub("([a-z0-9])([A-Z])", @"\1_\2", name);
//         return name.replace("2_D", "2D").Replace("3_D", "3D").lower();
//     }
//
//     public static object make_function_parameters(object parameters, object include_default = false, object for_builtin = false, object is_vararg = false)
//     {
//         var signature = new List<object>();
//         foreach (var (index, par) in parameters.Select((_p_1, _p_2) => Tuple.Create(_p_2, _p_1)))
//         {
//             var parameter = type_for_parameter(par["type"], par.Contains("meta") ? par["meta"] : null);
//             var parameter_name = escape_identifier(par["name"]);
//             if (parameter_name.Count == 0)
//             {
//                 parameter_name = "arg_" + (index + 1).ToString();
//             }
//
//             parameter += parameter_name;
//             if (include_default && par.Contains("default_value") && (!for_builtin || par["type"] != "Variant"))
//             {
//                 parameter += " = ";
//                 if (is_enum(par["type"]))
//                 {
//                     var parameter_type = correct_type(par["type"]);
//                     if (parameter_type == "void")
//                     {
//                         parameter_type = "Variant";
//                     }
//
//                     parameter += $"({parameter_type})";
//                 }
//
//                 parameter += CorrectDefaultValue(par["default_value"], par["type"]);
//             }
//
//             signature.append(parameter);
//         }
//
//         if (is_vararg)
//         {
//             signature.append("const Args&... args");
//         }
//
//         return ", ".join(signature);
//     }
//
//     public static object type_for_parameter(object type_name, object meta = null)
//     {
//         if (type_name == "void")
//         {
//             return "Variant ";
//         }
//         else if (is_pod_type(type_name) && type_name != "Nil" || is_enum(type_name))
//         {
//             return $"{correct_type(type_name, meta)} ";
//         }
//         else if (is_variant(type_name) || is_refcounted(type_name))
//         {
//             return $"const {correct_type(type_name)} &";
//         }
//         else
//         {
//             return $"{correct_type(type_name)}";
//         }
//     }
//
//     public static object get_include_path(object type_name)
//     {
//         var base_dir = "";
//         if (type_name == "Object")
//         {
//             base_dir = "core";
//         }
//         else if (is_variant(type_name))
//         {
//             base_dir = "variant";
//         }
//         else
//         {
//             base_dir = "classes";
//         }
//
//         return $"{base_dir}/{camel_to_snake(type_name)}.hpp";
//     }
//
//     public static object get_encoded_arg(object arg_name, object type_name, object type_meta)
//     {
//         var result = new List<object>();
//         var name = escape_identifier(arg_name);
//         var arg_type = correct_type(type_name);
//         if (is_pod_type(arg_type))
//         {
//             result.append($"\t{get_gdextension_type(arg_type)} {name}_encoded;");
//             result.append($"\tPtrToArg<{correct_type(type_name)}>::encode({name}, &{name}_encoded);");
//             name = $"&{name}_encoded";
//         }
//         else if (is_engine_class(type_name))
//         {
//             // `{name}` is a C++ wrapper, it contains a field which is the object's pointer Godot expects.
//             // We have to check `nullptr` because when the caller sends `nullptr`, the wrapper itself will be null.
//             name = $"({name} != nullptr ? &{name}->_owner : nullptr)";
//         }
//         else
//         {
//             name = $"&{name}";
//         }
//
//         return (result, name);
//     }
//
//     public static object make_signature
//         (
//             object class_name,
//             object function_data,
//             object for_header = false,
//             object use_template_get_node = true,
//             object for_builtin = false,
//             object @static = false
//         )
//     {
//         var function_signature = "";
//         var is_vararg = function_data.Contains("is_vararg") && function_data["is_vararg"];
//         if (for_header)
//         {
//             if (function_data.Contains("is_virtual") && function_data["is_virtual"])
//             {
//                 function_signature += "virtual ";
//             }
//
//             if (is_vararg)
//             {
//                 function_signature += "private: ";
//             }
//
//             if (@static)
//             {
//                 function_signature += "static ";
//             }
//         }
//
//         var return_type = "void";
//         object return_meta = null;
//         if (function_data.Contains("return_type"))
//         {
//             return_type = correct_type(function_data["return_type"]);
//         }
//         else if (function_data.Contains("return_value"))
//         {
//             return_type = function_data["return_value"]["type"];
//             return_meta = function_data["return_value"].Contains("meta") ? function_data["return_value"]["meta"] : null;
//         }
//
//         function_signature += correct_type(return_type, return_meta);
//         if (!function_signature.endswith("*"))
//         {
//             function_signature += " ";
//         }
//
//         if (!for_header)
//         {
//             function_signature += $"{class_name}::";
//         }
//
//         function_signature += escape_identifier(function_data["name"]);
//         if (is_vararg || !for_builtin && use_template_get_node && class_name == "Node" && function_data["name"] == "get_node")
//         {
//             function_signature += "_internal";
//         }
//
//         function_signature += "(";
//         var arguments = function_data.Contains("arguments") ? function_data["arguments"] : new List<object>();
//         if (!is_vararg)
//         {
//             function_signature += make_function_parameters(arguments, for_header, for_builtin, is_vararg);
//         }
//         else
//         {
//             function_signature += "const Variant **args, GDExtensionInt arg_count";
//         }
//
//         function_signature += ")";
//         if (function_data.Contains("is_static") && function_data["is_static"] && for_header)
//         {
//             function_signature = "static " + function_signature;
//         }
//         else if (function_data.Contains("is_const") && function_data["is_const"])
//         {
//             function_signature += " const";
//         }
//
//         return function_signature;
//     }
//
//     public static object make_varargs_template(object function_data, object @static = false)
//     {
//         var result = new List<object>();
//         var function_signature = "\tpublic: template<class... Args> ";
//         if (@static)
//         {
//             function_signature += "static ";
//         }
//
//         var return_type = "void";
//         object return_meta = null;
//         if (function_data.Contains("return_type"))
//         {
//             return_type = correct_type(function_data["return_type"]);
//         }
//         else if (function_data.Contains("return_value"))
//         {
//             return_type = function_data["return_value"]["type"];
//             return_meta = function_data["return_value"].Contains("meta") ? function_data["return_value"]["meta"] : null;
//         }
//
//         function_signature += correct_type(return_type, return_meta);
//         if (!function_signature.endswith("*"))
//         {
//             function_signature += " ";
//         }
//
//         function_signature += $"{escape_identifier(function_data[\"name\"])}";
//         var method_arguments = new List<object>();
//         if (function_data.Contains("arguments"))
//         {
//             method_arguments = function_data["arguments"];
//         }
//
//         function_signature += "(";
//         var is_vararg = function_data.Contains("is_vararg") && function_data["is_vararg"];
//         function_signature += make_function_parameters(method_arguments, include_default : true, is_vararg : is_vararg);
//         function_signature += ")";
//         if (function_data.Contains("is_const") && function_data["is_const"])
//         {
//             function_signature += " const";
//         }
//
//         function_signature += " {";
//         result.append(function_signature);
//         var args_array = $"\t\tstd::array<Variant, {len(method_arguments)} + sizeof...(Args)> variant_args {{ ";
//         foreach (var argument in method_arguments)
//         {
//             if (argument["type"] == "Variant")
//             {
//                 args_array += argument["name"];
//             }
//             else
//             {
//                 args_array += $"Variant({argument[\"name\"]})";
//             }
//
//             args_array += ", ";
//         }
//
//         args_array += "Variant(args)... };";
//         result.append(args_array);
//         result.append($"\t\tstd::array<const Variant *, {len(method_arguments)} + sizeof...(Args)> call_args;");
//         result.append("\t\tfor(size_t i = 0; i < variant_args.size(); i++) {");
//         result.append("\t\t\tcall_args[i] = &variant_args[i];");
//         result.append("\t\t}");
//         var call_line = "\t\t";
//         if (return_type != "void")
//         {
//             call_line += "return ";
//         }
//
//         call_line += $"{escape_identifier(function_data[\"name\"])}_internal(call_args.data(), variant_args.size());";
//         result.append(call_line);
//         result.append("\t}");
//         return result;
//     }
//
//     // Engine idiosyncrasies.
//     // 
//     //     Those are types for which no class should be generated.
//     //     
//     public static bool is_pod_type(string type_name)
//     {
//         return new List<object>
//                {
//                    "Nil",
//                    "void",
//                    "bool",
//                    "real_t",
//                    "float",
//                    "double",
//                    "int",
//                    "int8_t",
//                    "uint8_t",
//                    "int16_t",
//                    "uint16_t",
//                    "int32_t",
//                    "int64_t",
//                    "uint32_t",
//                    "uint64_t"
//                }.Contains(type_name);
//     }
//
//     public static bool is_included_type(string type_name)
//     {
//         // Types which we already have implemented.
//         return is_included_struct_type(type_name) ||
//                new List<object>
//                {
//                    "ObjectID"
//                }.Contains(type_name);
//     }
//
//     public static bool is_included_struct_type(string type_name)
//     {
//         // Struct types which we already have implemented.
//         return new List<object>
//                {
//                    "AABB",
//                    "Basis",
//                    "Color",
//                    "Plane",
//                    "Projection",
//                    "Quaternion",
//                    "Rect2",
//                    "Rect2i",
//                    "Transform2D",
//                    "Transform3D",
//                    "Vector2",
//                    "Vector2i",
//                    "Vector3",
//                    "Vector3i",
//                    "Vector4",
//                    "Vector4i"
//                }.Contains(type_name);
//     }
//
//     // 
//     //     Those are types for which we add our extra packed array functions.
//     //     
//     public static bool is_packed_array(string type_name)
//     {
//         return new List<object>
//                {
//                    "PackedByteArray",
//                    "PackedColorArray",
//                    "PackedFloat32Array",
//                    "PackedFloat64Array",
//                    "PackedInt32Array",
//                    "PackedInt64Array",
//                    "PackedStringArray",
//                    "PackedVector2Array",
//                    "PackedVector3Array"
//                }.Contains(type_name);
//     }
//
//     // 
//     //     Those are types which need initialised data or we'll get warning spam so need a copy instead of move.
//     //     
//     public static bool needs_copy_instead_of_move(string type_name)
//     {
//         return new List<object>
//                {
//                    "Dictionary"
//                }.Contains(type_name);
//     }
//
//     public static bool is_enum(string type_name) { return type_name.StartsWith("enum::") || type_name.StartsWith("bitfield::"); }
//
//     public static bool is_bitfield(string type_name) { return type_name.StartsWith("bitfield::"); }
//
//     private static readonly char[] _spitArgs =
//         new[]
//         {
//             ','
//         };
//
//     public static string get_enum_class(string enum_name)
//     {
//         if (enum_name.Contains("."))
//         {
//             if (is_bitfield(enum_name))
//             {
//                 return enum_name.Replace("bitfield::", "").Split(_spitArgs)[0];
//             }
//             else
//             {
//                 return enum_name.Replace("enum::", "").Split(_spitArgs)[0];
//             }
//         }
//         else
//         {
//             return "GlobalConstants";
//         }
//     }
//
//     public static string get_enum_fullname(string enum_name)
//     {
//         if (is_bitfield(enum_name))
//         {
//             return enum_name.Replace("bitfield::", "BitField<") + ">";
//         }
//         else
//         {
//             return enum_name.Replace("enum::", "");
//         }
//     }
//
//     public static string get_enum_name(string enum_name)
//     {
//         if (is_bitfield(enum_name))
//         {
//             return enum_name.Replace("bitfield::", "").Split(_spitArgs)[^1];
//         }
//         else
//         {
//             return enum_name.Replace("enum::", "").Split(_spitArgs)[^1];
//         }
//     }
//
//     public static bool is_variant(string type_name) { return type_name == "Variant" || builtin_classes.Contains(type_name) || type_name == "Nil" || type_name.StartsWith("typedarray::"); }
//
//     public static bool is_engine_class(string type_name) { return type_name == "Object" || engine_classes.Contains(type_name); }
//
//     public static bool is_struct_type(string type_name)
//     {
//         // This is used to determine which keyword to use for forward declarations.
//         return is_included_struct_type(type_name) || native_structures.Contains(type_name);
//     }
//
//     public static bool is_refcounted(object type_name) { return engine_classes.Contains(type_name) && engine_classes[type_name]; }
//
//     // 
//     //     Check if a builtin type should be included.
//     //     This removes Variant and POD types from inclusion, and the current type.
//     //     
//     public static bool is_included(string type_name, string current_type)
//     {
//         if (type_name.StartsWith("typedarray::"))
//         {
//             return true;
//         }
//
//         var to_include = is_enum(type_name) ? get_enum_class(type_name) : type_name;
//         if (to_include == current_type || is_pod_type(to_include))
//         {
//             return false;
//         }
//
//         if (to_include == "GlobalConstants" || to_include == "UtilityFunctions")
//         {
//             return true;
//         }
//
//         return is_engine_class(to_include) || is_variant(to_include);
//     }
//
//     public static string CorrectDefaultValue(string value, string type_name)
//     {
//         var value_map = new Dictionary<string, string>
//                         {
//                             { "null", "nullptr" },
//                             { "\"\"", "String()" },
//                             { "&\"\"", "StringName()" },
//                             { "[]", "Array()" },
//                             { "{}", "Dictionary()" },
//                             { "Transform2D(1, 0, 0, 1, 0, 0)", "Transform2D()" },
//                             { "Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0)", "Transform3D()" }
//                         };
//         if (value_map.TryGetValue(value, out var default_value))
//         {
//             return default_value;
//         }
//
//         if (value == "")
//         {
//             return $"{type_name}()";
//         }
//
//         if (value.StartsWith("Array["))
//         {
//             return $"{{}}";
//         }
//
//         return value;
//     }
//
//     public static string correct_typed_array(string type_name)
//     {
//         if (type_name.StartsWith("typedarray::"))
//         {
//             return type_name.Replace("typedarray::", "TypedArray<") + ">";
//         }
//
//         return type_name;
//     }
//
//     public static string correct_type(string type_name, string meta = null)
//     {
//         var type_conversion = new Dictionary<string, string>
//                               {
//                                   { "float", "double" },
//                                   { "int", "int64_t" },
//                                   { "Nil", "Variant" }
//                               };
//         if (meta != null)
//         {
//             if (meta.Contains("int"))
//             {
//                 return $"{meta}_t";
//             }
//             else if (type_conversion.ContainsKey(meta))
//             {
//                 return type_conversion[type_name];
//             }
//             else
//             {
//                 return meta;
//             }
//         }
//
//         if (type_conversion.TryGetValue(type_name, out var type))
//         {
//             return type;
//         }
//
//         if (type_name.StartsWith("typedarray::"))
//         {
//             return type_name.Replace("typedarray::", "TypedArray<") + ">";
//         }
//
//         if (is_enum(type_name))
//         {
//             string base_class;
//             if (is_bitfield(type_name))
//             {
//                 base_class = get_enum_class(type_name);
//                 if (base_class == "GlobalConstants")
//                 {
//                     return $"BitField<{get_enum_name(type_name)}>";
//                 }
//
//                 return $"BitField<{base_class}::{get_enum_name(type_name)}>";
//             }
//             else
//             {
//                 base_class = get_enum_class(type_name);
//                 if (base_class == "GlobalConstants")
//                 {
//                     return $"{get_enum_name(type_name)}";
//                 }
//
//                 return $"{base_class}::{get_enum_name(type_name)}";
//             }
//         }
//
//         if (is_refcounted(type_name))
//         {
//             return $"Ref<{type_name}>";
//         }
//
//         if (type_name == "Object" || is_engine_class(type_name))
//         {
//             return $"{type_name} *";
//         }
//
//         if (type_name.EndsWith("*"))
//         {
//             return $"{type_name[^1].ToString()} *";
//         }
//
//         return type_name;
//     }
//
//     public static string get_gdextension_type(string type_name)
//     {
//         var type_conversion_map = new Dictionary<string, string>
//                                   {
//                                       { "bool", "int8_t" },
//                                       { "uint8_t", "int64_t" },
//                                       { "int8_t", "int64_t" },
//                                       { "uint16_t", "int64_t" },
//                                       { "int16_t", "int64_t" },
//                                       { "uint32_t", "int64_t" },
//                                       { "int32_t", "int64_t" },
//                                       { "int", "int64_t" },
//                                       { "float", "double" }
//                                   };
//         if (type_name.StartsWith("BitField<"))
//         {
//             return "int64_t";
//         }
//
//         if (type_conversion_map.TryGetValue(type_name, out var type))
//         {
//             return type;
//         }
//
//         return type_name;
//     }
//
//     public static string escape_identifier(string id)
//     {
//         var cpp_keywords_map = new Dictionary<string, string>
//                                {
//                                    { "class", "_class" },
//                                    { "char", "_char" },
//                                    { "short", "_short" },
//                                    { "bool", "_bool" },
//                                    { "int", "_int" },
//                                    { "default", "_default" },
//                                    { "case", "_case" },
//                                    { "switch", "_switch" },
//                                    { "export", "_export" },
//                                    { "template", "_template" },
//                                    { "new", "new_" },
//                                    { "operator", "_operator" },
//                                    { "typeof", "type_of" },
//                                    { "typename", "type_name" },
//                                    { "enum", "_enum" }
//                                };
//         if (cpp_keywords_map.TryGetValue(id, out var identifier))
//         {
//             return identifier;
//         }
//
//         return id;
//     }
//
//     public static string get_operator_id_name(string op)
//     {
//         var op_id_map = new Dictionary<string, string>
//                         {
//                             { "==", "equal" },
//                             { "!=", "not_equal" },
//                             { "<", "less" },
//                             { "<=", "less_equal" },
//                             { ">", "greater" },
//                             { ">=", "greater_equal" },
//                             { "+", "add" },
//                             { "-", "subtract" },
//                             { "*", "multiply" },
//                             { "/", "divide" },
//                             { "unary-", "negate" },
//                             { "unary+", "positive" },
//                             { "%", "module" },
//                             { "<<", "shift_left" },
//                             { ">>", "shift_right" },
//                             { "&", "bit_and" },
//                             { "|", "bit_or" },
//                             { "^", "bit_xor" },
//                             { "~", "bit_negate" },
//                             { "and", "and" },
//                             { "or", "or" },
//                             { "xor", "xor" },
//                             { "not", "not" },
//                             { "and", "and" },
//                             { "in", "in" }
//                         };
//         return op_id_map[op];
//     }
//
//     public static string get_default_value_for_type(string type_name)
//     {
//         if (type_name == "int")
//         {
//             return "0";
//         }
//
//         if (type_name == "float")
//         {
//             return "0.0";
//         }
//
//         if (type_name == "bool")
//         {
//             return "false";
//         }
//
//         if (type_name.StartsWith("typedarray::"))
//         {
//             return $"{correct_type(type_name)}()";
//         }
//
//         if (is_enum(type_name))
//         {
//             return $"{correct_type(type_name)}(0)";
//         }
//
//         if (is_variant(type_name))
//         {
//             return $"{type_name}()";
//         }
//
//         if (is_refcounted(type_name))
//         {
//             return $"Ref<{type_name}>()";
//         }
//
//         return "nullptr";
//     }
// }
