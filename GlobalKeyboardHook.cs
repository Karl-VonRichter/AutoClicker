using System.Runtime.InteropServices;

namespace AutoClicker;

public class GlobalKeyboardHook : IDisposable
{
    private IntPtr _hookId = IntPtr.Zero;
    private NativeMethods.LowLevelKeyboardProc? _proc;

    public event EventHandler<KeyEventArgs>? KeyPressed;

    public GlobalKeyboardHook()
    {
        _proc = HookCallback;
        _hookId = SetHook(_proc);
    }

    private IntPtr SetHook(NativeMethods.LowLevelKeyboardProc proc)
    {
        using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        return NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_KEYBOARD_LL,
            proc,
            NativeMethods.GetModuleHandle(curModule?.ModuleName ?? ""),
            0);
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)NativeMethods.WM_KEYDOWN || wParam == (IntPtr)NativeMethods.WM_SYSKEYDOWN))
        {
            var hookStruct = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
            var key = (Keys)hookStruct.vkCode;
            var modifiers = Keys.None;

            if ((NativeMethods.GetAsyncKeyState((int)Keys.ControlKey) & 0x8000) != 0)
                modifiers |= Keys.Control;
            if ((NativeMethods.GetAsyncKeyState((int)Keys.ShiftKey) & 0x8000) != 0)
                modifiers |= Keys.Shift;
            if ((NativeMethods.GetAsyncKeyState((int)Keys.Menu) & 0x8000) != 0)
                modifiers |= Keys.Alt;

            KeyPressed?.Invoke(this, new KeyEventArgs(key | modifiers));
        }

        return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_hookId != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }
        GC.SuppressFinalize(this);
    }
}
