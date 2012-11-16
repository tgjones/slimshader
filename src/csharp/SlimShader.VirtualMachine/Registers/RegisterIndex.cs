using System.Runtime.InteropServices;

namespace SlimShader.VirtualMachine.Registers
{
	[StructLayout(LayoutKind.Explicit)]
	public struct RegisterIndex
	{
		[FieldOffset(0)]
		public ushort Index1D;

		[FieldOffset(0)]
		public ushort Index2D_0;

		[FieldOffset(16)]
		public ushort Index2D_1;

		public RegisterIndex(ushort index1D)
			: this()
		{
			Index1D = index1D;
		}

		public RegisterIndex(ushort index2D_0, ushort index2D_1)
			: this()
		{
			Index2D_0 = index2D_0;
			Index2D_1 = index2D_1;
		}
	}
}