using System.ComponentModel.Composition;
using System.Linq;
using Gemini.Framework;
using Gemini.Framework.Services;
using SlimShader.Studio.Framework;
using SlimShader.Studio.Modules.ControlFlowViewer.Models;
using SlimShader.VirtualMachine.Analysis;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.Studio.Modules.ControlFlowViewer.ViewModels
{
	[Export(typeof(ControlFlowViewerViewModel))]
	public class ControlFlowViewerViewModel : Tool
	{
		public override string DisplayName
		{
			get { return "Control Flow"; }
		}

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Right; }
		}

		private readonly CfgGraph _graph;
		public CfgGraph Graph
		{
			get { return _graph; }
		}

		public string LayoutAlgorithmType
		{
			get
			{
				if (_graph.Vertices.Count() > 1)
					return "EfficientSugiyama";
				return "CompoundFDP";
			}
		}

		[ImportingConstructor]
		public ControlFlowViewerViewModel(IExtendedShell extendedShell)
		{
			_graph = new CfgGraph();

			extendedShell.ActiveDocumentChanged += (sender, e) =>
			{
				Graph.RemoveVertexIf(x => true);
				Graph.Clear();

				var rewrittenInstructions = ExplicitBranchingRewriter.Rewrite(e.Editor.BytecodeContainer.Shader.InstructionTokens);
				var cfg = ControlFlowGraph.FromInstructions(rewrittenInstructions);

				var basicBlockLookup = cfg.BasicBlocks.ToDictionary(x => x, x => new BasicBlockViewModel(x));

				Graph.AddVertexRange(basicBlockLookup.Values);

				foreach (var basicBlock in cfg.BasicBlocks)
				{
					foreach (var successor in basicBlock.Successors)
						Graph.AddEdge(new CfgEdge(basicBlockLookup[basicBlock], basicBlockLookup[successor]));
					if (basicBlock.ImmediatePostDominator != null)
						Graph.AddEdge(new CfgEdge(basicBlockLookup[basicBlock],
							basicBlockLookup[basicBlock.ImmediatePostDominator])
						{
							IsImmediatePostDominatorEdge = true
						});
				}

				NotifyOfPropertyChange(() => LayoutAlgorithmType);
			};
		}
	}
}