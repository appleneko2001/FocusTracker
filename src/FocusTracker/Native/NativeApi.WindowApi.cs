using System;
using System.Runtime.InteropServices;
using System.Text;
using FocusDog.Native;
using FocusTracker.Native.Enums;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable IdentifierTypo

namespace FocusTracker.Native
{
    internal static partial class NativeApi
    {
        [DllImport(WinUser)]
        internal static extern void DispatchMessage([In] ref Message message);

        [DllImport(WinUser)]
        internal static extern bool GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin,
            uint wMsgFilterMax);

        [DllImport(WinUser, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, MessageDefinitions msg, IntPtr wParam, StringBuilder lParam);
        
        [DllImport(WinUser, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, MessageDefinitions msg, IntPtr wParam, IntPtr lParam);

        [DllImport(WinUser, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostThreadMessage(IntPtr threadId, MessageDefinitions msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport(WinUser)]
        internal static extern IntPtr SetWinEventHook(WinEventKind eventMin, WinEventKind eventMax, IntPtr hmodWinEventProc,
            WinEventProc pfnWinEventProc, uint idProcess, uint idThread, WinEventFlags flags);

        [DllImport(WinUser)]
        internal static extern bool UnhookWinEvent(IntPtr hHook);
        
        [DllImport(WinUser)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        
        internal delegate void WinEventProc(IntPtr hEventHook, WinEventKind @event, IntPtr hWindow, int idObj, int idChild,
            uint idEventThread, uint eventTime);
    }
}