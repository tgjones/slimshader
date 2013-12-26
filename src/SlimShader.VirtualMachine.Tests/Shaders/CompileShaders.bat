@ECHO OFF

CLS

ECHO Compiling shaders...
ECHO.

CALL CompileShader.bat GS CubeMap.hlsl GS_CubeMap_GS gs_4_0 GS_CubeMap || GOTO :error

GOTO :EOF

:error
ECHO Failed with error #%errorlevel%.
EXIT /b %errorlevel%