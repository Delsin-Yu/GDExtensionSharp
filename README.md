# GDExtensionSharp (GodotSharp for GDExtension)

## Writing GDExtension with C\#

The goal for this repo is to allow developers to write `GDExtension` with `C#/.Net 8`.
By leveraging the power of `NativeAOT`, the C# project can be compiled into an `unmanaged library` and expose `C-style function entries`,  allowing the GDExtension system access.

## Current Approach

## Current Structure of the Project

### DodgeTheCreeps.csproj

The planned example project aims to replicate the well-known [Dodge the Creeps](https://docs.godotengine.org/en/latest/getting_started/first_2d_game/index.html) Godot demo and, hopefully, be 100% source code compatible with the original version.

### GDExtensionSharp.csproj

This project contains all the `Interop Code` and the source code from `GodotSharp`. Planned to ship as a Nuget package for .Net developer consumption.

### GDExtensionSharp.SourceGenerator.Api.csproj

This source generator project is responsible for converting `extension_api.json` to C# source code into `GDExtensionSharp.csproj`.

### GDExtensionSharp.SourceGenerator.Header.csproj

This source generator project is responsible for converting `gdextension_interface.h` to C# source code into `GDExtensionSharp.csproj`.

### GDExtensionSharp.SourceGenerator.NativeBinding.csproj

This source generator project is responsible for creating bridge C# source code between the original `Godot.NativeInterop.NativeFunc` and the `GDExtension interfaces` inside `GDExtensionSharp.GDExtensionSharpBinding.MethodTable` into `GDExtensionSharp.csproj`.

### GDExtensionSharp.SourceGenerators.csproj

This source generator project is planned to ship with `GDExtensionSharp.csproj` for generating `GDExtension Initialization Entry` and `ClassDB Registration Code`.
