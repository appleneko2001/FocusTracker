## FocusTracker: C++ version

Less RAM usage, faster (and its painful to code for me, F widechar and UTF-16)
anyway it works fine, and supports unicode character output.

## Tested on 

+ Windows Terminal 1.12.10732.0, Monospaced Font: Consolas, Iosevka Fixed, No-monospace Font: Microsoft YaHei, Microsoft YaHei UI, Microsoft JhengHei UI.
+ Conhost (which you double click on executable), Monospaced Font: NSimSun, MS Gothic, SimSun-ExtB, MingLiU

> Tips for Windows Terminal users: Terminal will print CJK chars anyway, if you use monospaced font.

## How to use

+ Double click to executable (not recommended, you need to do some configuration to let console prints CJK characters
+ Windows Terminal (cmd or powershell profile) and run executable by typing path to executable file and Enter.

Binary is compiled by MSVC 14.29.30133, Microsoft Visual Studio 2019

## Known issue

Terminal, which unsupported / have weird behaviours:

+ Git Bash
+ and my JetBrains Rider
