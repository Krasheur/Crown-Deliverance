using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBlastFX : MonoBehaviour
{
    [SerializeField] float duration;
    GameObject lightningBlast;
    GameObject lightning;
    GameObject blast;
    GameObject core;
    Material matLightning;
    Material matBlast;
    Material matCore;
    Light light;

    Color blast_Color_Start;
    Color blast_Color_End;

    Color lightningBlast_Color_Start;
    Color lightningBlast_Color_End;

    Vector3 lightningStartPos;

    [SerializeField] float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        lightningBlast = transform.GetChild(0).GetChild(0).gameObject;
        blast = transform.GetChild(0).GetChild(1).gameObject;
        core = transform.GetChild(0).GetChild(3).gameObject;
        lightning = transform.GetChild(1).gameObject;
        lightningStartPos = lightning.transform.localPosition;
        matLightning = lightningBlast.GetComponent<MeshRenderer>().material;
        matBlast = blast.GetComponent<MeshRenderer>().material;
        matCore = core.GetComponent<MeshRenderer>().material;
        light = transform.GetChild(0).GetChild(2).GetComponent<Light>();

        blast_Color_Start = matBlast.GetColor("_Color");
        blast_Color_End = Color.black;

        lightningBlast_Color_Start = matLightning.GetColor("_Color");
        lightningBlast_Color_End = Color.black;
        CameraState.instance.Shake(2.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        light.range = 3.33f * transform.localScale.x;
        timer = Mathf.Min(timer + Time.deltaTime / duration, 1);
        float t1 = Mathf.Min(timer * 0.9f * 1.1f, 1);
        float t2 = Mathf.Clamp01(Mathf.Min(Mathf.Pow(timer, 0.5f) * 1.5f, 1));
        matBlast.SetFloat("_OutlineIntensity", Mathf.Lerp(0, 3, Mathf.Pow(Mathf.Max(0, Mathf.Sin(t2 * Mathf.PI)), 0.1f)));
        matBlast.SetFloat("_RayMarchingThickness", Mathf.Lerp(-10, 0, Mathf.Pow(t2, 0.5f)));
        matBlast.SetColor("_Color", Color.Lerp(blast_Color_Start, blast_Color_End, Mathf.Pow(t2, 10.0f)));

        matCore.SetFloat("_OutlineIntensity", Mathf.Lerp(0, 4, Mathf.Pow(t1, 0.5f) - Mathf.Pow(Mathf.Max(t1 * 5.0f - 4.0f, 0), 0.5f)));
        matCore.SetFloat("_RayMarchingThickness", Mathf.Lerp(-5, -2, Mathf.Pow(t1, 1.0f)));
        matCore.SetColor("_Color", Color.Lerp(blast_Color_End, blast_Color_Start, Mathf.Pow(t1, 0.5f) - Mathf.Pow(Mathf.Max(t1 * 5.0f - 4.0f, 0), 0.5f)));

        lightningBlast.transform.localScale = Vector3.one * Mathf.Lerp(0, 6f, Mathf.Pow(t1, 0.2f) - Mathf.Pow(Mathf.Max(t1 * 5.0f - 4.0f, 0), 0.5f));
        matLightning.SetColor("_Color", Color.Lerp(lightningBlast_Color_Start, lightningBlast_Color_End, Mathf.Pow(Mathf.Max(0, Mathf.Cos(t1 * Mathf.PI)), 0.7f)));

        light.intensity = Mathf.Lerp(0, 33.33f * transform.localScale.x, Mathf.Sin(t2 * Mathf.PI) + 0.3f * (Mathf.Pow(t1, 0.5f) - Mathf.Pow(Mathf.Max(t1 * 5.0f - 4.0f, 0), 0.5f)));
        lightning.transform.localPosition = Vector3.Lerp(lightningStartPos, Vector3.zero, Mathf.Sin(t2 * Mathf.PI));
        if (timer >= 1)
        {
            timer = 0f;
            Destroy(gameObject);
        }
    }
}
