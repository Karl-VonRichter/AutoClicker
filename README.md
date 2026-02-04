# Non-Intrusive Auto Clicker

A C# implementation of a non-intrusive auto-clicker based on NIAutoclicker, designed for gaming and automation tasks.

## Features

- **Non-Intrusive Design**: Clicks at a locked position in the background while you maintain full control of your mouse
- **Global Hotkeys**: Control the auto-clicker from anywhere without focusing the window
- **System Tray Integration**: Minimizes to system tray with color-coded status indicator (Red = Stopped, Green = Running)
- **Customizable Settings**: Adjust click interval (minimum 50ms) and mouse button
- **Position Locking**: Locks the click position so you can move your mouse freely
- **Minimal UI**: Simple, clean interface with real-time status updates

## How It Works

The auto-clicker works by:
1. Capturing your current mouse position when started
2. Clicking at that locked position in the background
3. Immediately returning your cursor to where you moved it
4. This allows you to continue using your mouse normally while clicks happen at the target location

## Hotkeys

- **Alt + Backspace**: Toggle the auto-clicker on/off
- **Alt + P**: Update the locked click position to current mouse location
- **Alt + Minus**: Show/hide the application window

## Usage

1. **Build the application**:
   ```bash
   dotnet build
   ```

2. **Run as Administrator** (required for global hotkeys):
   ```bash
   dotnet run
   ```

3. **Configure settings**:
   - Set your desired click interval (in milliseconds)
   - Choose Left or Right mouse button

4. **Position your mouse** where you want clicks to occur

5. **Press Alt + Backspace** to start clicking

6. **Move your mouse freely** - clicks will continue at the locked position

7. **Press Alt + Backspace** again to stop

## Requirements

- Windows OS
- .NET 10.0 or higher
- Administrator privileges (for global keyboard hooks)

## Building

### Local Build

```bash
dotnet build -c Release
```

The executable will be in `bin/Release/net10.0-windows/`

### Creating a Release

To create a new release with GitHub Actions:

1. Tag your commit with a version number:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. GitHub Actions will automatically:
   - Build the project in Release mode
   - Create a ZIP archive with all necessary files
   - Create a GitHub release with the build artifacts
   - Add release notes

The release will include the executable and the favicon.ico file.

## Notes

- The application must run as Administrator for global hotkeys to work properly
- Minimum click interval is 50ms to ensure stable operation
- The application minimizes to the system tray - right-click the tray icon for options
- Tray icon color indicates status: Red = Stopped, Green = Running
- Closing the window minimizes to tray - use "Exit" from tray menu to quit
- The cursor will briefly jump to the click position and back - this is normal behavior
- Based on the NIAutoclicker concept by Shadowspaz

## Safety

This tool is intended for legitimate gaming and automation purposes. Be aware that using auto-clickers may violate terms of service for some games or applications. Use responsibly.
