namespace AutoClicker;

public class AutoClickerEngine : IDisposable
{
    private Thread? _clickThread;
    private bool _isRunning;
    private int _clickInterval = 100;
    private NativeMethods.POINT _lockedPosition;
    private bool _positionLocked;
    private MouseButton _mouseButton = MouseButton.Left;

    public bool IsRunning => _isRunning;
    public int ClickInterval
    {
        get => _clickInterval;
        set => _clickInterval = Math.Max(50, value);
    }

    public MouseButton MouseButton
    {
        get => _mouseButton;
        set => _mouseButton = value;
    }

    public void Start()
    {
        if (_isRunning) return;

        _isRunning = true;
        NativeMethods.GetCursorPos(out _lockedPosition);
        _positionLocked = true;

        _clickThread = new Thread(ClickLoop)
        {
            IsBackground = true
        };
        _clickThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
        _clickThread?.Join(1000);
        _clickThread = null;
    }

    private void ClickLoop()
    {
        while (_isRunning)
        {
            if (_positionLocked)
            {
                PerformClick(_lockedPosition.X, _lockedPosition.Y);
            }
            else
            {
                NativeMethods.GetCursorPos(out var currentPos);
                PerformClick(currentPos.X, currentPos.Y);
            }

            Thread.Sleep(_clickInterval);
        }
    }

    private void PerformClick(int x, int y)
    {
        var currentPos = new NativeMethods.POINT();
        NativeMethods.GetCursorPos(out currentPos);

        NativeMethods.SetCursorPos(x, y);

        if (_mouseButton == MouseButton.Left)
        {
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(10);
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }
        else
        {
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(10);
            NativeMethods.mouse_event(NativeMethods.MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }

        NativeMethods.SetCursorPos(currentPos.X, currentPos.Y);
    }

    public void UpdateLockedPosition()
    {
        NativeMethods.GetCursorPos(out _lockedPosition);
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}

public enum MouseButton
{
    Left,
    Right
}
