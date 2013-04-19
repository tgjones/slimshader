using QuickGraph;
using SlimShader.Studio.Modules.ControlFlowViewer.ViewModels;

namespace SlimShader.Studio.Modules.ControlFlowViewer.Models
{
	public class CfgGraph : BidirectionalGraph<BasicBlockViewModel, CfgEdge>
	{
		public CfgGraph()
			: base(true)
		{

		}
	}
}