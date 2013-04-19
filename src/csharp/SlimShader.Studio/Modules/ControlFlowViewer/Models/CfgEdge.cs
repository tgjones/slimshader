using QuickGraph;
using SlimShader.Studio.Modules.ControlFlowViewer.ViewModels;

namespace SlimShader.Studio.Modules.ControlFlowViewer.Models
{
	public class CfgEdge : Edge<BasicBlockViewModel>
	{
		public CfgEdge(BasicBlockViewModel source, BasicBlockViewModel target)
			: base(source, target)
		{
		}
	}
}