namespace TransparentCefSharp.Native {
	
	using System.Runtime.InteropServices;



	[StructLayout(LayoutKind.Sequential)]
	struct NativeMargin {

		public int Left;
		public int Right;
		public int Top;
		public int Bottom;



		public NativeMargin(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}



	}



}
