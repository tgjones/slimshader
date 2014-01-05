using System.IO;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using SlimShader.VirtualMachine.Tests.Shaders.VS;
using SlimShader.VirtualMachine.Util;
using Device = SharpDX.Direct3D10.Device;
using DriverType = SharpDX.Direct3D10.DriverType;

namespace SlimShader.VirtualMachine.Tests.Util
{
    internal static class Direct3DUtility
    {
        public static BasicHlsl.VertexShaderOutput ExecuteVertexShader(string compiledShaderFile,
            BasicHlsl.ConstantBufferGlobals globals, VertexPositionNormalTexture vertex)
        {
            var device = new Device(DriverType.Warp);

            var vertexShaderBytes = File.ReadAllBytes(compiledShaderFile);
            var vertexShaderBytecode = new ShaderBytecode(vertexShaderBytes);
            var vertexShader = new VertexShader(device, vertexShaderBytecode);

            var layout = new InputLayout(device,
                ShaderSignature.GetInputSignature(vertexShaderBytecode),
                new[]
                {
                    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 16, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 28, 0)
                });

            var vertices = Buffer.Create(device, BindFlags.VertexBuffer, new[] { vertex });

            var constantBuffer = Buffer.Create(device, ref globals, new BufferDescription
            {
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                Usage = ResourceUsage.Default
            });

            var geometryShader = new GeometryShader(device, vertexShaderBytecode,
               new[]
                {
                    new StreamOutputElement { SemanticName = "SV_POSITION", ComponentCount = 4 },
                    new StreamOutputElement { SemanticName = "COLOR", ComponentCount = 4 },
                    new StreamOutputElement { SemanticName = "TEXCOORD", ComponentCount = 2 }
                },
               BasicHlsl.VertexShaderOutput.SizeInBytes);

            var outputBuffer = Buffer.Create(device, new BasicHlsl.VertexShaderOutput[1],
                new BufferDescription
                {
                    CpuAccessFlags = CpuAccessFlags.None,
                    BindFlags = BindFlags.StreamOutput,
                    Usage = ResourceUsage.Default
                });

            var stagingBuffer = Buffer.Create(device, new BasicHlsl.VertexShaderOutput[1],
                new BufferDescription
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Usage = ResourceUsage.Staging
                });

            device.InputAssembler.InputLayout = layout;
            device.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, VertexPositionNormalTexture.SizeInBytes, 0));
            device.VertexShader.SetConstantBuffer(0, constantBuffer);
            device.VertexShader.Set(vertexShader);
            device.GeometryShader.Set(geometryShader);
            device.StreamOutput.SetTargets(new StreamOutputBufferBinding(outputBuffer, 0));

            device.Draw(1, 0);

            device.CopyResource(outputBuffer, stagingBuffer);
            device.Flush();

            var stream = stagingBuffer.Map(MapMode.Read, SharpDX.Direct3D10.MapFlags.None);
            var bytes = new byte[BasicHlsl.VertexShaderOutput.SizeInBytes];
            stream.Read(bytes, 0, bytes.Length);
            stream.Dispose();

            outputBuffer.Dispose();
            vertices.Dispose();
            layout.Dispose();
            geometryShader.Dispose();
            vertexShader.Dispose();
            vertexShaderBytecode.Dispose();
            device.Dispose();

            return StructUtility.FromBytes<BasicHlsl.VertexShaderOutput>(bytes);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTexture
    {
        public static int SizeInBytes
        {
            get { return 36; }
        }

        public Vector4 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
    }
}