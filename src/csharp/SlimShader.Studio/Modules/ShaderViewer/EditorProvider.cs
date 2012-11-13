using System.ComponentModel.Composition;
using Gemini.Framework;
using Gemini.Framework.Services;
using SlimShader.Studio.Modules.ShaderViewer.ViewModels;

namespace SlimShader.Studio.Modules.ShaderViewer
{
	[Export(typeof(IEditorProvider))]
	public class EditorProvider : IEditorProvider
	{
		private readonly IPropertyGrid _propertyGrid;

		[ImportingConstructor]
		public EditorProvider(IPropertyGrid propertyGrid)
		{
			_propertyGrid = propertyGrid;
		}

		public bool Handles(string path)
		{
			return true;
		}

		public IDocument Create(string path)
		{
			var editor = new EditorViewModel(_propertyGrid);
			editor.Open(path);
			return editor;
		}
	}
}