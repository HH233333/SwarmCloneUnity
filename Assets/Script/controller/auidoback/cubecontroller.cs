using Unity.VisualScripting;
using UnityEngine;

public class cubecontroller : MonoBehaviour {

    public AudioPeer audioPeer;
    public int frequencyBand = 0;
    public float multiplier = 0.2f;
    public Vector3 bumpDimensions = new Vector3(0, 1, 0);

    private Vector3 baseScale;
    private Material material;

    private void Start()
    {
        baseScale = transform.localScale;
        material = transform.GetComponent<MeshRenderer>().materials[0];
    }

    void Update() {
        if (audioPeer == null) 
            return;
        float audioBand = audioPeer.GetAudioBand(frequencyBand);
        transform.localScale = baseScale + bumpDimensions * audioPeer.GetFrequencyBand(frequencyBand) * multiplier;
        Vector3 vector3 = new Vector3(audioBand , audioBand , audioBand);
        material.SetVector("_emission", vector3);
    }
}
