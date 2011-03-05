@echo off

rem Wrapper script for nant build targets

setlocal

set NANT=tools\NAnt-0.90\bin\nant.exe
set OPTIONS=-f:scripts\nunit.build.targets
set CONFIG=
set TARGET=
set CLEAN=
set COMMANDS=
goto start

:shift
shift /1

:start

IF /I "%1" EQU "?"	goto usage
IF /I "%1" EQU "/h"	goto usage
IF /I "%1" EQU "/help"	goto usage

IF /I "%1" EQU "debug" set CONFIG=debug&goto shift
IF /I "%1" EQU "release" set CONFIG=release&goto shift

IF /I "%1" EQU "net" set TARGET=net&goto shift
IF /I "%1" EQU "net-1.0" set TARGET=net-1.0&goto shift
IF /I "%1" EQU "net-1.1" set TARGET=net-1.1&goto shift
IF /I "%1" EQU "net-2.0" set TARGET=net-2.0&goto shift
IF /I "%1" EQU "net-3.0" set TARGET=net-3.0&goto shift
IF /I "%1" EQU "net-3.5" set TARGET=net-3.5&goto shift
IF /I "%1" EQU "net-4.0" set TARGET=net-4.0&goto shift

IF /I "%1" EQU "mono" set TARGET=mono&goto shift
IF /I "%1" EQU "mono-1.0" set TARGET=mono-1.0&goto shift
IF /I "%1" EQU "mono-2.0" set TARGET=mono-2.0&goto shift
IF /I "%1" EQU "mono-3.5" set TARGET=mono-3.5&goto shift
IF /I "%1" EQU "mono-4.0" set TARGET=mono-4.0&goto shift

if "%1" EQU "clean" set CLEAN=clean&goto shift
IF "%1" EQU "samples" set COMMANDS=%COMMANDS% build-samples&goto shift
IF "%1" EQU "tools" set COMMANDS=%COMMANDS% build-tools&goto shift

IF "%1" NEQ "" set COMMANDS=%COMMANDS% %1&goto shift

if "%CONFIG%" NEQ "" set OPTIONS=%OPTIONS% -D:build.config=%CONFIG%
if "%TARGET%" NEQ "" set OPTIONS=%OPTIONS% -D:runtime.config=%TARGET%

if "%COMMANDS%" EQU "" set COMMANDS=build

%NANT% %OPTIONS% %CLEAN% %COMMANDS%

goto done

: usage

echo Builds and tests NUnit for various targets
echo.
echo usage: BUILD [option [...] ]
echo.
echo Options may be any of the following, in any order...
echo.
echo   debug          Builds debug configuration (default)
echo   release        Builds release configuration
echo.
echo   net-4.0        Targets .NET 4.0 (future)
echo   net-2.0        Targets .NET 2.0 (default)
echo   net-1.1        Targets .NET 1.1
echo   net-1.0        Targets .NET 1.0
echo.
echo   clean          Cleans the output directory before building
echo.
echo   samples        Builds the NUnit samples
echo   tools          Builds the NUnit tools
echo.
echo   test           Runs tests for a build using the console runner
echo   gui-test       Runs tests for a build using the NUnit gui
echo.
echo   ?, /h, /help   Displays this help message
echo.
echo In addition, any valid target in the NAnt script may
echo be supplied as an argument. This requires some degree
echo of familiarity with the script, in order to avoid
echo use of incompatible options.
echo.   

: done
