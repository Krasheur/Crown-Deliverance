using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBlastFX : MonoBehaviour
{
    Material mat;
    float timer = 0;
    Color color;
    Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        color = mat.GetColor("_Color");
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 4.0f;
        transform.localScale = scale * 0.5f + 0.5f * scale * Mathf.Pow(timer / 3.0f, 0.7f);
        mat.SetFloat("_RayMarchingThickness", -timer * 2 * transform.localScale.x * Mathf.Pow(timer / 3.0f, 4.0f) / 2.0f);
        mat.SetVector("_Color", color * Mathf.Pow(Mathf.Sin(timer / 3.0f * Mathf.PI), 0.2f));
        if (timer > 3.0f)
        {
            Destroy(gameObject);
        }
    }
}
