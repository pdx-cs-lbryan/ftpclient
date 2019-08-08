/* References:
 * 1.) https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getlastinputinfo
 * 2.) http://joelabrahamsson.com/detecting-mouse-and-keyboard-input-with-net/
 * 3.) http://www.blackwasp.co.uk/InactivityDetection.aspx
 */

using System;
using System.Runtime.InteropServices;
namespace FtpClientApp
{
    public class Timeout
    {
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public TimeSpan? TimemoutTime()
        { 

            LASTINPUTINFO info = new LASTINPUTINFO();
            info.cbSize = (uint) Marshal.SizeOf(info);
            if (GetLastInputInfo(ref info))
                return TimeSpan.FromMilliseconds(Environment.TickCount - info.dwTime);
            else
                return null;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }
}
