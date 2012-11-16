using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Xsgn;

namespace SlimShader.Tests
{
	[TestFixture]
	public class BytecodeContainerTests
	{
		private static IEnumerable<string> TestShaders
		{
			get
			{
				const string shadersDirectory = @"..\..\..\..\..\shaders";
				return Directory.GetFiles(shadersDirectory, "*.o", SearchOption.AllDirectories)
					.Select(x => Path.GetDirectoryName(x) + @"\" + Path.GetFileNameWithoutExtension(x));
			}
		}

		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void ParsedShaderOutputMatchesFxcOutput(string file)
		{
			// Arrange.
			var asmFileText = string.Join(Environment.NewLine,
				File.ReadAllLines(file + ".asm").Select(x => x.Trim()));

			// Act.
			var container = BytecodeContainer.Parse(File.ReadAllBytes(file + ".o"));
			var decompiledAsmText = string.Join(Environment.NewLine, container.ToString()
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Select(x => x.Trim()));

			// Assert.
			Assert.That(decompiledAsmText, Is.EqualTo(asmFileText));
		}

		/// <summary>
		/// Compare with actual Direct3D reflected objects.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void ShaderReflectionMatchesDirect3DReflection(string file)
		{
			// Arrange.
			var binaryFileBytes = File.ReadAllBytes(file + ".o");
			using (var shaderBytecode = ShaderBytecode.FromStream(new MemoryStream(binaryFileBytes)))
			using (var shaderReflection = new ShaderReflection(shaderBytecode))
			{
				var desc = shaderReflection.Description;

				// Act.
				var container = BytecodeContainer.Parse(binaryFileBytes);

				// Assert.
				Assert.AreEqual(shaderReflection.BitwiseInstructionCount, 0); // TODO
				Assert.AreEqual(shaderReflection.ConditionalMoveInstructionCount, container.Statistics.MovCInstructionCount);
				Assert.AreEqual(shaderReflection.ConversionInstructionCount, container.Statistics.ConversionInstructionCount);
				Assert.AreEqual((int) shaderReflection.GeometryShaderSInputPrimitive, (int) container.Statistics.InputPrimitive);
				Assert.AreEqual(shaderReflection.InterfaceSlotCount, container.ResourceDefinition.InterfaceSlotCount);
				Assert.AreEqual((bool) shaderReflection.IsSampleFrequencyShader, container.Statistics.IsSampleFrequencyShader);
				Assert.AreEqual(shaderReflection.MoveInstructionCount, container.Statistics.MovInstructionCount);
				//Assert.AreEqual(shaderReflection.RequiresFlags, 0); // TODO

				int expectedSizeX, expectedSizeY, expectedSizeZ;
				uint actualSizeX, actualSizeY, actualSizeZ;
				shaderReflection.GetThreadGroupSize(out expectedSizeX, out expectedSizeY, out expectedSizeZ);
				container.Shader.GetThreadGroupSize(out actualSizeX, out actualSizeY, out actualSizeZ);
				Assert.AreEqual(expectedSizeX, actualSizeX);
				Assert.AreEqual(expectedSizeY, actualSizeY);
				Assert.AreEqual(expectedSizeZ, actualSizeZ);

				//Assert.AreEqual((int) shaderReflection.MinFeatureLevel, 0); // TODO

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
			}
		}

		private static void CompareConstantBuffer(SharpDX.D3DCompiler.ConstantBuffer expected, Chunks.Rdef.ConstantBuffer actual)
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
			Assert.AreEqual(expected.Description.SamplerSize, actual.SamplerSize);
			Assert.AreEqual(expected.Description.Size, actual.Size);
			Assert.AreEqual(expected.Description.StartOffset, actual.StartOffset);
			if (expected.Description.StartSampler != -1 && actual.StartSampler != 0)
				Assert.AreEqual(expected.Description.StartSampler, actual.StartSampler);
			if (expected.Description.StartTexture != -1 && actual.StartTexture != 0)
			Assert.AreEqual(expected.Description.StartTexture, actual.StartTexture);
			Assert.AreEqual(expected.Description.TextureSize, actual.TextureSize);

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
			//Assert.AreEqual(expected.NumInterfaces, 0); // TODO
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