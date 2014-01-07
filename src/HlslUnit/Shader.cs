using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Xsgn;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Resources;
using SlimShader.VirtualMachine.Util;

namespace HlslUnit
{
    /// <summary>
    /// Encapsules a method that takes texture coordinates as its parameters, and returns a colour.
    /// </summary>
    /// <typeparam name="TColor">Type of the colour to return, i.e. Vector4.</typeparam>
    /// <param name="u">U coordinate.</param>
    /// <param name="v">V coordinate.</param>
    /// <param name="w">W coordinate.</param>
    /// <param name="x">Array index. Only used by TextureCubeArray.</param>
    /// <returns></returns>
    public delegate TColor ResourceCallback<out TColor>(float u, float v, float w, float x);

    /// <summary>
    /// The Shader class executes HLSL shaders on the CPU, entirely in managed code.
    /// It it designed to support unit testing for shaders.
    /// </summary>
	public class Shader
    {
        // We only support executing a single context at a time.
        private const int ContextIndex = 0;

        private readonly List<ConstantBuffer> _constantBuffers;
        private readonly List<ResourceBinding> _resourceBindings;
        private readonly InputSignatureChunk _inputSignature;
	    private readonly int _inputSignatureSize;
        private readonly OutputSignatureChunk _outputSignature;
        private readonly int _outputSignatureSize;
        private readonly VirtualMachine _virtualMachine;

        /// <summary>
        /// Creates a new instance of the Shader class.
        /// </summary>
        /// <param name="shaderBytecode">Byte array containing the compiled shader bytecode.</param>
		public Shader(byte[] shaderBytecode)
		{
            var bytecodeContainer = new BytecodeContainer(shaderBytecode);

            var programType = bytecodeContainer.Shader.Version.ProgramType;
            switch (programType)
            {
                case ProgramType.GeometryShader:
                case ProgramType.HullShader:
                case ProgramType.DomainShader:
                case ProgramType.ComputeShader:
                    throw new NotSupportedException(string.Format(
                        "The '{0}' shader type is not yet supported.", 
                        bytecodeContainer.Shader.Version.ProgramType));
            }

            _constantBuffers = bytecodeContainer.ResourceDefinition.ConstantBuffers;
            _resourceBindings = bytecodeContainer.ResourceDefinition.ResourceBindings;
		    _inputSignature = bytecodeContainer.InputSignature;
		    _inputSignatureSize = _inputSignature.Parameters.Sum(x => x.ByteCount);
            _outputSignature = bytecodeContainer.OutputSignature;
            _outputSignatureSize = _outputSignature.Parameters.Sum(x => x.ByteCount);

            var numContexts = (programType == ProgramType.PixelShader) ? 4 : 1;
            _virtualMachine = new VirtualMachine(bytecodeContainer, numContexts);
		}

        /// <summary>
        /// Sets a constant buffer with the given name and value.
        /// This must be done before executing the shader.
        /// </summary>
		public void SetConstantBuffer<T>(string name, T value)
			where T : struct
		{
            // Validate.
            var constantBufferIndex = _constantBuffers.FindIndex(x => x.Name == name);
			if (constantBufferIndex == -1)
				throw new ArgumentException("Could not find constant buffer named '" + name + "'", "name");

            // Set constant buffer into virtual machine registers.
		    var bytes = StructUtility.ToBytes(value);
            for (var i = 0; i < bytes.Length; i += 16)
                _virtualMachine.SetConstantBufferRegisterValue(constantBufferIndex, i / 16,
                    Number4.FromByteArray(bytes, i));
		}

        /// <summary>
        /// Sets a callback to be used whenever texture data is requested by a shader.
        /// The callback will be passed the u, v and w coordinates.
        /// </summary>
        /// <typeparam name="TColor">Color type, i.e. Vector4.</typeparam>
        /// <param name="name">Name of the resource.</param>
        /// <param name="callback">The callback will be executed once for each texture fetch,
        /// and will be passed the u, v and w coordinates.</param>
        public void SetResource<TColor>(string name, ResourceCallback<TColor> callback)
            where TColor : struct
        {
            // Validate.
            var resourceIndex = _resourceBindings.FindIndex(x => x.Name == name);
            if (resourceIndex == -1)
                throw new ArgumentException("Could not find resource named '" + name + "'", "name");
            if (StructUtility.SizeOf<TColor>() != Number4.SizeInBytes)
                throw new ArgumentException(string.Format("Expected color struct to be {0} bytes, but was {1}'",
                    Number4.SizeInBytes, StructUtility.SizeOf<TColor>()));

            // Set resource into virtual machine. We also set a default sampler state.
            var registerIndex = new RegisterIndex((ushort) resourceIndex);
            _virtualMachine.SetSampler(registerIndex, new SamplerState());
            _virtualMachine.SetTexture(registerIndex,
                new FakeTexture((u, v, w, i) =>
                {
                    var bytes = StructUtility.ToBytes(callback(u, v, w, i));
                    return Number4.FromByteArray(bytes, 0);
                }));
        }

        /// <summary>
        /// Executes the shader. First, the input value is set into the input registers.
        /// Then the shader is executed. Finally, the output value is extracted from
        /// the output registers.
        /// </summary>
        /// <typeparam name="TInput">Input type. This must be a struct whose byte count
        /// exactly matches the shader input.</typeparam>
        /// <typeparam name="TOutput">Output type. This must be a struct whose byte count
        /// exactly matches the shader output.</typeparam>
        /// <param name="input">Input to the shader.</param>
        /// <returns>Shader output.</returns>
		public TOutput Execute<TInput, TOutput>(TInput input)
            where TInput : struct 
			where TOutput : struct
		{
            // Validate.
            if (StructUtility.SizeOf<TInput>() != _inputSignatureSize)
                throw new ArgumentException(string.Format("Expected input struct to be {0} bytes, but was {1}'",
                    _inputSignatureSize, StructUtility.SizeOf<TInput>()));

            // Set input parameters into virtual machine registers. Use knowledge of byte size of each 
            // parameter to extract the right values from the input.
            var inputBytes = StructUtility.ToBytes(input);
		    var inputByteIndex = 0;
		    for (int i = 0; i < _inputSignature.Parameters.Count; i++)
		    {
		        var inputParameter = _inputSignature.Parameters[i];
		        var registerBytes = new byte[Number4.SizeInBytes];
		        Array.Copy(inputBytes, inputByteIndex, registerBytes, 0, inputParameter.ByteCount);
                _virtualMachine.SetInputRegisterValue(ContextIndex, 0, i, Number4.FromByteArray(registerBytes, 0));
		        inputByteIndex += inputParameter.ByteCount;
		    }

		    // Execute shader.
			_virtualMachine.Execute();

            // Get output parameters from virtual machine registers.
			var outputBytes = new byte[_outputSignatureSize];
            var outputByteIndex = 0;
            for (var i = 0; i < _outputSignature.Parameters.Count; i++)
            {
                var outputParameter = _outputSignature.Parameters[i];
                var registerBytes = _virtualMachine.GetOutputRegisterValue(ContextIndex, i).RawBytes;
				Array.Copy(registerBytes, 0, outputBytes, outputByteIndex, outputParameter.ByteCount);
                outputByteIndex += outputParameter.ByteCount;
            }
			return StructUtility.FromBytes<TOutput>(outputBytes);
		}
	}
}