using System;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	public abstract class DeclarationToken : OpcodeToken
	{
		public Operand Operand { get; internal set; }

		public static DeclarationToken Parse(BytecodeReader reader, OpcodeType opcodeType)
		{
			switch (opcodeType)
			{
				case OpcodeType.DclGlobalFlags:
					return GlobalFlagsDeclarationToken.Parse(reader);
				case OpcodeType.DclResource:
					return ResourceDeclarationToken.Parse(reader);
				case OpcodeType.DclSampler:
					return SamplerDeclarationToken.Parse(reader);
				case OpcodeType.DclInput:
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
				case OpcodeType.DclInputPs:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					return InputRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclOutput:
				case OpcodeType.DclOutputSgv:
				case OpcodeType.DclOutputSiv:
					return OutputRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclIndexRange:
					return IndexingRangeDeclarationToken.Parse(reader);
				case OpcodeType.DclTemps:
					return TempRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclIndexableTemp:
					return IndexableTempRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclConstantBuffer:
					return ConstantBufferDeclarationToken.Parse(reader);
				case OpcodeType.DclGsInputPrimitive:
					return GeometryShaderInputPrimitiveDeclarationToken.Parse(reader);
				case OpcodeType.DclGsOutputPrimitiveTopology:
					return GeometryShaderOutputPrimitiveTopologyDeclarationToken.Parse(reader);
				case OpcodeType.DclMaxOutputVertexCount:
					return GeometryShaderMaxOutputVertexCountDeclarationToken.Parse(reader);
				case OpcodeType.DclGsInstanceCount:
					return GeometryShaderInstanceCountDeclarationToken.Parse(reader);
				case OpcodeType.DclInputControlPointCount:
				case OpcodeType.DclOutputControlPointCount:
					return ControlPointCountDeclarationToken.Parse(reader);
				case OpcodeType.DclTessDomain:
					return TessellatorDomainDeclarationToken.Parse(reader);
				case OpcodeType.DclTessPartitioning:
					return TessellatorPartitioningDeclarationToken.Parse(reader);
				case OpcodeType.DclTessOutputPrimitive:
					return TessellatorOutputPrimitiveDeclarationToken.Parse(reader);
				case OpcodeType.DclHsMaxTessFactor:
					return HullShaderMaxTessFactorDeclarationToken.Parse(reader);
				case OpcodeType.DclHsForkPhaseInstanceCount:
					return HullShaderForkPhaseInstanceCountDeclarationToken.Parse(reader);
				case OpcodeType.DclFunctionBody :
					return FunctionBodyDeclarationToken.Parse(reader);
				case OpcodeType.DclFunctionTable :
					return FunctionTableDeclarationToken.Parse(reader);
				case OpcodeType.DclInterface :
					return InterfaceDeclarationToken.Parse(reader);
				case OpcodeType.DclThreadGroup:
					return ThreadGroupDeclarationToken.Parse(reader);
				case OpcodeType.DclUnorderedAccessViewTyped :
					return TypedUnorderedAccessViewDeclarationToken.Parse(reader);
				case OpcodeType.DclUnorderedAccessViewRaw :
					return RawUnorderedAccessViewDeclarationToken.Parse(reader);
				case OpcodeType.DclUnorderedAccessViewStructured :
					return StructuredUnorderedAccessViewDeclarationToken.Parse(reader);
				case OpcodeType.DclThreadGroupSharedMemoryRaw :
					return RawThreadGroupSharedMemoryDeclarationToken.Parse(reader);
				case OpcodeType.DclThreadGroupSharedMemoryStructured :
					return StructuredThreadGroupSharedMemoryDeclarationToken.Parse(reader);
				case OpcodeType.DclResourceRaw :
					return RawShaderResourceViewDeclarationToken.Parse(reader);
				case OpcodeType.DclResourceStructured:
					return StructuredShaderResourceViewDeclarationToken.Parse(reader);
				case OpcodeType.DclStream :
					return StreamDeclarationToken.Parse(reader);
				default:
					throw new ParseException("OpcodeType '" + opcodeType + "' is not supported.");
			}
		}
	}
}