using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightNoise : MonoBehaviour
{
    [SerializeField] Texture2D tex = null;
    Light light = null;
    Vector3 mainPosition;
    public float positionSpeed = 1f;
    public float positionSpread = 1f;
    public float intensitySpeed = 100f;
    public float intensitySpread = .5f;
    public float intensity = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        mainPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float noise = (tex.GetPixel((int)(Time.timeSinceLevelLoad * intensitySpeed), 0).r * 2f - 1f) * intensitySpread;
        float xNoise = (tex.GetPixel(0, (int)(Time.timeSinceLevelLoad * positionSpeed)).r * 2f - 1f) * positionSpread;
        float yNoise = (tex.GetPixel(0, (int)(Time.timeSinceLevelLoad * positionSpeed)).g * 2f - 1f) * positionSpread;
        float zNoise = (tex.GetPixel((int)(Time.timeSinceLevelLoad * positionSpeed), (int)(Time.timeSinceLevelLoad * positionSpeed)).r * 2f - 1f) * positionSpread;

        light.intensity = intensity + noise;
        transform.localPosition = mainPosition + new Vector3(xNoise, yNoise, zNoise);
    }
}
