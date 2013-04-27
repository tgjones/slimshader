using System.Collections;

namespace SlimShader.VirtualMachine.Execution
{
	public class DivergenceStackEntry
	{
		public int NextPC { get; set; }
		public BitArray ActiveMask { get; private set; }
		public int ReconvergencePC { get; private set; }

		public DivergenceStackEntry(int nextPC, BitArray activeMask, int reconvergencePC)
		{
			NextPC = nextPC;
			ActiveMask = activeMask;
			ReconvergencePC = reconvergencePC;
		}
	}
}