using System.ComponentModel.Composition;
using System.IO;
using Gemini.Framework;
using Gemini.Framework.Services;
using SlimShader.Studio.Framework;

namespace SlimShader.Studio.Modules.ShaderViewer.ViewModels
{
	[Export]
	public class EditorViewModel : Document
	{
		private readonly IPropertyGrid _propertyGrid;
		private readonly IExtendedShell _extendedShell;

		private string _path;
		private string _fileName;
		private BytecodeContainer _bytecodeContainer;
		private string _disassembledCode;

		public override string DisplayName
		{
			get { return _fileName; }
		}

		public BytecodeContainer BytecodeContainer
		{
			get { return _bytecodeContainer; }
		}

		public string DisassembledCode
		{
			get { return _disassembledCode; }
			private set
			{
				_disassembledCode = value;
				NotifyOfPropertyChange(() => DisassembledCode);
			}
		}

		public EditorViewModel(IPropertyGrid propertyGrid, IExtendedShell extendedShell)
		{
			_extendedShell = extendedShell;
			_propertyGrid = propertyGrid;
		}

		public void Open(string path)
		{
			_path = path;
			_fileName = Path.GetFileName(path);
			_bytecodeContainer = BytecodeContainer.Parse(File.ReadAllBytes(path));
			DisassembledCode = _bytecodeContainer.ToString();
		}

		public override bool Equals(object obj)
		{
			var other = obj as EditorViewModel;
			return other != null && string.Compare(_path, other._path) == 0;
		}

		protected override void OnActivate()
		{
			_extendedShell.RaiseActiveDocumentChanged(this);
			_propertyGrid.SelectedObject = _bytecodeContainer.Statistics;
			base.OnActivate();
		}
	}
}