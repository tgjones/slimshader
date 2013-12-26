@ECHO OFF

"C:\Program Files (x86)\Windows Kits\8.0\bin\x86\fxc" %1/%2 /Fo %1/%3.o /Fc %1/%3.asm /T %4 /E %5 /nologo %6 %7

IF %errorlevel% NEQ 0 EXIT /b %errorlevel%