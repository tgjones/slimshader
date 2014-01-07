using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.Chunks.Xsgn
{
	/// <summary>
	/// Describes a shader signature.
	/// Based on D3D11_SIGNATURE_PARAMETER_DESC.
	/// </summary>
	public class SignatureParameterDescription
	{
		/// <summary>
		/// A per-parameter string that identifies how the data will be used.
		/// </summary>
		public string SemanticName { get; private set; }

		/// <summary>
		/// Semantic index that modifies the semantic. Used to differentiate different parameters that use the same 
		/// semantic.
		/// </summary>
		public uint SemanticIndex { get; private set; }

		/// <summary>
		/// The register that will contain this variable's data.
		/// </summary>
		public uint Register { get; private set; }

		/// <summary>
		/// A <see cref="SystemValueName"/>-typed value that identifies a predefined string that determines the 
		/// functionality of certain pipeline stages.
		/// </summary>
		public Name SystemValueType { get; private set; }

		/// <summary>
		/// A <see cref="RegisterComponentType"/>-typed value that identifies the per-component-data type that is 
		/// stored in a register. Each register can store up to four-components of data.
		/// </summary>
		public RegisterComponentType ComponentType { get; private set; }

		/// <summary>
		/// Mask which indicates which components of a register are used.
		/// </summary>
		public ComponentMask Mask { get; private set; }

		/// <summary>
		/// Mask which indicates whether a given component is never written (if the signature is an output signature) 
		/// or always read (if the signature is an input signature).
		/// </summary>
		public ComponentMask ReadWriteMask { get; private set; }

		/// <summary>
		/// Indicates which stream the geometry shader is using for the signature parameter.
		/// </summary>
		public uint Stream { get; private set; }

		///// <summary>
		///// A <see cref="MinPrecision"/>-typed value that indicates the minimum desired interpolation precision.
		///// </summary>
		//public MinPrecision MinPrecision { get; private set; }

        /// <summary>
        /// Gets the total number of bytes used by this parameter.
        /// </summary>
	    public int ByteCount
	    {
	        get
	        {
	            var result = 0;
	            if (Mask.HasFlag(ComponentMask.X))
	                result++;
                if (Mask.HasFlag(ComponentMask.Y))
                    result++;
                if (Mask.HasFlag(ComponentMask.Z))
                    result++;
                if (Mask.HasFlag(ComponentMask.W))
                    result++;
	            return result * sizeof(float);
	        }
	    }

		public SignatureParameterDescription(string semanticName, uint semanticIndex,
			Name systemValueType, RegisterComponentType componentType, uint register,
			ComponentMask mask, ComponentMask readWriteMask)
		{
			SemanticName = semanticName;
			SemanticIndex = semanticIndex;
			Register = register;
			SystemValueType = systemValueType;
			ComponentType = componentType;
			Mask = mask;
			ReadWriteMask = readWriteMask;
		}

		public SignatureParameterDescription()
		{
			
		}

		public static SignatureParameterDescription Parse(BytecodeReader reader, BytecodeReader parameterReader,
			ChunkType chunkType, SignatureElementSize size, ProgramType programType)
		{
			uint stream = 0;
			if (size == SignatureElementSize._7)
				stream = parameterReader.ReadUInt32();

			uint nameOffset = parameterReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			var result = new SignatureParameterDescription
			{
				SemanticName = nameReader.ReadString(),
				SemanticIndex = parameterReader.ReadUInt32(),
				SystemValueType = (Name) parameterReader.ReadUInt32(),
				ComponentType = (RegisterComponentType) parameterReader.ReadUInt32(),
				Register = parameterReader.ReadUInt32(),
				Stream = stream,
				//MinPrecision = (MinPrecision) parameterReader.ReadByte() TODO
			};

			uint mask = parameterReader.ReadUInt32();
			result.Mask = mask.DecodeValue<ComponentMask>(0, 7);
			result.ReadWriteMask = mask.DecodeValue<ComponentMask>(8, 15);

			// This is my guesswork, but it works so far...
			if (chunkType == ChunkType.Osg5 || chunkType == ChunkType.Osgn
				|| (chunkType == ChunkType.Pcsg && programType == ProgramType.HullShader))
				result.ReadWriteMask = (ComponentMask) (ComponentMask.All - result.ReadWriteMask);

			// Vertex and pixel shaders need special handling for SystemValueType in the output signature.
			if ((programType == ProgramType.PixelShader || programType == ProgramType.VertexShader)
				&& (chunkType == ChunkType.Osg5 || chunkType == ChunkType.Osgn))
			{
				if (result.Register == 0xffffffff)
					switch (result.SemanticName.ToUpper())
					{
						case "SV_DEPTH":
							result.SystemValueType = Name.Depth;
							break;
						case "SV_COVERAGE":
							result.SystemValueType = Name.Coverage;
							break;
						case "SV_DEPTHGREATEREQUAL":
							result.SystemValueType = Name.DepthGreaterEqual;
							break;
						case "SV_DEPTHLESSEQUAL":
							result.SystemValueType = Name.DepthLessEqual;
							break;
					}
				else if (programType == ProgramType.PixelShader)
					result.SystemValueType = Name.Target;
			}

			return result;
		}

		public override string ToString()
		{
			// For example:
			// Name                 Index   Mask Register SysValue  Format   Used
			// TEXCOORD                 0   xyzw        0     NONE   float   xyzw
			// SV_DepthGreaterEqual     0    N/A oDepthGE  DEPTHGE   float    YES
			if (SystemValueType.RequiresMask())
			{
				return string.Format("{0,-20} {1,5}   {2,-4} {3,8} {4,8} {5,7}   {6,-4}", SemanticName, SemanticIndex,
					Mask.GetDescription(),
					Register, SystemValueType.GetDescription(),
					ComponentType.GetDescription(), ReadWriteMask.GetDescription());
			}
			return string.Format("{0,-20} {1,5}   {2,4} {3,8} {4,8} {5,7}   {6,4}", SemanticName, SemanticIndex,
				"N/A", SystemValueType.GetRegisterName(), SystemValueType.GetDescription(),
				ComponentType.GetDescription(), "YES");
		}
	}
}