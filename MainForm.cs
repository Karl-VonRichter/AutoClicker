namespace AutoClicker;

public partial class MainForm : Form
{
    private readonly AutoClickerEngine _engine;
    private readonly GlobalKeyboardHook _keyboardHook;
    private readonly System.Windows.Forms.Timer _updateTimer;
    private NotifyIcon? _trayIcon;
    private Icon? _baseIcon;

    public MainForm()
    {
        InitializeComponent();

        _engine = new AutoClickerEngine();
        _keyboardHook = new GlobalKeyboardHook();
        _keyboardHook.KeyPressed += OnGlobalKeyPressed;

        _updateTimer = new System.Windows.Forms.Timer
        {
            Interval = 100
        };
        _updateTimer.Tick += UpdateTimer_Tick;
        _updateTimer.Start();

        InitializeTrayIcon();
        UpdateUI();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.ClientSize = new Size(400, 280);
        this.Text = "Non-Intrusive Auto Clicker";
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

        var statusLabel = new Label
        {
            Name = "statusLabel",
            Text = "Status: Stopped",
            Location = new Point(20, 20),
            Size = new Size(360, 30),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold)
        };

        var intervalLabel = new Label
        {
            Text = "Click Interval (ms):",
            Location = new Point(20, 70),
            Size = new Size(150, 20)
        };

        var intervalNumeric = new NumericUpDown
        {
            Name = "intervalNumeric",
            Location = new Point(180, 68),
            Size = new Size(100, 23),
            Minimum = 50,
            Maximum = 10000,
            Value = 100,
            Increment = 10
        };
        intervalNumeric.ValueChanged += (s, e) =>
        {
            _engine.ClickInterval = (int)intervalNumeric.Value;
        };

        var buttonLabel = new Label
        {
            Text = "Mouse Button:",
            Location = new Point(20, 105),
            Size = new Size(150, 20)
        };

        var buttonComboBox = new ComboBox
        {
            Name = "buttonComboBox",
            Location = new Point(180, 103),
            Size = new Size(100, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        buttonComboBox.Items.AddRange(new[] { "Left", "Right" });
        buttonComboBox.SelectedIndex = 0;
        buttonComboBox.SelectedIndexChanged += (s, e) =>
        {
            _engine.MouseButton = buttonComboBox.SelectedIndex == 0 ? MouseButton.Left : MouseButton.Right;
        };

        var hotkeysLabel = new Label
        {
            Text = "Hotkeys:",
            Location = new Point(20, 145),
            Size = new Size(360, 20),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold)
        };

        var hotkeyInfo = new Label
        {
            Text = "Alt + Backspace: Toggle Auto-Clicker\nAlt + P: Update Click Position\nAlt + Minus: Show/Hide Window",
            Location = new Point(20, 170),
            Size = new Size(360, 60),
            Font = new Font("Segoe UI", 9F)
        };

        var creditLabel = new Label
        {
            Text = "Based on NIAutoclicker concept",
            Location = new Point(20, 245),
            Size = new Size(360, 20),
            Font = new Font("Segoe UI", 8F, FontStyle.Italic),
            ForeColor = Color.Gray
        };

        this.Controls.AddRange(new Control[]
        {
            statusLabel,
            intervalLabel,
            intervalNumeric,
            buttonLabel,
            buttonComboBox,
            hotkeysLabel,
            hotkeyInfo,
            creditLabel
        });

        this.ResumeLayout(false);
    }

    private void InitializeTrayIcon()
    {
        // Load the base icon from file
        var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "favicon.ico");
        if (File.Exists(iconPath))
        {
            _baseIcon = new Icon(iconPath);
            this.Icon = _baseIcon;
        }

        _trayIcon = new NotifyIcon
        {
            Text = "Auto Clicker - Stopped",
            Visible = true
        };

        _trayIcon.Icon = CreateTrayIcon(Color.Red);

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Show", null, (s, e) =>
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        });
        contextMenu.Items.Add("Toggle (Alt+Backspace)", null, (s, e) =>
        {
            if (_engine.IsRunning)
                _engine.Stop();
            else
                _engine.Start();
            UpdateUI();
        });
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, (s, e) =>
        {
            _trayIcon.Visible = false;
            Application.Exit();
        });

        _trayIcon.ContextMenuStrip = contextMenu;
        _trayIcon.DoubleClick += (s, e) =>
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        };
    }

    private Icon CreateTrayIcon(Color statusColor)
    {
        // If we have a base icon, overlay status indicator on it
        if (_baseIcon != null)
        {
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.DrawIcon(_baseIcon, 0, 0);

                // Draw small status indicator in bottom-right corner
                using (var brush = new SolidBrush(statusColor))
                {
                    g.FillEllipse(brush, 10, 10, 6, 6);
                }
                using (var pen = new Pen(Color.White, 1))
                {
                    g.DrawEllipse(pen, 10, 10, 6, 6);
                }
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }
        else
        {
            // Fallback to simple colored dot
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(statusColor))
                {
                    g.FillEllipse(brush, 2, 2, 12, 12);
                }
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }
    }

    private void OnGlobalKeyPressed(object? sender, KeyEventArgs e)
    {
        if (e.Alt && e.KeyCode == Keys.Back)
        {
            if (_engine.IsRunning)
            {
                _engine.Stop();
            }
            else
            {
                _engine.Start();
            }
            UpdateUI();
            e.Handled = true;
        }
        else if (e.Alt && e.KeyCode == Keys.P)
        {
            _engine.UpdateLockedPosition();
            e.Handled = true;
        }
        else if (e.Alt && e.KeyCode == Keys.OemMinus)
        {
            if (this.Visible)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.Activate();
            }
            e.Handled = true;
        }
    }

    private void UpdateTimer_Tick(object? sender, EventArgs e)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        var statusLabel = this.Controls.Find("statusLabel", false).FirstOrDefault() as Label;
        if (statusLabel != null)
        {
            statusLabel.Text = _engine.IsRunning ? "Status: RUNNING" : "Status: Stopped";
            statusLabel.ForeColor = _engine.IsRunning ? Color.Green : Color.Red;
        }

        if (_trayIcon != null)
        {
            _trayIcon.Text = _engine.IsRunning ? "Auto Clicker - RUNNING" : "Auto Clicker - Stopped";
            _trayIcon.Icon = CreateTrayIcon(_engine.IsRunning ? Color.Green : Color.Red);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (this.WindowState == FormWindowState.Minimized)
        {
            this.Hide();
            this.ShowInTaskbar = false;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            this.Hide();
            this.ShowInTaskbar = false;
            return;
        }

        _updateTimer.Stop();
        _engine.Dispose();
        _keyboardHook.Dispose();
        _trayIcon?.Dispose();
        base.OnFormClosing(e);
    }
}
