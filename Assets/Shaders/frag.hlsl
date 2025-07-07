void wave_float(UnityTexture2D _AudioTex, float2 uv, float time,float testrange, float sinfreq, float cosfreq, out float3 OUT)
{
    const float CNT = 8.0;

    for(float i =0.0; i < CNT;i += 1.0)
    {
        float fft =  SAMPLE_TEXTURE2D(_AudioTex.tex, _AudioTex.samplerstate, float2(i / CNT ,0.0)).r;
        
        fft *= testrange;

        if(fft < 0.5)
        {
            fft += 0.5;
        }

        float x = uv.y;

        float sinv = sin(x * sinfreq + time + (i+1.0) / CNT *2.0);
        float cosv = cos(x * cosfreq);
        float v = sinv * cosv * fft;

        float factor = (i + 1.0) / CNT;
        v = v * factor + 0.5;

        float t = 1.0 - smoothstep(0.0,0.02,abs(uv.x-v));

        OUT += float3(i / 5.0,0.5,1.75) * t * 3.0 / CNT;
    }
}