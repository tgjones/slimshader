namespace SlimShader.VirtualMachine.Resources
{
	public class UnorderedAccessView
	{
		private readonly int _width;
		private readonly int _height;
		private readonly float[,] _data;

		public float this[int x, int y]
		{
			get { return _data[x, y]; }
			set { _data[x, y] = value; }
		}

		public UnorderedAccessView(int width, int height)
		{
			_width = width;
			_height = height;
			_data = new float[width,height];
		}
	}
}