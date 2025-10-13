# osu-chat-notifier

An osu!lazer custom ruleset that sends you notifications at startup if you have unread private messages!

## Install

### Installation script (recommended)

Download and run [`installer.bat` (Windows)](https://github.com/fuyukiSmkw/osu-chat-notifier/releases/latest/download/installer.bat) or [`installer.sh` (Linux/Mac)](https://github.com/fuyukiSmkw/osu-chat-notifier/releases/latest/download/installer.sh).

### Manual install

Go to Releases, download the dll file, and place it in the `rulesets` subfolder of your osu!lazer directory.

* On Windows, this is usually located at `%APPDATA%\osu\rulesets`
* On Linux, it's usually at `~/.local/share/osu/rulesets`

## Build

Requires .NET `9.0` or higher.

```bash
dotnet build -c Release
```

Remove `-c Release` during development.

## Acknowledgements

* [ppy/osu](https://github.com/ppy/osu): The official osu!lazer project
* [MATRIX-feather/LLin](https://github.com/MATRIX-feather/LLin): A custom ruleset that provides beatmap downloads from a mirror site and includes a music player
