using System.ComponentModel.Composition;
using Gemini.Framework;
using Gemini.Framework.Services;
using SlimShader.Studio.Framework;
using SlimShader.Studio.Modules.ShaderViewer.ViewModels;

namespace SlimShader.Studio.Modules.ShaderViewer
{
	[Export(typeof(IEditorProvider))]
	public class EditorProvider : IEditorProvider
	{
		private readonly IPropertyGrid _propertyGrid;
		private readonly IExtendedShell _extendedShell;

		[ImportingConstructor]
		public EditorProvider(IPropertyGrid propertyGrid, IExtendedShell extendedShell)
		{
			_propertyGrid = propertyGrid;
			_extendedShell = extendedShell;
		}

		public bool Handles(string path)
		{
			return true;
		}

		public IDocument Create(string path)
		{
			var editor = new EditorViewModel(_propertyGrid, _extendedShell);
			editor.Open(path);
			return editor;
		}
	}
}