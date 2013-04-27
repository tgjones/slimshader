using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Analysis.ExplicitBranching
{
	public abstract class InstructionBase
	{
		public InstructionToken InstructionToken { get; set; }

		public virtual bool IsUnconditionalBranch
		{
			get { return false; }
		}

		public virtual bool BranchesTo(InstructionBase otherInstruction)
		{
			return false;
		}
	}
}