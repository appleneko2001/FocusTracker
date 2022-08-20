using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using FocusTracker.Native;
using FocusTracker.Native.Enums;
using static FocusTracker.Native.NativeApi;

namespace FocusTracker
{
    /// <summary>
    /// Inspired by "focus.exe" from HappyDroid.com
    /// </summary>
    internal static class Program
    {
        private static readonly object CtrlCLock = new object();
        private static IntPtr _messageLoopThread;
        private static readonly CancellationTokenSource CancellationTokenSource =
            new CancellationTokenSource();

        // ReSharper disable once InconsistentNaming
        private static Func<IntPtr, string> GetWindowTitle;

        public static void Main(string[] args)
        {
            GetWindowTitle = IntPtr.Size == 8 ? (Func<IntPtr, string>) GetText_x86_64 : GetText_x86_32;

            _messageLoopThread = new IntPtr(GetCurrentThreadId());
            Console.CancelKeyPress += OnCancelKeyPress;

            var callback = new WinEventProc(WinEventProc);
            var hook = SetWinEventHook(WinEventKind.EVENT_SYSTEM_FOREGROUND,
                WinEventKind.EVENT_SYSTEM_FOREGROUND, 
                IntPtr.Zero,
                callback, 
                0, 0,
                WinEventFlags.OutOfContext | WinEventFlags.SkipOwnProcess);

            if (hook == IntPtr.Zero)
                throw new Win32Exception();
            
            Console.WriteLine("FocusTracker instance created. Press Control + C to perform stop instance.");
                
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                while (GetMessage(out var message, IntPtr.Zero, 0, 0))
                {
                    if(message.MessageKind == MessageDefinitions.Quit ||
                       message.MessageKind == MessageDefinitions.Null)
                        break;

                    DispatchMessage(ref message);
                }
            }

            Console.WriteLine("stopping FocusTracker instance.");
            if (!UnhookWinEvent(hook))
                throw new Exception("Unable to unhook windows event");
            
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        private static void WinEventProc(IntPtr hEventHook, WinEventKind @event, IntPtr hWindow, int idObj, int idChild, uint idEventThread, uint eventTime)
        {
            if(@event != WinEventKind.EVENT_SYSTEM_FOREGROUND)
                return;

            var timespan = DateTime.Now.ToString("u");
            var threadId = GetWindowThreadProcessId(hWindow, out var pid);
            var procName = Process.GetProcessById((int)pid).ProcessName;
            Console.WriteLine($"[{timespan}] \"{GetWindowTitle(hWindow)}\" (#{hWindow.ToString("x8")}, PID#{pid}, Thread#{threadId}, Name:{procName})");
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            lock (CtrlCLock)
            {
                if (CancellationTokenSource.IsCancellationRequested)
                    return;
                
                Console.WriteLine("Pressed Control + C (Cancel).");
                CancellationTokenSource.Cancel();

                if (!PostThreadMessage(_messageLoopThread, 
                        MessageDefinitions.Null,
                        IntPtr.Zero, IntPtr.Zero))
                {
                    throw new Win32Exception();
                }
                
                Console.CancelKeyPress -= OnCancelKeyPress;
            }
        }
        
        private static string GetText_x86_32(IntPtr hWnd)
        {
            var length = SendMessage(hWnd, 
                    MessageDefinitions.GetTextLength, 
                    IntPtr.Zero, 
                    IntPtr.Zero)
                .ToInt32();
            var sb = new StringBuilder(length + 1);
            SendMessage(hWnd, MessageDefinitions.GetText, (IntPtr)sb.Capacity, sb);
            return sb.ToString();
        }
        
        private static string GetText_x86_64(IntPtr hWnd)
        {
            var length = (int)SendMessage(hWnd, 
                MessageDefinitions.GetTextLength, 
                IntPtr.Zero, 
                IntPtr.Zero)
                .ToInt64();
            var sb = new StringBuilder(length + 1);
            SendMessage(hWnd, MessageDefinitions.GetText, (IntPtr)sb.Capacity, sb);
            return sb.ToString();
        }
    }
}