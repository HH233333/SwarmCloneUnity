using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backcontraller2 : MonoBehaviour
{
    public AudioPeer audioPeer;

    private Material material;
    private Texture2D _audiotexture;
    private void Start()
    {
        // baseScale = transform.localScale;
        material = transform.GetComponent<MeshRenderer>().materials[0];
        _audiotexture = new Texture2D(8, 1);
        _audiotexture.filterMode = FilterMode.Point;
        Color[] backPixels = new Color[_audiotexture.width * _audiotexture.height];
        for (int i = 0; i < backPixels.Length; i++)
            backPixels[i] = Color.black;
        _audiotexture.SetPixels(backPixels);
        _audiotexture.Apply();
        material.SetTexture(Shader.PropertyToID("_AudioTex"), _audiotexture);
    }
    // Update is called once per frame
    void Update()
    {
        if (audioPeer == null)
            return;
        if (audioPeer.singing)
        {
            var sample = audioPeer.GetGetAudioBands();
            Color col = Color.black;
            for (int i = 0; i < sample.Length; i++)
            {
                col.r = sample[i];
                _audiotexture.SetPixel(i, 0, col);
            }
            _audiotexture.Apply();
        }
    }
}
