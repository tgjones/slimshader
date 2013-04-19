using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Menus;
using Gemini.Framework.Results;
using SlimShader.Studio.Modules.ControlFlowViewer.ViewModels;

namespace SlimShader.Studio.Modules.ControlFlowViewer
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		public override void Initialize()
		{
			MainMenu.All.First(x => x.Name == "View")
				.Add(new MenuItem("Control Flow", OpenControlFlowViewer));
		}

		private static IEnumerable<IResult> OpenControlFlowViewer()
		{
			yield return Show.Tool<ControlFlowViewerViewModel>();
		}
	}
}