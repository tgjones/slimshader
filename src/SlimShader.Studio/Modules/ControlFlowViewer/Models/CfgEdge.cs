using QuickGraph;
using SlimShader.Studio.Modules.ControlFlowViewer.ViewModels;

namespace SlimShader.Studio.Modules.ControlFlowViewer.Models
{
	public class CfgEdge : Edge<BasicBlockViewModel>
	{
		public bool IsImmediatePostDominatorEdge { get; set; }

		public CfgEdge(BasicBlockViewModel source, BasicBlockViewModel target)
			: base(source, target)
		{
		}
	}
}