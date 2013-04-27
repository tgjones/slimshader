using System;
using SlimShader.Studio.Modules.ShaderViewer.ViewModels;

namespace SlimShader.Studio.Framework
{
	public class ActiveDocumentChangedEventArgs : EventArgs
	{
		public EditorViewModel Editor { get; private set; }

		public ActiveDocumentChangedEventArgs(EditorViewModel editor)
		{
			Editor = editor;
		}
	}
}