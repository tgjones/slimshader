using System.ComponentModel.Composition;
using Gemini.Framework;

namespace SlimShader.Studio.Modules.Startup
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
		public override void Initialize()
		{
			Shell.Title = "SlimShader Studio";
		}
	}
}