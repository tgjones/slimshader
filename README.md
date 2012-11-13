SlimShader
==========

SlimShader is a Direct3D shader bytecode parser for .NET and C++. It is a Portable Class Library (PCL), compatible with
.NET Framework 4.0+ and .NET for Windows Store apps.

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