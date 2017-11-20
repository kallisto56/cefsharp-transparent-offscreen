namespace TransparentCefSharp {

	using System;
	using System.Drawing;
	using System.Windows.Forms;

	using CefSharp;
	using CefSharp.OffScreen;
	using Native;


	
	public sealed class ExampleForm : Form {

		public ChromiumWebBrowser Chromium;

		

		public ExampleForm()
		{
			this.InitializeForm();
			this.InitializeChromium();
			
			// Try experimenting and see what each thing,
			// AND, in combination with others, does.
			this.EnableBlurBehind();
			//this.HackBlurBehind();
			//this.MakeBorderlessWindow();
			this.ExtendClientArea();
		}



		private void InitializeForm()
		{
			this.Text = "Example of transparent CefSharp.Offscreen.ChromiumWebBrowser";
			this.Size = new Size(1024, 768);
			this.StartPosition = FormStartPosition.CenterScreen;
		}

		

		public void InitializeChromium()
		{
			// ...
			this.Chromium = new ChromiumWebBrowser("www.google.com");
			this.Chromium.Size = this.Size;

			// Open DevTools after browser initialized
			this.Chromium.BrowserInitialized += (s, e) => {
				this.Chromium.GetBrowser().GetHost().ShowDevTools();
			};

			// Set document.body background to transparent when loading state changed
			this.Chromium.LoadingStateChanged += (s, e) => {
				if (e.IsLoading == false) {
					this.Chromium.GetMainFrame().EvaluateScriptAsync("document.body.style.background='transparent';");
				}
			};

			// ...
			this.Chromium.NewScreenshot += this.OnPaintChromium;
		}



		private void EnableBlurBehind()
		{
			// Enables Windows 7 blur behind whole form
			var options = new BlurBehind(true);
			NativeMethods.DwmEnableBlurBehindWindow(this.Handle, ref options);
		}



		private void HackBlurBehind()
		{
			// Enables transparency, but without blur.
			// Technicaly, it's still there, but it's outside of visible region
			var options = new BlurBehind(true);
			using (var g = this.CreateGraphics()) {
				using (var r = new Region(new Rectangle(-1, -1, 1, 1))) {
					options.SetRegion(g, r);
				}
			}
			NativeMethods.DwmEnableBlurBehindWindow(this.Handle, ref options);
		}



		private void MakeBorderlessWindow()
		{
			// Advise, in case if you want this, study WM_NCHITTEST, it's simple, trust me.
			// Make sure, you implement WM_NCHITTEST on empty form first.
			this.FormBorderStyle = FormBorderStyle.None;
		}



		private void ExtendClientArea()
		{
			// This native method extends non-client area. If you want to draw on whole window,
			// search for tutorial about it on CodeProject, that involves handling WM_NCCALCSIZE.
			var margin = new NativeMargin(0, 64, 0, 0);
			NativeMethods.DwmExtendFrameIntoClientArea(this.Handle, ref margin);
		}



		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (this.Chromium == null) return;
			this.Chromium.Size = this.ClientSize;
		}



		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.Chromium.IsBrowserInitialized) return;
			var which = this.ConvertMouseButton(e.Button);
			if (which == -1) return;
			this.GetHost().SendMouseClickEvent(e.X, e.Y, (MouseButtonType)which, false, e.Clicks, CefEventFlags.None);
		}



		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!this.Chromium.IsBrowserInitialized) return;
			var which = this.ConvertMouseButton(e.Button);
			if (which == -1) return;
			this.GetHost().SendMouseClickEvent(e.X, e.Y, (MouseButtonType)which, true, e.Clicks, CefEventFlags.None);
		}



		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!this.Chromium.IsBrowserInitialized) return;
			this.GetHost().SendMouseMoveEvent(e.X, e.Y, false, CefEventFlags.None);
		}



		protected override void OnMouseWheel(MouseEventArgs e)
		{
            if (!this.Chromium.IsBrowserInitialized) return;
			var isShiftKeyDown = ((ModifierKeys & Keys.Shift) != Keys.None);
			this.Chromium.SendMouseWheelEvent(e.X, e.Y, (isShiftKeyDown ? e.Delta : 0), (!isShiftKeyDown ? e.Delta : 0), CefEventFlags.None);
		}



		private void OnPaintChromium(object sender, EventArgs e)
		{
			using (var graphics = this.CreateGraphics())
			{
				// This option allows us to paint what we want without erasing background first.
				graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

				// ...
				lock (this.Chromium.BitmapLock) {
					graphics.DrawImage(this.Chromium.Bitmap, new Rectangle(Point.Empty, this.Chromium.Bitmap.Size));
				}
			}
		}



		protected override void WndProc(ref Message m)
		{
			switch (m.Msg) {
				case (int)WindowMessages.WM_SYSCHAR:
                case (int)WindowMessages.WM_SYSKEYDOWN:
                case (int)WindowMessages.WM_SYSKEYUP:
                case (int)WindowMessages.WM_KEYDOWN:
                case (int)WindowMessages.WM_KEYUP:
                case (int)WindowMessages.WM_CHAR:
                case (int)WindowMessages.WM_IME_CHAR:

					// This part of code sends keyboard events...
					if (this.Chromium.IsBrowserInitialized) {
						var wParam = unchecked((int)m.WParam.ToInt64());
						var lParam = unchecked((int)m.LParam.ToInt64());
						this.GetHost().SendKeyEvent(m.Msg, wParam, lParam);
					}

					base.WndProc(ref m);
					break;

				default:
					base.WndProc(ref m);
					break;
			}
		}



		private IBrowserHost GetHost()
		{
			return this.Chromium.GetBrowser().GetHost();
		}



		private int ConvertMouseButton(MouseButtons button)
		{
			switch (button) {
				case MouseButtons.Left: return (int)MouseButtonType.Left;
				case MouseButtons.Middle: return (int)MouseButtonType.Middle;
				case MouseButtons.Right: return (int)MouseButtonType.Right;
				default: return -1;
			}
		}



	}



}
