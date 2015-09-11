SlimShader
==========

SlimShader is a Direct3D shader bytecode parser for .NET and C++. It is a Portable Class Library (PCL), compatible with
.NET Framework 4.0+ and .NET for Windows Store apps. This is the repository for the .NET version; the C++ version can be found at
[tgjones/slimshader-cpp](https://github.com/tgjones/slimshader-cpp).

Usage
-----

```csharp
var fileBytes = File.ReadAllBytes("CompiledShader.o");
var bytecodeContainer = BytecodeContainer.Parse(fileBytes);

Console.WriteLine(bytecodeContainer.InputSignature.Parameters.Count);
Console.WriteLine(bytecodeContainer.Statistics.InstructionCount);
Console.WriteLine(bytecodeContainer.Statistics.StaticFlowControlCount);
Console.WriteLine(bytecodeContainer.Shader.Tokens.Count);
```

Virtual Machine
---------------

SlimShader also includes a virtual machine, which can execute HLSL shaders on the CPU. [See the source code here](https://github.com/tgjones/slimshader/tree/master/src/SlimShader.VirtualMachine). SlimShader.VirtualMachine includes both an interpreter and a just-in-time (JIT) compiler.

HlslUnit
--------

HlslUnit is a .NET library that allows you to unit test your HLSL shaders. It is built on top of SlimShader and SlimShader.VirtualMachine. [I blogged about it here](http://timjones.tw/blog/archive/2014/01/07/introducing-hlslunit-unit-tests-for-your-hlsl-shader-code).

GUI
---

I've written a simple GUI to showcase SlimShader. You can open a compiled (binary) shader file, view the disassembled 
version, and view various properties (from the STAT chunk).

![Screenshot](https://github.com/tgjones/slimshader/raw/master/doc/screenshot.png)

Acknowledgements
----------------

* SlimShader uses several test shaders from the [HLSLCrossCompiler](https://github.com/James-Jones/HLSLCrossCompiler) project,
  by kind permission of [James Jones](https://github.com/James-Jones).
* The [Nuclex Framework](https://devel.nuclex.org/framework), in particular the 
  [HlslShaderReflector class](https://devel.nuclex.org/framework/browser/graphics/Nuclex.Graphics.Native/trunk/Source/Introspection/HlslShaderReflector.cpp),
  was very helpful when figuring out the RDEF, ISGN and OSGN chunks.
* The [Wine](https://github.com/mirrors/wine) project, in particular Wine's [shader reflection code](http://source.winehq.org/source/dlls/d3dcompiler_43/reflection.c),
  had some good tips for decoding the STAT chunk.
* [FXDIS](http://code.google.com/p/fxdis-d3d1x/) was useful to look at when getting started, but the techniques used
  in that project (casting raw bytes to struct types) don't translate well from C++ to C#.
* For the SHDR chunk, I mostly just used D3D11TokenizedProgramFormat.hpp, a header file that comes with the Windows DDK.

License
-------

SlimShader is released under the [MIT License](http://www.opensource.org/licenses/MIT).
