# DesktopPrank (WinForms)

Humorous Windows desktop "cleaner" that tucks your desktop icons behind its own window.

## Build and run (Windows 10/11)

```bash
dotnet restore
dotnet build DesktopPrank/DesktopPrank.csproj
dotnet run --project DesktopPrank/DesktopPrank.csproj
```

## How it works (Win32 API)

The app locates the desktop icon list view (`SysListView32`) by walking the `Progman`/`WorkerW` window tree, then uses `SendMessage` with `LVM_SETITEMPOSITION32` to move every desktop icon into the screen-space bounds of the app window. During window moves (`WM_MOVING`), it recalculates the new bounds and repositions all icons so they remain hidden behind the app as you drag it around.