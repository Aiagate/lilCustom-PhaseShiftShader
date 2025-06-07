
void PS_GetMirageColloidCol(inout lilFragData fd LIL_SAMP_IN_FUNC(samp)) {
    // Dissolve計算
    float dissolveAlpha = 1.0;
    float dissolveMaskVal = 1.0;
    float dissolveNoise = 1.0;
    dissolveMaskVal = LIL_SAMPLE_2D(_PS_MirageColloidDissolveMask, samp, lilCalcUV(fd.uvMain, _PS_MirageColloidDissolveMask_ST)).r;
    dissolveNoise = LIL_SAMPLE_2D(_PS_MirageColloidDissolveNoiseMask, samp, lilCalcUV(fd.uvMain, _PS_MirageColloidDissolveNoiseMask_ST, _PS_MirageColloidDissolveNoiseMaskAnim.xy)).r - 0.5;
    dissolveNoise *= _PS_MirageColloidDissolveNoiseMaskStrength;
    
    dissolveAlpha = 1.0 - saturate(abs(dissolveMaskVal + dissolveNoise - _PS_MirageColloidDissolveBorder) / _PS_MirageColloidDissolveBlur);
    
    // Rim計算
    float rim = pow(saturate(1.0 - fd.nvabs), _RimFresnelPower);
    rim = lilTooningScale(_AAStrength, rim, _RimBorder, _RimBlur);
    rim = lerp(rim, rim * fd.shadowmix, _RimShadowMask);

    // バックグラウンドテクスチャを取得
    float4 bgtex;
    bgtex = LIL_GET_BG_TEX(fd.uvScn + (fd.N * rim * 0.03), 0);

    // Rimを用いてコントラストを変更
    bgtex.rgb += rim * fd.triMask.g * bgtex.rgb;
    bgtex.rgb = lilToneCorrection(bgtex.rgb, _PS_MirageColloidHSVG);
    
    fd.col.rgb = dissolveMaskVal + dissolveNoise > _PS_MirageColloidDissolveBorder ? bgtex.rgb : fd.col.rgb;
    fd.emissionColor += _PS_MirageColloidDissolveColor.rgb * dissolveAlpha;
}
