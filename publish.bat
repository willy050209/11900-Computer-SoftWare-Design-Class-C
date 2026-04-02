@echo off
chcp 65001 > nul
SETLOCAL EnableExtensions EnableDelayedExpansion

SET "DIST_DIR=dist"
SET "TOOLS_DIR=%DIST_DIR%\tools"

echo [1/4] 清除舊的發佈目錄...
if exist "%DIST_DIR%" rd /s /q "%DIST_DIR%"
mkdir "%TOOLS_DIR%"

echo [2/4] 正在發佈 Task1Tester...
dotnet publish "Test\Task1Tester\Task1Tester\Task1Tester.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o "%TOOLS_DIR%"

echo [3/4] 正在發佈 WinFormUITester...
dotnet publish "Test\WinFormUITester\WinFormUITester.csproj" -c Release -o "%TOOLS_DIR%"

echo [4/4] 正在發佈 TestLauncher 主程式...
dotnet publish "Test\TestLauncher\TestLauncher.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o "%DIST_DIR%"

echo.
echo ==========================================
echo 發佈完成！檔案已存放於 "%DIST_DIR%"。
echo ==========================================
pause
