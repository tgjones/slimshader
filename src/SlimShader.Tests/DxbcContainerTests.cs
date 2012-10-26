using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using SlimShader.Chunks;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks.Xsgn;
using SlimShader.Util;
using ConstantBuffer = SharpDX.D3DCompiler.ConstantBuffer;
using ResourceDimension = SlimShader.Chunks.Shex.ResourceDimension;
using ResourceReturnType = SlimShader.Chunks.Common.ResourceReturnType;

namespace SlimShader.Tests
{
	/// <summary>
	/// Using documentation from d3d11tokenizedprogramformat.hpp.
	/// </summary>
	[TestFixture]
	public class DxbcContainerTests
	{
		[Test]
		public void CanLoadShaderBytecode()
		{
			// Arrange.
			var fileBytes = File.ReadAllBytes("Shaders/FxDis/test.o");

			// Act.
			var container = DxbcContainer.Parse(new BytecodeReader(fileBytes, 0, fileBytes.Length));

			// Assert.
			Assert.That(container.Header.FourCc, Is.EqualTo(1128421444));
			Assert.That(container.Header.UniqueKey[0], Is.EqualTo(2210296095));
			Assert.That(container.Header.UniqueKey[1], Is.EqualTo(678178285));
			Assert.That(container.Header.UniqueKey[2], Is.EqualTo(4191542541));
			Assert.That(container.Header.UniqueKey[3], Is.EqualTo(1829059345));
			Assert.That(container.Header.One, Is.EqualTo(1));
			Assert.That(container.Header.TotalSize, Is.EqualTo(5864));
			Assert.That(container.Header.ChunkCount, Is.EqualTo(5));

			Assert.That(container.Chunks.Count, Is.EqualTo(5));
			Assert.That(container.Chunks[0].FourCc, Is.EqualTo(1178944594));
			Assert.That(container.Chunks[0].ChunkSize, Is.EqualTo(544));
			Assert.That(container.Chunks[1].FourCc, Is.EqualTo(1313297225));
			Assert.That(container.Chunks[1].ChunkSize, Is.EqualTo(104));
			Assert.That(container.Chunks[2].FourCc, Is.EqualTo(1313297231));
			Assert.That(container.Chunks[2].ChunkSize, Is.EqualTo(44));
			Assert.That(container.Chunks[3].FourCc, Is.EqualTo(1380206675));
			Assert.That(container.Chunks[3].ChunkSize, Is.EqualTo(4964));
			Assert.That(container.Chunks[3].ChunkType, Is.EqualTo(ChunkType.Shdr));
			Assert.That(container.Chunks[4].FourCc, Is.EqualTo(1413567571));
			Assert.That(container.Chunks[4].ChunkSize, Is.EqualTo(116));

			var shaderProgram = (ShaderProgramChunk) container.Chunks[3];
			Assert.That(shaderProgram.Version.MajorVersion, Is.EqualTo(4));
			Assert.That(shaderProgram.Version.MinorVersion, Is.EqualTo(0));
			Assert.That(shaderProgram.Version.ProgramType, Is.EqualTo(ProgramType.PixelShader));
			Assert.That(shaderProgram.Length, Is.EqualTo(1241));

			Assert.That(shaderProgram.Tokens, Has.Count.EqualTo(213));

			Assert.That(shaderProgram.Tokens[3], Is.InstanceOf<ResourceDeclarationToken>());
			var resourceToken1 = (ResourceDeclarationToken) shaderProgram.Tokens[3];
			Assert.That(resourceToken1.Header.IsExtended, Is.False);
			Assert.That(resourceToken1.Header.Length, Is.EqualTo(4));
			Assert.That(resourceToken1.Header.OpcodeType, Is.EqualTo(OpcodeType.DclResource));
			Assert.That(resourceToken1.ResourceDimension, Is.EqualTo(ResourceDimension.Texture2D));
			Assert.That(resourceToken1.ReturnType.X, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.ReturnType.Y, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.ReturnType.Z, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.ReturnType.W, Is.EqualTo(ResourceReturnType.Float));
			Assert.That(resourceToken1.SampleCount, Is.EqualTo(0));
			Assert.That(resourceToken1.Operand.OperandType, Is.EqualTo(OperandType.Resource));
			Assert.That(resourceToken1.Operand.Modifier, Is.EqualTo(OperandModifier.None));
			Assert.That(resourceToken1.Operand.IsExtended, Is.False);
			Assert.That(resourceToken1.Operand.ComponentMask, Is.EqualTo(ComponentMask.None));
			Assert.That(resourceToken1.Operand.IndexDimension, Is.EqualTo(OperandIndexDimension._1D));
			Assert.That(resourceToken1.Operand.IndexRepresentations[0], Is.EqualTo(OperandIndexRepresentation.Immediate32));
			Assert.That(resourceToken1.Operand.Indices[0].Value, Is.EqualTo(0));
		}

		[TestCase("Shaders/FxDis/test")]
		[TestCase("Shaders/HlslCrossCompiler/ds5/basic")]
		[TestCase("Shaders/HlslCrossCompiler/gs4/CubeMap_Inst")]
		[TestCase("Shaders/HlslCrossCompiler/hs5/basic")]
		[TestCase("Shaders/HlslCrossCompiler/ps4/fxaa")]
		[TestCase("Shaders/HlslCrossCompiler/ps4/primID")]
		[TestCase("Shaders/HlslCrossCompiler/ps5/conservative_depth_ge")]
		[TestCase("Shaders/HlslCrossCompiler/ps5/interfaces")]
		[TestCase("Shaders/HlslCrossCompiler/ps5/sample")]
		[TestCase("Shaders/HlslCrossCompiler/vs4/mov")]
		[TestCase("Shaders/HlslCrossCompiler/vs4/multiple_const_buffers")]
		[TestCase("Shaders/HlslCrossCompiler/vs4/switch")]
		[TestCase("Shaders/HlslCrossCompiler/vs5/any")]
		[TestCase("Shaders/HlslCrossCompiler/vs5/const_temp")]
		[TestCase("Shaders/HlslCrossCompiler/vs5/mad_imm")]
		[TestCase("Shaders/HlslCrossCompiler/vs5/mov")]
		[TestCase("Shaders/HlslCrossCompiler/vs5/sincos")]
		[TestCase("Shaders/Sdk/Direct3D11/BasicCompute11/BasicCompute11")]
		[TestCase("Shaders/Sdk/Direct3D11/DynamicShaderLinkage11/DynamicShaderLinkage11_PS")]
		[TestCase("Shaders/Sdk/Direct3D11/BC6HBC7EncoderDecoder11/BC6HDecode")]
		[TestCase("Shaders/Sdk/Direct3D11/BC6HBC7EncoderDecoder11/BC7Decode")]
		[TestCase("Shaders/Sdk/Direct3D11/BC6HBC7EncoderDecoder11/BC7Encode")]
		[TestCase("Shaders/Sdk/Direct3D11/AdaptiveTessellationCS40/TessellatorCS40_EdgeFactorCS")]
		[TestCase("Shaders/Sdk/Direct3D11/AdaptiveTessellationCS40/TessellatorCS40_NumVerticesIndicesCS")]
		[TestCase("Shaders/Sdk/Direct3D11/AdaptiveTessellationCS40/TessellatorCS40_ScatterIDCS")]
		[TestCase("Shaders/Sdk/Direct3D11/AdaptiveTessellationCS40/TessellatorCS40_TessellateIndicesCS")]
		[TestCase("Shaders/Sdk/Direct3D11/AdaptiveTessellationCS40/TessellatorCS40_TessellateVerticesCS")]
		public void CanParseShader(string file)
		{
			string binaryFile = file + ".o";
			var binaryFileBytes = File.ReadAllBytes(binaryFile);
			var container = DxbcContainer.Parse(new BytecodeReader(binaryFileBytes, 0, binaryFileBytes.Length));

			CompareAssemblyOutput(file, container);
			CompareDirect3DReflection(binaryFileBytes, container);
		}

		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="container"></param>
		private static void CompareAssemblyOutput(string file, DxbcContainer container)
		{
			// Arrange.
			string asmFile = file + ".asm";
			var asmFileText = GetAsmText(asmFile);

			// Act.

			// Assert.
			// Ignore first 5 lines - they contain the compiler-specific headers.
			var decompiledAsmText = string.Join(Environment.NewLine, container.ToString()
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Skip(5).Select(x => x.Trim()));
			Assert.That(decompiledAsmText, Is.EqualTo(asmFileText));
		}

		private static string GetAsmText(string asmFile)
		{
			var asmFileLines = File.ReadAllLines(asmFile);

			/* The first 5 or 6 lines contain something like:
			
			//
			// Generated by Microsoft (R) HLSL Shader Compiler 9.29.952.3111
			//
			//
			//   fxc /T vs_4_0 /Fo multiple_const_buffers.o /Fc multiple_const_buffers.asm
			//    multiple_const_buffers
			*/

			// We want to skip all that, because we can't accurately recreate the fxc command-line, and so we
			// aren't able to do a string comparison on these lines.
			int skip = 5;
			while (asmFileLines[skip] != "//")
				skip++;
			return string.Join(Environment.NewLine, asmFileLines.Skip(skip).Select(x => x.Trim()));
		}

		/// <summary>
		/// Compare with actual Direct3D reflected objects.
		/// </summary>
		/// <param name="binaryFileBytes"> </param>
		/// <param name="container"></param>
		private static void CompareDirect3DReflection(byte[] binaryFileBytes, DxbcContainer container)
		{
			var shaderBytecode = ShaderBytecode.FromStream(new MemoryStream(binaryFileBytes));
			var shaderReflection = new ShaderReflection(shaderBytecode);

			var desc = shaderReflection.Description;

			//Assert.AreEqual(shaderReflection.BitwiseInstructionCount, 2); // TODO
			Assert.AreEqual(shaderReflection.ConditionalMoveInstructionCount, container.Statistics.MovCInstructionCount);
			Assert.AreEqual(shaderReflection.ConversionInstructionCount, container.Statistics.ConversionInstructionCount);
			//Assert.AreEqual(shaderReflection.GeometryShaderSInputPrimitive, actual.g); // TODO
			Assert.AreEqual(shaderReflection.InterfaceSlotCount, (container.Interfaces != null)
				? container.Interfaces.InterfaceSlots.Count
				: 0);
			Assert.AreEqual(shaderReflection.IsSampleFrequencyShader, false); // TODO
			Assert.AreEqual(shaderReflection.MoveInstructionCount, container.Statistics.MovInstructionCount);

			Assert.AreEqual(desc.ArrayInstructionCount, container.Statistics.ArrayInstructionCount);
			Assert.AreEqual(desc.BarrierInstructions, container.Statistics.BarrierInstructions);
			Assert.AreEqual(desc.BoundResources, container.ResourceDefinition.ResourceBindings.Count);
			Assert.AreEqual(desc.ConstantBuffers, container.ResourceDefinition.ConstantBuffers.Count);
			Assert.AreEqual(desc.ControlPoints, container.Statistics.ControlPoints);
			Assert.AreEqual(desc.Creator, container.ResourceDefinition.Creator);
			Assert.AreEqual(desc.CutInstructionCount, container.Statistics.CutInstructionCount);
			Assert.AreEqual(desc.DeclarationCount, container.Statistics.DeclarationCount);
			Assert.AreEqual(desc.DefineCount, container.Statistics.DefineCount);
			Assert.AreEqual(desc.DynamicFlowControlCount, container.Statistics.DynamicFlowControlCount);
			Assert.AreEqual(desc.EmitInstructionCount, container.Statistics.EmitInstructionCount);
			Assert.AreEqual((int) desc.Flags, (int) container.ResourceDefinition.Flags);
			Assert.AreEqual(desc.FloatInstructionCount, container.Statistics.FloatInstructionCount);
			Assert.AreEqual(desc.GeometryShaderInstanceCount, container.Statistics.GeometryShaderInstanceCount);
			Assert.AreEqual(desc.GeometryShaderMaxOutputVertexCount, container.Statistics.GeometryShaderMaxOutputVertexCount);
			Assert.AreEqual((int) desc.GeometryShaderOutputTopology, (int) container.Statistics.GeometryShaderOutputTopology);
			Assert.AreEqual((int) desc.HullShaderOutputPrimitive, (int) container.Statistics.HullShaderOutputPrimitive);
			Assert.AreEqual((int) desc.HullShaderPartitioning, (int) container.Statistics.HullShaderPartitioning);
			Assert.AreEqual(desc.InputParameters, container.InputSignature.Parameters.Count);
			Assert.AreEqual((int) desc.InputPrimitive, (int) container.Statistics.InputPrimitive);
			Assert.AreEqual(desc.InstructionCount, container.Statistics.InstructionCount);
			Assert.AreEqual(desc.InterlockedInstructions, container.Statistics.InterlockedInstructions);
			Assert.AreEqual(desc.IntInstructionCount, container.Statistics.IntInstructionCount);
			Assert.AreEqual(desc.MacroInstructionCount, container.Statistics.MacroInstructionCount);
			Assert.AreEqual(desc.OutputParameters, container.OutputSignature.Parameters.Count);
			Assert.AreEqual(desc.PatchConstantParameters, (container.PatchConstantSignature != null)
				? container.PatchConstantSignature.Parameters.Count
				: 0);
			Assert.AreEqual(desc.StaticFlowControlCount, container.Statistics.StaticFlowControlCount);
			Assert.AreEqual(desc.TempArrayCount, container.Statistics.TempArrayCount);
			Assert.AreEqual(desc.TempRegisterCount, container.Statistics.TempRegisterCount);
			Assert.AreEqual((int) desc.TessellatorDomain, (int) container.Statistics.TessellatorDomain);
			Assert.AreEqual(desc.TextureBiasInstructions, container.Statistics.TextureBiasInstructions);
			Assert.AreEqual(desc.TextureCompInstructions, container.Statistics.TextureCompInstructions);
			Assert.AreEqual(desc.TextureGradientInstructions, container.Statistics.TextureGradientInstructions);
			Assert.AreEqual(desc.TextureLoadInstructions, container.Statistics.TextureLoadInstructions);
			Assert.AreEqual(desc.TextureNormalInstructions, container.Statistics.TextureNormalInstructions);
			Assert.AreEqual(desc.TextureStoreInstructions, container.Statistics.TextureStoreInstructions);
			Assert.AreEqual(desc.UintInstructionCount, container.Statistics.UIntInstructionCount);
			//Assert.AreEqual(desc.Version, container.ResourceDefinition.Target); // TODO

			for (int i = 0; i < shaderReflection.Description.ConstantBuffers; i++)
				CompareConstantBuffer(shaderReflection.GetConstantBuffer(i),
					container.ResourceDefinition.ConstantBuffers[i]);

			for (int i = 0; i < shaderReflection.Description.BoundResources; i++)
				CompareResourceBinding(shaderReflection.GetResourceBindingDescription(i),
					container.ResourceDefinition.ResourceBindings[i]);

			for (int i = 0; i < shaderReflection.Description.InputParameters; i++)
				CompareParameter(shaderReflection.GetInputParameterDescription(i),
					container.InputSignature.Parameters[i]);

			for (int i = 0; i < shaderReflection.Description.OutputParameters; i++)
				CompareParameter(shaderReflection.GetOutputParameterDescription(i),
					container.OutputSignature.Parameters[i]);

			for (int i = 0; i < shaderReflection.Description.PatchConstantParameters; i++)
				CompareParameter(shaderReflection.GetPatchConstantParameterDescription(i),
					container.PatchConstantSignature.Parameters[i]);

			//shaderReflection.GetThreadGroupSize(); // TODO
		}

		private static void CompareConstantBuffer(ConstantBuffer expected, Chunks.Rdef.ConstantBuffer actual)
		{
			Assert.AreEqual((int) expected.Description.Flags, (int) actual.Flags);
			Assert.AreEqual(expected.Description.Name, actual.Name);
			Assert.AreEqual(expected.Description.Size, actual.Size);
			Assert.AreEqual((int) expected.Description.Type, (int) actual.BufferType);
			Assert.AreEqual(expected.Description.VariableCount, actual.Variables.Count);

			for (int i = 0; i < expected.Description.VariableCount; i++)
				CompareConstantBufferVariable(expected.GetVariable(i), actual.Variables[i]);
		}

		private static void CompareConstantBufferVariable(ShaderReflectionVariable expected,
			ShaderVariable actual)
		{
			//Assert.AreEqual(expected.Description.DefaultValue, actual.DefaultValue); // TODO
			Assert.AreEqual((int) expected.Description.Flags, (int) actual.Flags);
			Assert.AreEqual(expected.Description.Name, actual.Name);
			//Assert.AreEqual(expected.Description.SamplerSize, actual.Size); // TODO
			Assert.AreEqual(expected.Description.Size, actual.Size);
			Assert.AreEqual(expected.Description.StartOffset, actual.StartOffset);
			//Assert.AreEqual(expected.Description.StartSampler, actual.Size); // TODO
			//Assert.AreEqual(expected.Description.StartTexture, actual.Size); // TODO
			//Assert.AreEqual(expected.Description.TextureSize, actual.Size); // TODO

			CompareConstantBufferVariableType(expected.GetVariableType(), actual.ShaderType);
		}

		private static void CompareConstantBufferVariableType(ShaderReflectionType expected,
			ShaderType actual)
		{
			//Assert.AreEqual(expected.BaseClass, actual.BaseTypeName); // TODO
			Assert.AreEqual((int) expected.Description.Class, (int) actual.VariableClass);
			Assert.AreEqual(expected.Description.ColumnCount, actual.Columns);
			Assert.AreEqual(expected.Description.ElementCount, actual.ElementCount);
			Assert.AreEqual(expected.Description.MemberCount, actual.Members.Count);
			Assert.AreEqual(expected.Description.Name, actual.BaseTypeName);
			//Assert.AreEqual(expected.Description.Offset, actual.off); // TODO
			Assert.AreEqual(expected.Description.RowCount, actual.Rows);
			Assert.AreEqual((int) expected.Description.Type, (int) actual.VariableType);
			//Assert.AreEqual(expected.NumInterfaces, actual.Rows); // TODO
		}

		private static void CompareParameter(ShaderParameterDescription expected,
			SignatureParameterDescription actual)
		{
			Assert.AreEqual((int) expected.ComponentType, (int) actual.ComponentType);
			//Assert.AreEqual((int) expected.ReadWriteMask, (int) actual.ReadWriteMask); // TODO: Bug in SharpDX?
			if (expected.Register != -1 || actual.Register != uint.MaxValue)
				Assert.AreEqual(expected.Register, actual.Register);
			Assert.AreEqual(expected.SemanticIndex, actual.SemanticIndex);
			Assert.AreEqual(expected.SemanticName, actual.SemanticName);
			Assert.AreEqual(expected.Stream, actual.Stream);
			Assert.AreEqual((int) expected.SystemValueType, (int) actual.SystemValueType);
			//Assert.AreEqual((int) expected.UsageMask, (int) actual.Mask); // TODO: Bug in SharpDX?
		}

		private static void CompareResourceBinding(InputBindingDescription expected,
			ResourceBinding actual)
		{
			Assert.AreEqual(expected.BindCount, actual.BindCount);
			Assert.AreEqual(expected.BindPoint, actual.BindPoint);
			Assert.AreEqual((int) expected.Dimension, (int) actual.Dimension);
			Assert.AreEqual((int) expected.Flags, (int) actual.Flags);
			Assert.AreEqual(expected.Name, actual.Name);
			if (expected.NumSamples != -1 || actual.NumSamples != uint.MaxValue)
				Assert.AreEqual(expected.NumSamples, actual.NumSamples);
			Assert.AreEqual((int) expected.ReturnType, (int) actual.ReturnType);
			Assert.AreEqual((int) expected.Type, (int) actual.Type);
		}
	}
}