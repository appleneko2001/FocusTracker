// I HATE F WIDE CHAR

#include <cstdio>
#include <exception>
#include <io.h>
#include <fcntl.h>

#include <iostream>
#include <Windows.h>
#include <chrono>

DWORD MainThreadId;
bool RequiredStop;
const char* NullProcName = u8"(null)";

void CALLBACK WinEventProc(HWINEVENTHOOK eventHook, DWORD eventKind,
    HWND window, LONG idObj, LONG idChild, DWORD idEventThread, DWORD eventTime);

BOOL CALLBACK CtrlCHandle(DWORD ctrl_type);

int main(int argc, char* argv[])
{
    SetConsoleOutputCP(CP_UTF8);
    _setmode(_fileno(stdout), _O_U16TEXT);
    setlocale(LC_ALL, "");

    MainThreadId = GetCurrentThreadId();
    auto hook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND,
        EVENT_SYSTEM_FOREGROUND,
        nullptr,
        WinEventProc, 0, 0, WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);

    if(hook == nullptr)
        throw std::exception("Unable to hook windows event");

    SetConsoleCtrlHandler(CtrlCHandle, true);
    
    wprintf_s(L"FocusTracker instance created. Press Control + C to perform stop instance.\n");

    MSG message;
    while (!RequiredStop)
    {
        while (GetMessage(&message, nullptr, 0, 0))
        {
            if(message.message == WM_QUIT || message.message == WM_NULL)
                break;

            DispatchMessage(&message);
        }
    }

    wprintf_s(L"Stopping FocusTracker instance.\n");
    if(!UnhookWinEvent(hook))
        throw std::exception("Unable to unhook windows event");

    wprintf_s(L"Press Enter key to continue.\n");
    char sym[1];
    std::cin.read(sym, 1);
    return 0;
}

BOOL CALLBACK CtrlCHandle(DWORD ctrl_type)
{
    wprintf_s(L"Pressed Control + C (Cancel).\n");
    RequiredStop = true;

    if(!PostThreadMessage(MainThreadId, WM_NULL, 0, 0))
        throw std::exception("Unable to trigger message loop");

    SetConsoleCtrlHandler(CtrlCHandle, false);
    return true;
}

void CALLBACK WinEventProc(HWINEVENTHOOK eventHook, DWORD eventKind,
    HWND window, LONG idObj, LONG idChild, DWORD idEventThread, DWORD eventTime)
{
    if(eventKind != EVENT_SYSTEM_FOREGROUND)
        return;

    time_t raw_time;
    time(&raw_time);
    std::tm timespan{};
    localtime_s(&timespan, &raw_time);

    wchar_t* title = nullptr;

    wchar_t processName[260] { '\0' };
    
    DWORD processId;
    const auto thread_id = GetWindowThreadProcessId(window, &processId);

    if(processId != 0)
    {
        const auto process = OpenProcess(PROCESS_QUERY_INFORMATION , false, processId);
        auto process_name_size = static_cast<DWORD>(sizeof processName / sizeof(wchar_t));
        
        if(!QueryFullProcessImageNameW(process, 0, &processName[0], &process_name_size))
        {
            wprintf_s(L"Unable to get process name: 0x%X\n", GetLastError());
        }
        const size_t title_size = SendMessageW(window, WM_GETTEXTLENGTH, 0, 0) + 1;
        title = static_cast<wchar_t*>(calloc(title_size, sizeof(wchar_t*)));
        SendMessageW(window, WM_GETTEXT, title_size, reinterpret_cast<LPARAM>(&title[0]));
        
        CloseHandle(process);
    }
    else
    {
        swprintf_s(&processName[0], 256, L"%hs", NullProcName);
    }
    
    char timeString[128];
    strftime(&timeString[0], 128, "%x %T", &timespan);

    if(wprintf_s(L"[%hs] \"%s\" (#%p, PID#%lu, Thread#%lu, Name:%s)\n", timeString, title,
        window, processId, thread_id, processName) < 0)
    {
        wprintf(L"\nError: ");
        perror("wprintf_s");
    }

    if(title != nullptr)
        free(title);
}


