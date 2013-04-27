using System.ComponentModel.Composition;
using System.Reflection;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Results;
using Gemini.Framework.Services;
using SlimShader.Studio.Modules.ControlFlowViewer.ViewModels;

namespace SlimShader.Studio.Modules.Startup
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		private readonly IResourceManager _resourceManager;

		[ImportingConstructor]
		public Module(IResourceManager resourceManager)
		{
			_resourceManager = resourceManager;
		}

		public override void Initialize()
		{
			Shell.Title = "SlimShader Studio";
			Shell.Icon = _resourceManager.GetBitmap("Resources/Icon.ico",
				Assembly.GetExecutingAssembly().GetAssemblyName());
			Shell.ShowTool(IoC.Get<ControlFlowViewerViewModel>());
		}
	}
}