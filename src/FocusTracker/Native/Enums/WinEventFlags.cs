using System;

namespace FocusTracker.Native.Enums
{
    // Credit https://www.pinvoke.net/default.aspx/user32/SetWinEventHook.html
    [Flags]
    public enum WinEventFlags: uint
    {
        OutOfContext = 0x0000, // Events are ASYNC
        SkipOwnThread = 0x0001, // Don't call back for events on installer's thread
        SkipOwnProcess = 0x0002, // Don't call back for events on installer's process
        InContext = 0x0004 // Events are SYNC, this causes your dll to be injected into every process
    }
}