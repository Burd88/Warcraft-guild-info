using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace WpfWindow
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
        public Int32 X;
        public Int32 Y;
    };

    class Mouse
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        public static Point GetMousePosition()
        {
            Win32Point point = new Win32Point();
            GetCursorPos(ref point);
            return new Point(point.X, point.Y);
        }
    }
}
