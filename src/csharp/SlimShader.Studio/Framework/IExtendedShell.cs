using System;
using SlimShader.Studio.Modules.ShaderViewer.ViewModels;

namespace SlimShader.Studio.Framework
{
	public interface IExtendedShell
	{
		event EventHandler<ActiveDocumentChangedEventArgs> ActiveDocumentChanged;
		void RaiseActiveDocumentChanged(EditorViewModel editor);
	}
}