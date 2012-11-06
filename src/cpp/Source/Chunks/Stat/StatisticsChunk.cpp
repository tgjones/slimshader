#include "PCH.h"
#include "StatisticsChunk.h"

using namespace std;
using namespace SlimShader;

shared_ptr<StatisticsChunk> StatisticsChunk::Parse(BytecodeReader& reader, const uint32_t chunkSize)
{
	auto size = chunkSize / sizeof(uint32_t);

	// Unknowns:
	// DefCount
	// MacroInstructionCount

	// PatchConstantParameters
	// cGSInstanceCount
	// cBarrierInstructions
	// cInterlockedInstructions
	// cTextureStoreInstructions

	auto result = shared_ptr<StatisticsChunk>(new StatisticsChunk());

	result->_instructionCount = reader.ReadUInt32();
	result->_tempRegisterCount = reader.ReadUInt32();
	result->_defineCount = reader.ReadUInt32(); // Guessed
	result->_declarationCount = reader.ReadUInt32();
	result->_floatInstructionCount = reader.ReadUInt32();
	result->_intInstructionCount = reader.ReadUInt32();
	result->_uintInstructionCount = reader.ReadUInt32();
	result->_staticFlowControlCount = reader.ReadUInt32();
	result->_dynamicFlowControlCount = reader.ReadUInt32();
	result->_macroInstructionCount = reader.ReadUInt32(); // Guessed
	result->_tempArrayCount = reader.ReadUInt32();
	result->_arrayInstructionCount = reader.ReadUInt32();
	result->_cutInstructionCount = reader.ReadUInt32();
	result->_emitInstructionCount = reader.ReadUInt32();
	result->_textureNormalInstructions = reader.ReadUInt32();
	result->_textureLoadInstructions = reader.ReadUInt32();
	result->_textureCompInstructions = reader.ReadUInt32();
	result->_textureBiasInstructions = reader.ReadUInt32();
	result->_textureGradientInstructions = reader.ReadUInt32();
	result->_movInstructionCount = reader.ReadUInt32();
	result->_movCInstructionCount = reader.ReadUInt32(); // Guessed
	result->_conversionInstructionCount = reader.ReadUInt32();
	result->_unknown4 = reader.ReadUInt32();
	result->_inputPrimitive = static_cast<Primitive>(reader.ReadUInt32());
	result->_geometryShaderOutputTopology = static_cast<PrimitiveTopology>(reader.ReadUInt32());
	result->_geometryShaderMaxOutputVertexCount = reader.ReadUInt32();
	result->_unknown5 = reader.ReadUInt32();
	result->_unknown6 = reader.ReadUInt32();
	result->_unknown7 = reader.ReadUInt32();

	// DX10 stat size
	if (size == 29)
		return result;

	// Unknown.
	assert(reader.ReadUInt32() == 0); // TODO

	result->_controlPoints = reader.ReadUInt32();
	result->_hullShaderOutputPrimitive = static_cast<TessellatorOutputPrimitive>(reader.ReadUInt32());
	result->_hullShaderPartitioning = static_cast<TessellatorPartitioning>(reader.ReadUInt32());
	result->_tessellatorDomain = static_cast<TessellatorDomain>(reader.ReadUInt32());

	result->_barrierInstructions = reader.ReadUInt32(); // Guessed
	result->_interlockedInstructions = reader.ReadUInt32(); // Guessed
	result->_textureStoreInstructions = reader.ReadUInt32(); // Guessed

	// DX11 stat size.
	if (size == 37)
		return result;

	throw runtime_error("Unhandled stat size: " + chunkSize);
}

StatisticsChunk::StatisticsChunk()
	: _controlPoints(0),
	_hullShaderOutputPrimitive(TessellatorOutputPrimitive::Undefined),
	_hullShaderPartitioning(TessellatorPartitioning::Undefined),
	_tessellatorDomain(TessellatorDomain::Undefined),
	_barrierInstructions(0),
	_interlockedInstructions(0),
	_textureStoreInstructions(0)
{

}

uint32_t StatisticsChunk::GetInstructionCount() const { return _instructionCount; }
uint32_t StatisticsChunk::GetTempRegisterCount() const { return _tempRegisterCount; }
uint32_t StatisticsChunk::GetTempArrayCount() const { return _tempArrayCount; }
uint32_t StatisticsChunk::GetDefineCount() const { return _defineCount; }
uint32_t StatisticsChunk::GetDeclarationCount() const { return _declarationCount; }
uint32_t StatisticsChunk::GetTextureNormalInstructions() const { return _textureNormalInstructions; }
uint32_t StatisticsChunk::GetTextureLoadInstructions() const { return _textureLoadInstructions; }
uint32_t StatisticsChunk::GetTextureCompInstructions() const { return _textureCompInstructions; }
uint32_t StatisticsChunk::GetTextureBiasInstructions() const { return _textureBiasInstructions; }
uint32_t StatisticsChunk::GetTextureGradientInstructions() const { return _textureGradientInstructions; }
uint32_t StatisticsChunk::GetFloatInstructionCount() const { return _floatInstructionCount; }
uint32_t StatisticsChunk::GetIntInstructionCount() const { return _intInstructionCount; }
uint32_t StatisticsChunk::GetUIntInstructionCount() const { return _uintInstructionCount; }
uint32_t StatisticsChunk::GetStaticFlowControlCount() const { return _staticFlowControlCount; }
uint32_t StatisticsChunk::GetDynamicFlowControlCount() const { return _dynamicFlowControlCount; }
uint32_t StatisticsChunk::GetMacroInstructionCount() const { return _macroInstructionCount; }
uint32_t StatisticsChunk::GetArrayInstructionCount() const { return _arrayInstructionCount; }
uint32_t StatisticsChunk::GetCutInstructionCount() const { return _cutInstructionCount; }
uint32_t StatisticsChunk::GetEmitInstructionCount() const { return _emitInstructionCount; }
PrimitiveTopology StatisticsChunk::GetGeometryShaderOutputTopology() const { return _geometryShaderOutputTopology; }
uint32_t StatisticsChunk::GetGeometryShaderMaxOutputVertexCount() const { return _geometryShaderMaxOutputVertexCount; }
Primitive StatisticsChunk::GetInputPrimitive() const { return _inputPrimitive; }
uint32_t StatisticsChunk::GetGeometryShaderInstanceCount() const { return _geometryShaderInstanceCount; }
uint32_t StatisticsChunk::GetControlPoints() const { return _controlPoints; }
TessellatorOutputPrimitive StatisticsChunk::GetHullShaderOutputPrimitive() const { return _hullShaderOutputPrimitive; }
TessellatorPartitioning StatisticsChunk::GetHullShaderPartitioning() const { return _hullShaderPartitioning; }
TessellatorDomain StatisticsChunk::GetTessellatorDomain() const { return _tessellatorDomain; }
uint32_t StatisticsChunk::GetBarrierInstructions() const { return _barrierInstructions; }
uint32_t StatisticsChunk::GetInterlockedInstructions() const { return _interlockedInstructions; }
uint32_t StatisticsChunk::GetTextureStoreInstructions() const { return _textureStoreInstructions; }
uint32_t StatisticsChunk::GetMovInstructionCount() const { return _movInstructionCount; }
uint32_t StatisticsChunk::GetMovCInstructionCount() const { return _movCInstructionCount; }
uint32_t StatisticsChunk::GetUnknown3() const { return _unknown3; }
uint32_t StatisticsChunk::GetConversionInstructionCount() const { return _conversionInstructionCount; }
uint32_t StatisticsChunk::GetUnknown4() const { return _unknown4; }
uint32_t StatisticsChunk::GetUnknown5() const { return _unknown5; }
uint32_t StatisticsChunk::GetUnknown6() const { return _unknown6; }
uint32_t StatisticsChunk::GetUnknown7() const { return _unknown7; }

std::ostream& SlimShader::operator<<(std::ostream& out, const StatisticsChunk& chunk)
{
	if (chunk._tessellatorDomain != TessellatorDomain::Undefined)
	{
		out << "// Tessellation Domain   # of control points" << endl;
		out << "// -------------------- --------------------" << endl;
		out << boost::format("// %-20s %20i") 
			% ToStringStat(chunk._tessellatorDomain)
			% chunk._controlPoints
			<< endl;
		out << "//" << endl;
	}
	if (chunk._hullShaderOutputPrimitive != TessellatorOutputPrimitive::Undefined)
	{
		out << "// Tessellation Output Primitive  Partitioning Type " << endl;
		out << "// ------------------------------ ------------------" << endl;
		out << boost::format("// %-30s %-18s")
			% ToStringStat(chunk._hullShaderOutputPrimitive)
			% ToStringStat(chunk._hullShaderPartitioning)
			<< endl;
		out << "//" << endl;
	}
	return out;
}