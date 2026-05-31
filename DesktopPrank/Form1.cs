using System.Runtime.InteropServices;

namespace DesktopPrank;

public partial class Form1 : Form
{
    private const int ProgressDurationMs = 1500;
    private const int ProgressIntervalMs = 50;
    private const int IconPadding = 12;

    private readonly System.Windows.Forms.Timer _progressTimer;
    private int _progressValue;
    private bool _prankActive;
    private IntPtr _desktopListViewHandle;

    public Form1()
    {
        InitializeComponent();

        cleanDesktopButton.Click += CleanDesktopButton_Click;

        _progressTimer = new System.Windows.Forms.Timer { Interval = ProgressIntervalMs };
        _progressTimer.Tick += ProgressTimer_Tick;
    }

    protected override void OnLocationChanged(EventArgs e)
    {
        base.OnLocationChanged(e);
        if (_prankActive)
        {
            HideIconsBehindWindow(Bounds);
        }
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_MOVING = 0x0216;
        if (_prankActive && m.Msg == WM_MOVING)
        {
            var rect = Marshal.PtrToStructure<RECT>(m.LParam);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            HideIconsBehindWindow(bounds);
        }

        base.WndProc(ref m);
    }

    private void CleanDesktopButton_Click(object? sender, EventArgs e)
    {
        if (_progressTimer.Enabled)
        {
            return;
        }

        cleanDesktopButton.Enabled = false;
        cleanDesktopButton.Text = "Cleaning...";
        _progressValue = 0;
        cleanProgressBar.Value = 0;
        cleanProgressBar.Visible = true;
        _progressTimer.Start();
    }

    private void ProgressTimer_Tick(object? sender, EventArgs e)
    {
        var increment = (int)Math.Ceiling(100.0 * ProgressIntervalMs / ProgressDurationMs);
        _progressValue = Math.Min(100, _progressValue + increment);
        cleanProgressBar.Value = _progressValue;

        if (_progressValue >= 100)
        {
            _progressTimer.Stop();
            cleanProgressBar.Visible = false;
            cleanDesktopButton.Text = "Desktop Cleaned!";
            cleanDesktopButton.Enabled = true;
            ActivatePrank();
        }
    }

    private void ActivatePrank()
    {
        _desktopListViewHandle = GetDesktopListView();
        if (_desktopListViewHandle == IntPtr.Zero)
        {
            MessageBox.Show(this, "Couldn't find the desktop icon list.", "Desktop Cleaner",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _prankActive = true;
        HideIconsBehindWindow(Bounds);
    }

    private void HideIconsBehindWindow(Rectangle bounds)
    {
        if (_desktopListViewHandle == IntPtr.Zero)
        {
            return;
        }

        int count = SendMessage(_desktopListViewHandle, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero).ToInt32();
        if (count <= 0)
        {
            return;
        }

        int width = Math.Max(1, bounds.Width - IconPadding * 2);
        int height = Math.Max(1, bounds.Height - IconPadding * 2);

        int columns = Math.Max(1, (int)Math.Ceiling(Math.Sqrt(count * width / (double)height)));
        int rows = (int)Math.Ceiling(count / (double)columns);
        int xStep = Math.Max(1, width / columns);
        int yStep = Math.Max(1, height / rows);

        for (int i = 0; i < count; i++)
        {
            int col = i % columns;
            int row = i / columns;
            int x = bounds.Left + IconPadding + col * xStep;
            int y = bounds.Top + IconPadding + row * yStep;
            SendMessage(_desktopListViewHandle, LVM_SETITEMPOSITION32, new IntPtr(i), MakeLParam(x, y));
        }
    }

    private static IntPtr GetDesktopListView()
    {
        IntPtr progman = FindWindow("Progman", null);
        IntPtr defView = FindWindowEx(progman, IntPtr.Zero, "SHELLDLL_DefView", null);
        if (defView == IntPtr.Zero)
        {
            IntPtr workerw = IntPtr.Zero;
            while ((workerw = FindWindowEx(IntPtr.Zero, workerw, "WorkerW", null)) != IntPtr.Zero)
            {
                defView = FindWindowEx(workerw, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (defView != IntPtr.Zero)
                {
                    break;
                }
            }
        }

        return defView == IntPtr.Zero
            ? IntPtr.Zero
            : FindWindowEx(defView, IntPtr.Zero, "SysListView32", null);
    }

    private static IntPtr MakeLParam(int x, int y)
    {
        return unchecked((IntPtr)(int)((y << 16) | (x & 0xFFFF)));
    }

    private const int LVM_FIRST = 0x1000;
    private const int LVM_GETITEMCOUNT = LVM_FIRST + 4;
    private const int LVM_SETITEMPOSITION32 = LVM_FIRST + 49;

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string? className, string? windowTitle);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
}
