@echo off

rem Wrapper script for nant packaging targets

setlocal

set NANT=tools\NAnt-0.90\bin\nant.exe
set OPTIONS=-f:scripts/nunit.package.targets
set CONFIG=
set TARGET=
set COMMANDS=
goto start

:shift
shift /1

:start

IF /I "%1" EQU "?"	goto usage
IF /I "%1" EQU "/h"	goto usage
IF /I "%1" EQU "/help"	goto usage

IF /I "%1" EQU "debug"	set CONFIG=debug&goto shift
IF /I "%1" EQU "release" set CONFIG=release&goto shift

IF /I "%1" EQU "net"	 set TARGET=net&goto shift
IF /I "%1" EQU "net-1.0" set TARGET=net-1.0&goto shift
IF /I "%1" EQU "net-1.1" set TARGET=net-1.1&goto shift
IF /I "%1" EQU "net-2.0" set TARGET=net-2.0&goto shift
IF /I "%1" EQU "net-3.0" set TARGET=net-3.0&goto shift
IF /I "%1" EQU "net-3.5" set TARGET=net-3.5&goto shift
IF /I "%1" EQU "net-4.0" set TARGET=net-4.0&goto shift

IF /I "%1" EQU "mono"	  set TARGET=mono&goto shift
IF /I "%1" EQU "mono-1.0" set TARGET=mono-1.0&goto shift
IF /I "%1" EQU "mono-2.0" set TARGET=mono-2.0&goto shift
IF /I "%1" EQU "mono-3.5" set TARGET=mono-3.5&goto shift
IF /I "%1" EQU "mono-4.0" set TARGET=mono-4.0&goto shift

IF /I "%1" EQU "all"	set COMMANDS=%COMMANDS% package-all&goto shift
IF /I "%1" EQU "docs"	set COMMANDS=%COMMANDS% package-docs&goto shift
IF /I "%1" EQU "source"	set COMMANS=%COMMANDS% package-src&goto shift
IF /I "%1" EQU "src"	set COMMANS=%COMMANDS% package-src&goto shift

IF "%1" NEQ "" set COMMANDS=%COMMANDS% %1&goto shift

IF "%CONFIG%" EQU "" set CONFIG=release
set OPTIONS=%OPTIONS% -D:build.config=%CONFIG%

IF "%TARGET%" NEQ "" set OPTIONS=%OPTIONS% -D:runtime.config=%TARGET%

if "%COMMANDS%" EQU "" set COMMANDS=package

%NANT% %OPTIONS% %COMMANDS%

goto done

:usage

echo Builds one or more NUnit packages for distribution
echo.
echo usage: PACKAGE [option [...] ]
echo.
echo Options may be any of the following, in any order...
echo.
echo   release        Builds packages for release (default)
echo   debug          Builds debug packages
echo.
echo   net-4.0        Builds packages for .NET 4.0 (future)
echo   net-2.0        Builds packages for .NET 2.0 (default)
echo   net-1.1        Builds packages for .NET 1.1
echo   net-1.0        Builds packages for .NET 1.0
echo.
echo   src, source    Builds the source package
echo   docs           Builds the documentation package
echo   all            Builds all packages for all platforms
echo.
echo   ?, /h, /help   Displays this help message
echo.
echo In addition, any valid target in the NAnt script may
echo be supplied as an argument. This requires some degree
echo of familiarity with the script, in order to avoid
echo use of incompatible options.
echo.   

:done
