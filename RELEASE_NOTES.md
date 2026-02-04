# Auto Clicker v1.0.0

A non-intrusive auto-clicker for Windows that allows you to automate mouse clicks while maintaining full control of your cursor.

## 🎯 Features

- **Non-Intrusive Design**: Clicks at a locked position in the background while you maintain full control of your mouse
- **Global Hotkeys**: Control the auto-clicker from anywhere without focusing the window
  - `Alt + Backspace`: Toggle auto-clicker on/off
  - `Alt + P`: Update the locked click position
  - `Alt + Minus`: Show/hide the application window
- **System Tray Integration**: Minimizes to system tray with color-coded status indicator
  - Red dot = Stopped
  - Green dot = Running
- **Customizable Settings**:
  - Adjustable click interval (minimum 50ms for stable operation)
  - Choose between Left or Right mouse button
- **Position Locking**: Locks the click position so you can move your mouse freely while clicks continue at the target location

## 📦 Installation

1. Download `AutoClicker-v1.0.0.zip` from the assets below
2. Extract the ZIP file to a folder of your choice
3. Run `AutoClicker.exe` **as Administrator** (required for global hotkeys)

## ⚙️ Requirements

- Windows OS (Windows 10 or later recommended)
- .NET 10.0 Runtime or higher ([Download here](https://dotnet.microsoft.com/download/dotnet/10.0))
- Administrator privileges (for global keyboard hooks)

## 🚀 Quick Start

1. Launch the application as Administrator
2. Configure your desired click interval and mouse button in the settings window
3. Position your mouse where you want the clicks to occur
4. Press `Alt + Backspace` to start clicking
5. Move your mouse freely - clicks will continue at the locked position
6. Press `Alt + Backspace` again to stop
7. The application minimizes to the system tray - right-click the tray icon for options

## 💡 How It Works

The auto-clicker captures your mouse position when started, then repeatedly:
1. Saves your current cursor position
2. Moves cursor to the locked click position
3. Performs the click
4. Returns cursor back to where you moved it

This happens fast enough that you maintain full mouse control while automated clicks occur at the target location.

## 📝 Notes

- The application must run as Administrator for global hotkeys to work properly
- Minimum click interval is 50ms to ensure stable operation
- The application minimizes to the system tray - right-click the tray icon for options
- Closing the window minimizes to tray - use "Exit" from tray menu to quit
- The cursor will briefly jump to the click position and back - this is normal behavior

## ⚠️ Safety & Responsible Use

This tool is intended for **legitimate gaming and automation purposes only**.

Be aware that using auto-clickers may violate the terms of service for some games or applications. Always check the rules and policies of the software you're using this with. Use responsibly and ethically.

## 🐛 Known Issues

None at this time. Please report any issues on the [GitHub Issues page](https://github.com/YOUR_USERNAME/AutoClicker/issues).

## 📄 License

This software is licensed under the Apache License 2.0. See the LICENSE file for details.

## 🙏 Acknowledgments

Based on the [NIAutoclicker](https://github.com/Shadowspaz/NIAutoclicker) concept by Shadowspaz.

---

**Installation Support**: If you encounter issues running the application, ensure you have the .NET 10.0 Runtime installed and are running as Administrator.
