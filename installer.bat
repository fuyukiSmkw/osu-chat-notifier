@echo off
setlocal EnableDelayedExpansion

:: ===== Configuration =====
set "RULESET_URL=https://github.com/fuyukiSmkw/osu-chat-notifier/releases/latest/download/osu.Game.Rulesets.ChatNotifier.dll"
set "RULESET_NAME=osu.Game.Rulesets.ChatNotifier.dll"

echo.
echo ============================================
echo osu!lazer Chat Notifier Installer
echo ============================================
echo.

:: ===== Find osu! folder =====
set "INI_FILE=%AppData%\osu\storage.ini"
if exist "%INI_FILE%" (
    for /f "usebackq tokens=1,* delims==" %%A in (`findstr /i "^FullPath" "%INI_FILE%"`) do (
        set "VAL=%%B"
        set "VAL=!VAL:"=!" & REM remove quotes
        for /f "tokens=* delims= " %%C in ("!VAL!") do set "OSU_DATA_PATH=%%C"
    )
    if defined OSU_DATA_PATH (
        echo Found migrated osu folder path: !OSU_DATA_PATH!
    )
)
if not defined OSU_DATA_PATH (
    set "OSU_DATA_PATH=%AppData%\osu"
    if not exist "!OSU_DATA_PATH!" (
        echo [Error] Folder !OSU_DATA_PATH! does not exist
        echo Install osu lazer first.
        pause>nul|set/p =Press any key to quit...
        exit /b 1
    )
    echo Using default osu folder path: !OSU_DATA_PATH!
)

:: ===== Check rulesets folder =====
set "RULESET_DIR=%OSU_DATA_PATH%\rulesets"
if not exist "%RULESET_DIR%" (
    echo The rulesets folder does not exist. Creating it now...
    mkdir "%RULESET_DIR%"
    if %errorlevel% neq 0 (
        echo [Error] Failed to create the folder: %RULESET_DIR%
        echo Try running this script as Administrator or adjust permissions.
        pause>nul|set/p =Press any key to quit...
        exit /b 1
    )
)
echo Using rulesets folder: %RULESET_DIR%

:: ===== Download the DLL to target folder =====
echo.
echo Downloading %RULESET_NAME% to %RULESET_DIR% ...

set pwshCmd=try {^
    Invoke-WebRequest "%RULESET_URL%" -OutFile "%RULESET_DIR%\%RULESET_NAME%" -UseBasicParsing -ErrorAction Stop; ^
    exit 0; ^
} catch [System.UnauthorizedAccessException] { ^
    exit 72; ^
} catch { ^
    exit 7; ^
}

powershell -Command "%pwshCmd%"
set "PS_RESULT=%errorlevel%"
if %PS_RESULT%==0 (
    echo Download succeeded.
) else (
    if exist "%RULESET_DIR%\%RULESET_NAME%" (
        del "%RULESET_DIR%\%RULESET_NAME%"
    )
    if %PS_RESULT%==72 (
        echo [Error] Access denied when writing to folder: %RULESET_DIR%
        echo Try running this script as Administrator or adjust permissions.
        pause>nul|set/p =Press any key to quit...
        exit /b 1
    ) else if %PS_RESULT%==7 (
        echo [Error] Download failed. Please check your internet connection.
        pause>nul|set/p =Press any key to quit...
        exit /b 1
    ) else (
        echo [Error] Unknown error occurred, code %PS_RESULT%.
        pause>nul|set/p =Press any key to quit...
        exit /b 1
    )
)

:: ===== Success =====
echo.
echo Installation complete.
echo Installed to: %RULESET_DIR%\%RULESET_NAME%
echo.
echo You can close this window now.
pause>nul|set/p =Press any key to quit...
exit /b 0
