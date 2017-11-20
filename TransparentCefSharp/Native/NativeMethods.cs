namespace TransparentCefSharp.Native {

	using System;
	using System.Runtime.InteropServices;



	/// <summary>
	/// Platform Invocation methods, such as those that are marked by using the System.Runtime.InteropServices.DllImportAttribute attribute and access unmanaged code.
	/// </summary>
	internal static class NativeMethods {



		/// <summary>
		/// Extends the window frame into the client area.
		/// </summary>
		/// <param name="hwnd">The handle to the window in which the frame will be extended into the client area.</param>
		/// <param name="margin">A pointer to a MARGINS structure that describes the margins to use when extending the frame into the client area.</param>
		[DllImport("dwmapi.dll", PreserveSig = false)]
		internal static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref NativeMargin margin);



		/// <summary>
		/// Enables the blur effect on a specified window.
		/// </summary>
		/// <param name="hwnd">The handle to the window on which the blur behind data is applied.</param>
		/// <param name="blurBehind">A pointer to a BlurBehind structure that provides blur behind data.</param>
		/// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[DllImport("dwmapi.dll")]
		internal static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref BlurBehind blurBehind);
		


		/// <summary>
		/// The GetWindowDC function retrieves the device context (DC) for the entire window, including title bar, menus, and scroll bars. A window device context permits painting anywhere in a window, because the origin of the device context is the upper-left corner of the window instead of the client area.
		/// GetWindowDC assigns default attributes to the window device context each time it retrieves the device context. Previous attributes are lost.
		/// </summary>
		/// <param name="hWnd">A handle to the window with a device context that is to be retrieved. If this value is NULL, GetWindowDC retrieves the device context for the entire screen. If this parameter is NULL, GetWindowDC retrieves the device context for the primary display monitor. To get the device context for other display monitors, use the EnumDisplayMonitors and CreateDC functions.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindowDC(IntPtr hWnd);



		/// <summary>
		/// The ReleaseDC function releases a device context (DC), freeing it for use by other applications. The effect of the ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private DCs.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose DC is to be released.</param>
		/// <param name="hDc">A handle to the DC to be released.</param>
		/// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1. If the DC was not released, the return value is zero.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);



	}



}
