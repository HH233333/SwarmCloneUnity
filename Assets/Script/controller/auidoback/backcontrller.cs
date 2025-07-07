using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backcontrller : MonoBehaviour
{
    public AudioPeer audioPeer;
    // public float multiplier = 0.2f;
    public int ringwildmax;
    // public Vector3 bumpDimensions = new Vector3(0, 1, 0);

    // private Vector3 baseScale;
    private Material material;
    private Texture2D _audiotexture;
    private void Start()
    {
        // baseScale = transform.localScale;
        material = transform.GetComponent<MeshRenderer>().materials[0];
        _audiotexture = new Texture2D(512, 1);
        _audiotexture.filterMode = FilterMode.Point;
        Color[] backPixels = new Color[_audiotexture.width * _audiotexture.height];
        for (int i = 0; i < backPixels.Length; i++)
        {
            var col = Color.black;
            if (i == 0 || i == backPixels.Length - 1)
                col.r = 1;
            backPixels[i] = col;
        }
        _audiotexture.SetPixels(backPixels);
        _audiotexture.Apply();

        material.SetTexture(Shader.PropertyToID("_AudioTex"), _audiotexture);
    }

    void Update()
    {
        if (audioPeer == null)
            return;
        if (audioPeer.singing)
        {
            var samples = audioPeer.GetGetAudioBands();
            var lowbandringwild = (int)(average(0, 3, samples) / 1 * ringwildmax);
            var highbandringwild = (int)(average(4, 7, samples)/ 1 * ringwildmax);
            Debug.Log(highbandringwild);
            if (lowbandringwild > 1)
            {
                for (int i = 0; i < ringwildmax; i++)
                {
                    var col = Color.black;
                    col.r = 1 - i / lowbandringwild;
                    _audiotexture.SetPixel(i, 0, col);
                }
            }

            if (highbandringwild > 1)
            {
                for (int i = 511; i > 511 - ringwildmax; i--)
                {
                    var col = Color.black;
                    col.r = 1 - (512-i) / highbandringwild;
                    _audiotexture.SetPixel(i, 0, col);
                }
            }
            _audiotexture.Apply();
        }
        // float audioBand = audioPeer.GetAudioBand(frequencyBand);
        // transform.localScale = baseScale + bumpDimensions * audioPeer.GetFrequencyBand(frequencyBand) * multiplier;
        // Vector3 vector3 = new Vector3(audioBand , audioBand , audioBand);
        // material.SetVector("_emission", vector3);
    }

    float average(int StartIndex, int EndIndex, float[] list)
    {
        float sum = 0;
        for (int i = StartIndex; i <= EndIndex; i++)
            sum += list[i];
        return sum / (EndIndex - StartIndex + 1);
    }
}
