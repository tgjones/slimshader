#include "PCH.h"
#include "OpcodeType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(OpcodeType value)
{
	switch (value)
	{
	case OpcodeType::Add :
		return "add";
	case OpcodeType::And :
		return "and";
	case OpcodeType::Break :
		return "break";
	case OpcodeType::BreakC :
		return "breakc";
	case OpcodeType::Call :
		return "call";
	case OpcodeType::CallC :
		return "callc";
	case OpcodeType::Case :
		return "case";
	case OpcodeType::Continue :
		return "continue";
	case OpcodeType::ContinueC :
		return "continuec";
	case OpcodeType::Cut :
		return "cut";
	case OpcodeType::Default :
		return "default";
	case OpcodeType::DerivRtx :
		return "deriv_rtx";
	case OpcodeType::DerivRty :
		return "deriv_rty";
	case OpcodeType::Discard :
		return "discard";
	case OpcodeType::Div :
		return "div";
	case OpcodeType::Dp2 :
		return "dp2";
	case OpcodeType::Dp3 :
		return "dp3";
	case OpcodeType::Dp4 :
		return "dp4";
	case OpcodeType::Else :
		return "else";
	case OpcodeType::Emit :
		return "emit";
	case OpcodeType::EmitThenCut :
		return "emitThenCut";
	case OpcodeType::EndIf :
		return "endif";
	case OpcodeType::EndLoop :
		return "endloop";
	case OpcodeType::EndSwitch :
		return "endswitch";
	case OpcodeType::Eq :
		return "eq";
	case OpcodeType::Exp :
		return "exp";
	case OpcodeType::Frc :
		return "frc";
	case OpcodeType::FtoI :
		return "ftoi";
	case OpcodeType::FtoU :
		return "ftou";
	case OpcodeType::Ge :
		return "ge";
	case OpcodeType::IAdd :
		return "iadd";
	case OpcodeType::If :
		return "if";
	case OpcodeType::IEq :
		return "ieq";
	case OpcodeType::IGe :
		return "ige";
	case OpcodeType::ILt :
		return "ilt";
	case OpcodeType::IMad :
		return "imad";
	case OpcodeType::IMax :
		return "imax";
	case OpcodeType::IMin :
		return "imin";
	case OpcodeType::IMul :
		return "imul";
	case OpcodeType::INe :
		return "ine";
	case OpcodeType::INeg :
		return "ineg";
	case OpcodeType::IShl :
		return "ishl";
	case OpcodeType::IShr :
		return "ishr";
	case OpcodeType::IToF :
		return "itof";
	case OpcodeType::Label :
		return "label";
	case OpcodeType::Ld :
		return "ld";
	case OpcodeType::LdMs :
		return "ldms";
	case OpcodeType::Log :
		return "log";
	case OpcodeType::Loop :
		return "loop";
	case OpcodeType::Lt :
		return "lt";
	case OpcodeType::Mad :
		return "mad";
	case OpcodeType::Min :
		return "min";
	case OpcodeType::Max :
		return "max";
	case OpcodeType::Mov :
		return "mov";
	case OpcodeType::MovC :
		return "movc";
	case OpcodeType::Mul :
		return "mul";
	case OpcodeType::Ne :
		return "ne";
	case OpcodeType::Nop :
		return "nop";
	case OpcodeType::Not :
		return "not";
	case OpcodeType::Or :
		return "or";
	case OpcodeType::Resinfo :
		return "resinfo";
	case OpcodeType::Ret :
		return "ret";
	case OpcodeType::RetC :
		return "retc";
	case OpcodeType::RoundNe :
		return "round_ne";
	case OpcodeType::RoundNi :
		return "round_ni";
	case OpcodeType::RoundPi :
		return "round_pi";
	case OpcodeType::RoundZ :
		return "round_z";
	case OpcodeType::Rsq :
		return "rsq";
	case OpcodeType::Sample :
		return "sample";
	case OpcodeType::SampleC :
		return "sample_c";
	case OpcodeType::SampleCLz :
		return "sample_c_lz";
	case OpcodeType::SampleL :
		return "sample_l";
	case OpcodeType::SampleD :
		return "sample_d";
	case OpcodeType::SampleB :
		return "sample_b";
	case OpcodeType::Sqrt :
		return "sqrt";
	case OpcodeType::Switch :
		return "switch";
	case OpcodeType::Sincos :
		return "sincos";
	case OpcodeType::UDiv :
		return "udiv";
	case OpcodeType::ULt :
		return "ult";
	case OpcodeType::UGe :
		return "uge";
	case OpcodeType::UMul :
		return "umul";
	case OpcodeType::UMad :
		return "umad";
	case OpcodeType::UMax :
		return "umax";
	case OpcodeType::UMin :
		return "umin";
	case OpcodeType::UShr :
		return "ushr";
	case OpcodeType::UTof :
		return "utof";
	case OpcodeType::Xor :
		return "xor";
	case OpcodeType::DclResource :
		return "dcl_resource";
	case OpcodeType::DclConstantBuffer :
		return "dcl_constantbuffer";
	case OpcodeType::DclSampler :
		return "dcl_sampler";
	case OpcodeType::DclIndexRange :
		return "dcl_indexrange";
	case OpcodeType::DclGsOutputPrimitiveTopology :
		return "dcl_outputtopology";
	case OpcodeType::DclGsInputPrimitive :
		return "dcl_inputprimitive";
	case OpcodeType::DclMaxOutputVertexCount :
		return "dcl_maxout";
	case OpcodeType::DclInput :
		return "dcl_input";
	case OpcodeType::DclInputSgv :
		return "dcl_input_sgv";
	case OpcodeType::DclInputSiv :
		return "dcl_input_siv";
	case OpcodeType::DclInputPs :
		return "dcl_input_ps";
	case OpcodeType::DclInputPsSgv :
		return "dcl_input_ps_sgv";
	case OpcodeType::DclInputPsSiv :
		return "dcl_input_ps_siv";
	case OpcodeType::DclOutput :
		return "dcl_output";
	case OpcodeType::DclOutputSgv :
		return "dcl_output_sgv";
	case OpcodeType::DclOutputSiv :
		return "dcl_output_siv";
	case OpcodeType::DclTemps :
		return "dcl_temps";
	case OpcodeType::DclIndexableTemp :
		return "dcl_indexableTemp";
	case OpcodeType::DclGlobalFlags :
		return "dcl_globalFlags";
	case OpcodeType::Lod :
		return "lod";
	case OpcodeType::Gather4 :
		return "gather4";
	case OpcodeType::SamplePos :
		return "samplepos";
	case OpcodeType::SampleInfo :
		return "sampleinfo";
	case OpcodeType::HsDecls :
		return "hs_decls";
	case OpcodeType::HsControlPointPhase :
		return "hs_control_point_phase";
	case OpcodeType::HsForkPhase :
		return "hs_fork_phase";
	case OpcodeType::HsJoinPhase :
		return "hs_join_phase";
	case OpcodeType::EmitStream :
		return "emit_stream";
	case OpcodeType::CutStream :
		return "cut_stream";
	case OpcodeType::InterfaceCall :
		return "fcall";
	case OpcodeType::UBfe :
		return "ubfe";
	case OpcodeType::IBfe :
		return "ibfe";
	case OpcodeType::Bfi :
		return "bfi";
	case OpcodeType::SwapC :
		return "swapc";
	case OpcodeType::DclFunctionBody :
		return "dcl_function_body";
	case OpcodeType::DclFunctionTable :
		return "dcl_function_table";
	case OpcodeType::DclInterface :
		return "dcl_interface";
	case OpcodeType::DclInputControlPointCount :
		return "dcl_input_control_point_count";
	case OpcodeType::DclOutputControlPointCount :
		return "dcl_output_control_point_count";
	case OpcodeType::DclTessDomain :
		return "dcl_tessellator_domain";
	case OpcodeType::DclTessPartitioning :
		return "dcl_tessellator_partitioning";
	case OpcodeType::DclTessOutputPrimitive :
		return "dcl_tessellator_output_primitive";
	case OpcodeType::DclHsForkPhaseInstanceCount :
		return "dcl_hs_fork_phase_instance_count";
	case OpcodeType::DclHsJoinPhaseInstanceCount :
		return "dcl_hs_join_phase_instance_count";
	case OpcodeType::DclThreadGroup :
		return "dcl_thread_group";
	case OpcodeType::DclUnorderedAccessViewTyped :
		return "dcl_uav_typed";
	case OpcodeType::DclUnorderedAccessViewRaw :
		return "dcl_uav_raw";
	case OpcodeType::DclUnorderedAccessViewStructured :
		return "dcl_uav_structured";
	case OpcodeType::DclThreadGroupSharedMemoryRaw :
		return "dcl_tgsm_raw";
	case OpcodeType::DclThreadGroupSharedMemoryStructured :
		return "dcl_tgsm_structured";
	case OpcodeType::DclResourceRaw :
		return "dcl_resource_raw";
	case OpcodeType::DclResourceStructured :
		return "dcl_resource_structured";
	case OpcodeType::LdUavTyped :
		return "ld_uav_typed";
	case OpcodeType::StoreUavTyped :
		return "store_uav_typed";
	case OpcodeType::LdRaw :
		return "ld_raw";
	case OpcodeType::StoreRaw :
		return "store_raw";
	case OpcodeType::LdStructured :
		return "ld_structured";
	case OpcodeType::StoreStructured :
		return "store_structured";
	case OpcodeType::Sync :
		return "sync";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}

NumberType SlimShader::GetNumberType(OpcodeType value)
{
	switch (value)
	{
	case OpcodeType::Add :
	case OpcodeType::Div :
	case OpcodeType::Dp2 :
	case OpcodeType::Dp3 :
	case OpcodeType::Dp4 :
	case OpcodeType::Eq :
	case OpcodeType::Exp :
	case OpcodeType::Frc :
	case OpcodeType::Ge :
	case OpcodeType::Log :
	case OpcodeType::Lt :
	case OpcodeType::Mad :
	case OpcodeType::Min :
	case OpcodeType::Max :
	case OpcodeType::Mul :
	case OpcodeType::Ne :
	case OpcodeType::RoundNe :
	case OpcodeType::RoundNi :
	case OpcodeType::RoundPi :
	case OpcodeType::RoundZ :
	case OpcodeType::Rsq :
	case OpcodeType::Sample :
	case OpcodeType::SampleC :
	case OpcodeType::SampleCLz :
	case OpcodeType::SampleL :
	case OpcodeType::SampleD :
	case OpcodeType::SampleB :
	case OpcodeType::Sqrt :
	case OpcodeType::Switch :
	case OpcodeType::Sincos :
	case OpcodeType::F32ToF16 :
	case OpcodeType::F16ToF32 :
		return NumberType::Float;
	case OpcodeType::DAdd :
	case OpcodeType::DMax :
	case OpcodeType::DMin :
	case OpcodeType::DMul :
	case OpcodeType::DEq :
	case OpcodeType::DGe :
	case OpcodeType::DLt :
	case OpcodeType::DNe :
	case OpcodeType::DMov :
	case OpcodeType::DMovC :
		return NumberType::Double;
	case OpcodeType::IAdd :
	case OpcodeType::IEq :
	case OpcodeType::IGe :
	case OpcodeType::ILt :
	case OpcodeType::IMad :
	case OpcodeType::IMax :
	case OpcodeType::IMin :
	case OpcodeType::IMul :
	case OpcodeType::INe :
	case OpcodeType::INeg :
	case OpcodeType::IShl :
	case OpcodeType::IShr :
	case OpcodeType::IBfe :
	case OpcodeType::AtomicIAdd :
	case OpcodeType::AtomicIMax :
	case OpcodeType::AtomicIMin :
	case OpcodeType::ImmAtomicIAdd :
	case OpcodeType::ImmAtomicIMax :
	case OpcodeType::ImmAtomicIMin :
		return NumberType::Int;
	case OpcodeType::And :
	case OpcodeType::Ld :
	case OpcodeType::LdMs :
	case OpcodeType::Or :
	case OpcodeType::UDiv :
	case OpcodeType::ULt :
	case OpcodeType::UGe :
	case OpcodeType::UMul :
	case OpcodeType::UMad :
	case OpcodeType::UMax :
	case OpcodeType::UMin :
	case OpcodeType::UShr :
	case OpcodeType::Xor :
	case OpcodeType::UAddC :
	case OpcodeType::USubB :
	case OpcodeType::CountBits :
	case OpcodeType::FirstBitHi :
	case OpcodeType::FirstBitLo :
	case OpcodeType::FirstBitSHi :
	case OpcodeType::UBfe :
	case OpcodeType::Bfi :
	case OpcodeType::AtomicAnd :
	case OpcodeType::AtomicOr :
	case OpcodeType::AtomicXor :
	case OpcodeType::AtomicUMax :
	case OpcodeType::AtomicUMin :
	case OpcodeType::ImmAtomicAnd :
	case OpcodeType::ImmAtomicOr :
	case OpcodeType::ImmAtomicXor :
	case OpcodeType::ImmAtomicUMax :
	case OpcodeType::ImmAtomicUMin :
		return NumberType::UInt;
	default :
		return NumberType::Unknown;
	}
}

bool SlimShader::IsConditionalInstruction(OpcodeType type)
{
	switch (type)
	{
	case OpcodeType::BreakC :
	case OpcodeType::CallC :
	case OpcodeType::If :
		return true;
	default :
		return false;
	}
}

bool SlimShader::IsDeclaration(OpcodeType type)
{
	return (type >= OpcodeType::DclResource && type <= OpcodeType::DclGlobalFlags)
		|| (type >= OpcodeType::DclStream && type <= OpcodeType::DclResourceStructured)
		|| type == OpcodeType::DclGsInstanceCount;
}