using System;
using System.ComponentModel.Composition;
using SlimShader.Studio.Modules.ShaderViewer.ViewModels;

namespace SlimShader.Studio.Framework
{
	[Export(typeof(IExtendedShell))]
	public class ExtendedShell : IExtendedShell
	{
		public event EventHandler<ActiveDocumentChangedEventArgs> ActiveDocumentChanged;

		public void RaiseActiveDocumentChanged(EditorViewModel editor)
		{
			var handler = ActiveDocumentChanged;
			if (handler != null)
				handler(this, new ActiveDocumentChangedEventArgs(editor));
		}
	}
}