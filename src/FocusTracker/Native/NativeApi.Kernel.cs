using System.Runtime.InteropServices;

namespace FocusTracker.Native
{
    internal static partial class NativeApi
    {
        [DllImport(WinKernel, CharSet = CharSet.Auto)]
        internal static extern int GetCurrentThreadId();
    }
}