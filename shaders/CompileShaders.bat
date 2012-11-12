@ECHO OFF

CLS

ECHO Compiling shaders...
ECHO.

CALL CompileShader.bat FxDis test test ps_4_0 main || GOTO :error

CALL CompileShader.bat HlslCrossCompiler/ds5 basic basic ds_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/hs5 basic basic hs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps4 fxaa fxaa ps_4_0 main "/DFXAA_HLSL_4=1" || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps4 primID primID ps_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 conservative_depth conservative_depth_ge ps_5_0 DepthGreaterThan || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 conservative_depth conservative_depth_le ps_5_0 DepthLessThan || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 interface_arrays interface_arrays ps_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 interfaces interfaces ps_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 sample sample ps_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs4 mov mov vs_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs4 multiple_const_buffers multiple_const_buffers vs_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs4 switch switch vs_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 any any vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 const_temp const_temp vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 mad_imm mad_imm vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 mov mov vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 sincos sincos vs_5_0 main || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_EdgeFactorCS.hlsl TessellatorCS40_EdgeFactorCS cs_4_0 CSEdgeFactor || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_NumVerticesIndicesCS.hlsl TessellatorCS40_NumVerticesIndicesCS cs_4_0 CSNumVerticesIndices || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_ScatterIDCS.hlsl TessellatorCS40_ScatterIDCS_Vertex cs_4_0 CSScatterVertexTriIDIndexID || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_ScatterIDCS.hlsl TessellatorCS40_ScatterIDCS_Index cs_4_0 CSScatterIndexTriIDIndexID || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_TessellateIndicesCS.hlsl TessellatorCS40_TessellateIndicesCS cs_4_0 CSTessellationIndices || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_TessellateVerticesCS.hlsl TessellatorCS40_TessellateVerticesCS cs_4_0 CSTessellationVertices || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Raw cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Raw_Double cs_5_0 CSMain "/DTEST_DOUBLE=1" || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Structured cs_5_0 CSMain "/DUSE_STRUCTURED_BUFFERS=1" || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Structured_Double cs_5_0 CSMain "/DUSE_STRUCTURED_BUFFERS=1" "/DTEST_DOUBLE=1" || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/BasicHLSL11 BasicHLSL.fx BasicHLSL_PS ps_4_0 RenderScenePS /Gec || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicHLSL11 BasicHLSL.fx BasicHLSL_VS vs_4_0 RenderSceneVS /Gec || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC6HDecode.hlsl BC6HDecode cs_4_0 main || GOTO :error
REM CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC6HEncode.hlsl BC6HEncode_G10 cs_4_0 TryModeG10CS || GOTO :error
REM CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC6HEncode.hlsl BC6HEncode_LE10 cs_4_0 TryModeLE10CS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Decode.hlsl BC7Decode cs_4_0 main || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Encode.hlsl BC7Encode_456 cs_4_0 TryMode456CS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Encode.hlsl BC7Encode_137 cs_4_0 TryMode137CS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Encode.hlsl BC7Encode_02 cs_4_0 TryMode02CS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DynamicShaderLinkage11 DynamicShaderLinkage11_PS.hlsl DynamicShaderLinkage11_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DynamicShaderLinkage11 DynamicShaderLinkage11_VS.hlsl DynamicShaderLinkage11_VS vs_5_0 VSMain || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 NBodyGravityCS11.hlsl NBodyGravityCS11 cs_4_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_GS gs_4_0 GSParticleDraw || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_PS ps_4_0 PSParticleDraw || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_VS vs_4_0 VSParticleDraw || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_DS ds_5_0 BezierDS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_HS hs_5_0 BezierHS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_PS ps_4_0 BezierPS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_VS vs_4_0 BezierVS || GOTO :error

GOTO :EOF

:error
ECHO Failed with error #%errorlevel%.
EXIT /b %errorlevel%