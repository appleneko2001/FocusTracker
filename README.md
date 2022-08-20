# FocusTracker

A lightweight utility used for tracking which window get focused. Its useful to know which program always f\*\*\*in annoying you while you get busy or playing games. Inspired by focus.exe by HappyDroid

FocusTracker uses Windows Event Hook to catch which window get focused, not GetForegroundWindow api in loop. So it wont takes a lot CPU usages and RAMs.

FocusTracker is easy to use, less resources usage and providing detailed information to catch sus™

# Why you make this thing?

Because some program always flashing window while I'm busy, or I playing video games like Metro Exodus. Its annoying if anything to interrupt you without any reason. 4A Engine sucks too, no any official supports at all and \"Game paused\" bug wont get fixed in 2k22 year. 

So I have to find a way to know which program always flashing window or fix game by hooking windows messages. Second way is too hard for me, and I chose first way to go. 

I found a program named \"focus.exe\" in some questions (like [this one](https://superuser.com/questions/932561/windows-7-constantly-switching-focus-to-nothing)), but it uses loop and GetForegroundWindow with delay after I look their source code, may reach inaccurate result and wasting resources. 

I created a C# program, after I found SetWinEventHook api in [StackOverflow](https://stackoverflow.com/a/4407715), from page [StackOverflow - Get active window without global hooks or polling GetActiveWindow?](https://stackoverflow.com/questions/334243/get-active-window-without-global-hooks-or-polling-getactivewindow) (nice answer, DReJ and HK1, my life saver). The program will hook windows event to catch and print which program is get focused.

# Why "FocusTracker" ?

Actually the first name of this program is FocusDog (combination of Focus and WatchDog), but it conflicts to FocusDog productivity app. So I dont want to misleading others with this name and gonna think (find) better name for it. 

I took FocusTracker as the name of program, but it conflicts to "CKEditor - FocusTracker" and "FocusTracker XAML Control" too. I couldn't think shorter name sorry :((( but those program are pretty similar to working as mine works. 

So my conclusion is use "FocusTracker" as the name of this program. I might rename it if I got warns from somebody :((((

## The program uses those WinAPI:

+ SetConsoleOutputCP (change codepage of current console instance)
+ GetCurrentThreadId (necessary for PostThreadMessage)
+ PostThreadMessage (send a null message to trigger message loop, otherwise message loop will be stuck)
+ SetConsoleCtrlHandler (catch Ctrl + C combination keys and invoke handler)
+ GetWindowThreadProcessId (get process id by window handle, additional returns thread identicator)
+ OpenProcess (necessary for GetModuleFileNameExW)
+ GetModuleFileNameExW (get path to executable file by process handle)
+ SendMessageW (get window title length and their data from outside of targeted process)
+ CloseHandle (free process handle from OpenProcess to release used resources)
+ SetWinEventHook (hook windows event handler, require message loop to dispatch message to hook handler)
+ UnhookWinEvent (unhook windows event handler and release resources to this)
+ GetMessage (necessary for message loop)
+ DispatchMessage (necessary for message loop)

## Thanks to

+ Google and StackOverflow community (helped me a lot to find solution quickly, because documentation from Microsoft is tl;dr (jokin, I have to read it some parts to know what I need to avoid while I use WinAPI))
+ JetBrains Rider IDE (favor IDE, which I didn't know it even supports C++ programming, Resharped is good)

## Found a sus, with this utility

[MEGAsync](https://twitter.com/appleneko2001/status/1560512271515893760)
> Its time to fix your app to make it more "slient", MEGA.