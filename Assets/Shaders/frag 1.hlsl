void wave2_float(UnityTexture2D _AudioTex, float2 uv, float testrange, float _BaseLine, float _InnerBorder, out float3 OUT)
{
    float sdf = length(uv);
    float x = uv.y;
    float y = uv.x;

    float fft =  SAMPLE_TEXTURE2D(_AudioTex.tex, _AudioTex.samplerstate, float2(x, 0.0)).r * testrange;


    float edge = _BaseLine + fft;// + sin(angle);
    float v = step(y,edge) * smoothstep(_BaseLine - _InnerBorder,_BaseLine,y);

    OUT = float3(0, 222, 250) * v;

}