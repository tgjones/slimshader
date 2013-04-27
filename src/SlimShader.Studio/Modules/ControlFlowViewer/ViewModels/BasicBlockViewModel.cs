using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using SlimShader.Studio.Framework;
using SlimShader.VirtualMachine.Analysis.ControlFlow;

namespace SlimShader.Studio.Modules.ControlFlowViewer.ViewModels
{
	public class BasicBlockViewModel : PropertyChangedBase
	{
		private readonly List<string> _instructions;
		public IEnumerable<string> Instructions
		{
			get { return _instructions; }
		}

		public BasicBlockViewModel(BasicBlock basicBlock)
		{
			if (basicBlock.Instructions.Count > 10)
				_instructions = basicBlock.Instructions.Take(5).Select(x => x.ToString().Truncate(40))
				    .Union(new[] { "..." })
					.Union(basicBlock.Instructions.Skip(basicBlock.Instructions.Count - 5).Take(5).Select(x => x.ToString().Truncate(40)))
				    .ToList();
			else
				_instructions = basicBlock.Instructions.Select(x => x.ToString().Truncate(40)).ToList();
		}
	}
}