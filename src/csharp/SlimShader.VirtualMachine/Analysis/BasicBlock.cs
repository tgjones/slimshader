using System.Collections.Generic;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Analysis
{
	public class BasicBlock
	{
		public int Position { get; private set; }
		public IList<InstructionBase> Instructions { get; private set; }
		public List<BasicBlock> Successors { get; private set; }
		//public BasicBlock ReconvergencePoint { get; set; }

		public BasicBlock(int position, IList<InstructionBase> instructions)
		{
			Position = position;
			Instructions = instructions;
			Successors = new List<BasicBlock>();
		}
	}
}