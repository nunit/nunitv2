set NANT=tools\NAnt-0.90\bin\nant.exe
set OPTIONS=-f:scripts/nunit.package.targets
set CONFIG=
set TARGET=
set COMMANDS=
goto start

:shift
shift /1

:start
IF /I "%1" EQU "debug" set CONFIG=debug&goto shift
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

if "%CONFIG%" NEQ "" set OPTIONS=%OPTIONS% -D:build.config=%CONFIG%
if "%TARGET%" NEQ "" set OPTIONS=%OPTIONS% -D:runtime.config=%TARGET%

if "%COMMANDS%" EQU "" set COMMANDS=package

%NANT% %OPTIONS% %COMMANDS%
