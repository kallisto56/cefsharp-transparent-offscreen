namespace TransparentCefSharp.Native {
	
	using System;
	using System.Runtime.InteropServices;



	[StructLayout(LayoutKind.Sequential)]
	struct BlurBehind {

		public BlurBehindFlags dwFlags;
		public bool fEnable;
		public IntPtr hRgnBlur;
		public bool fTransitionOnMaximized;



		public BlurBehind(bool enabled)
		{
			fEnable = enabled;
			hRgnBlur = IntPtr.Zero;
			fTransitionOnMaximized = false;
			dwFlags = BlurBehindFlags.Enable;
		}



		public System.Drawing.Region Region {
			get {
				return System.Drawing.Region.FromHrgn(hRgnBlur);
			}
		}



		public bool TransitionOnMaximized {
			get {
				return this.fTransitionOnMaximized == true;
			}
			set {
				this.fTransitionOnMaximized = value ? true : false;
				this.dwFlags |= BlurBehindFlags.TransitionMaximized;
			}
		}



		public void SetRegion(System.Drawing.Graphics graphics, System.Drawing.Region region)
		{
			hRgnBlur = region.GetHrgn(graphics);
			dwFlags |= BlurBehindFlags.BlurRegion;
		}



	}



}

