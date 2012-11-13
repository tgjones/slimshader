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

CALL CompileShader.bat Sdk/Direct3D10/CubeMapGS CubeMapGS.fx GS_CubeMap_VS vs_4_0 VS_CubeMap || GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/CubeMapGS CubeMapGS.fx GS_CubeMap_GS gs_4_0 GS_CubeMap || GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/CubeMapGS CubeMapGS.fx GS_CubeMap_PS ps_4_0 PS_EnvMappedScene || GOTO :error

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

CALL CompileShader.bat Sdk/Direct3D11/CascadedShadowMaps11 RenderCascadeScene.hlsl RenderCascadeScene_PS ps_4_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/CascadedShadowMaps11 RenderCascadeScene.hlsl RenderCascadeScene_VS vs_4_0 VSMain || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/ComputeShaderSort11 ComputeShaderSort11.hlsl ComputeShaderSort11_BitonicSort cs_5_0 BitonicSort || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/ComputeShaderSort11 ComputeShaderSort11.hlsl ComputeShaderSort11_MatrixTranspose cs_5_0 MatrixTranspose || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/ContactHardeningShadows11 ContactHardeningShadows11.hlsl ContactHardeningShadows11_PS ps_5_0 PS_RenderScene || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/ContactHardeningShadows11 ContactHardeningShadows11.hlsl ContactHardeningShadows11_VSSM vs_4_0 VS_RenderSceneSM || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/ContactHardeningShadows11 ContactHardeningShadows11.hlsl ContactHardeningShadows11_VS vs_4_0 VS_RenderScene || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DDSWithoutD3DX11 DDSWithoutD3DX.hlsl DDSWithoutD3DX_VS vs_4_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DDSWithoutD3DX11 DDSWithoutD3DX.hlsl DDSWithoutD3DX_PS ps_4_0 RenderScenePS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_VS_NoTessellation vs_4_0 VS_NoTessellation || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_VS vs_4_0 VS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_HS hs_5_0 HS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_DS ds_5_0 DS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_PS ps_5_0 PS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_VS_NoTessellation vs_4_0 VS_NoTessellation || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_VS vs_4_0 VS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_HS hs_5_0 HS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_DS ds_5_0 DS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_PS ps_4_0 BumpMapPS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 Particle.hlsl Particle_VS vs_5_0 VSPassThrough || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 Particle.hlsl Particle_GS gs_5_0 GSPointSprite || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 Particle.hlsl Particle_PS ps_5_0 PSConstantColor || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 POM.hlsl POM_VS vs_5_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 POM.hlsl POM_PS ps_4_0 RenderScenePS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DynamicShaderLinkage11 DynamicShaderLinkage11_PS.hlsl DynamicShaderLinkage11_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DynamicShaderLinkage11 DynamicShaderLinkage11_VS.hlsl DynamicShaderLinkage11_VS vs_5_0 VSMain || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_BuildGridCS cs_5_0 BuildGridCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ClearGridIndicesCS cs_5_0 ClearGridIndicesCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_BuildGridIndicesCS cs_5_0 BuildGridIndicesCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_RearrangeParticlesCS cs_5_0 RearrangeParticlesCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_DensityCS_Simple cs_5_0 DensityCS_Simple || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_DensityCS_Shared cs_5_0 DensityCS_Shared || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_DensityCS_Grid cs_5_0 DensityCS_Grid || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ForceCS_Simple cs_5_0 ForceCS_Simple || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ForceCS_Shared cs_5_0 ForceCS_Shared || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ForceCS_Grid cs_5_0 ForceCS_Grid || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_IntegrateCS cs_5_0 IntegrateCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidRender.hlsl FluidRender_VS vs_5_0 ParticleVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidRender.hlsl FluidRender_GS gs_5_0 ParticleGS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidRender.hlsl FluidRender_PS ps_5_0 ParticlePS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 BrightPassAndHorizFilterCS.hlsl BrightPassAndHorizFilterCS cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 DumpToTexture.hlsl DumpToTexture ps_5_0 PSDump || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FilterCS.hlsl FilterCS_Vertical cs_5_0 CSVerticalFilter || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FilterCS.hlsl FilterCS_Horizontal cs_5_0 CSHorizFilter || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FinalPass.hlsl FinalPass_VS vs_5_0 QuadVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FinalPass.hlsl FinalPass_PS ps_5_0 PSFinalPass || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FinalPass.hlsl FinalPass_PS_CPUReduction ps_5_0 PSFinalPassForCPUReduction || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 PSApproach.hlsl PSApproach_PS ps_5_0 DownScale3x3_BrightPass || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 ReduceTo1DCS.hlsl ReduceTo1DCS cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 ReduceToSingleCS.hlsl ReduceToSingleCS cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 skybox11.hlsl Skybox11_VS vs_5_0 SkyboxVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 skybox11.hlsl Skybox11_PS ps_5_0 SkyboxPS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 NBodyGravityCS11.hlsl NBodyGravityCS11 cs_4_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_GS gs_4_0 GSParticleDraw || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_PS ps_4_0 PSParticleDraw || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_VS vs_4_0 VSParticleDraw || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_VS vs_5_0 VS_RenderScene || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_VS_WithTessellation vs_5_0 VS_RenderSceneWithTessellation || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_HS hs_5_0 HS_PNTriangles || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_DS ds_5_0 DS_PNTriangles || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_PS ps_4_0 PS_RenderScene || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_DS ds_5_0 BezierDS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_HS hs_5_0 BezierHS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_PS ps_4_0 BezierPS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_VS vs_4_0 BezierVS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_VS_PatchSkinning vs_5_0 PatchSkinningVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_VS_MeshSkinning vs_5_0 MeshSkinningVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_HS hs_5_0 SubDToBezierHS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_HS_4444 hs_5_0 SubDToBezierHS4444 || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_DS ds_5_0 BezierEvalDS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_PS ps_5_0 SmoothPS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 2DQuadShaders.hlsl 2DQuadShaders_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 2DQuadShaders.hlsl 2DQuadShaders_PS_BlurX ps_5_0 PSBlurX || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 2DQuadShaders.hlsl 2DQuadShaders_PS_BlurY ps_5_0 PSBlurY || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceScene.hlsl RenderVarianceScene_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceScene.hlsl RenderVarianceScene_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceShadow.hlsl RenderVarianceShadow_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceShadow.hlsl RenderVarianceShadow_PS ps_5_0 PSMain || GOTO :error

GOTO :EOF

:error
ECHO Failed with error #%errorlevel%.
EXIT /b %errorlevel%