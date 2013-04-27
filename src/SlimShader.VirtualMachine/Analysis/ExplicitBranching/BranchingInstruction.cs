namespace SlimShader.VirtualMachine.Analysis.ExplicitBranching
{
	public class BranchingInstruction : InstructionBase
	{
		public InstructionBase BranchTarget { get; set; }
		public BranchType BranchType { get; set; }

		public override bool IsUnconditionalBranch
		{
			get { return BranchType == BranchType.Unconditional; }
		}

		public override bool BranchesTo(InstructionBase otherInstruction)
		{
			return BranchTarget == otherInstruction;
		}

		public override string ToString()
		{
			return "*branch* " + BranchType.ToString().ToLower();
		}
	}
}