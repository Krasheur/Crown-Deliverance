using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamageComportement : MonoBehaviour
{
    private float progress = 0f;
    private Vector3 startPos ;
    private float timer = 0;
    private TextMeshPro txtMeshPro;
    private float noise;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void Init()
    {
        txtMeshPro = GetComponent<TextMeshPro>();



        noise = Random.Range(-1.5f, 1.5f);
        startPos = transform.position;
        startPos.x += noise / 10.0f;
        startPos.y += 2 + noise / 10.0f;
        //transform.position += startPos;
        float distToCam = Vector3.Distance(Camera.main.transform.position, transform.position) / 50.0f;
        gameObject.transform.localScale = Vector3.one * distToCam * 0.5f;
    }

    // si delegate y a ptete moyen d'inscrire une fct differente et donc de faire un update dif pour chaque damag
    void Update()
    {
        float distToCam = Vector3.Distance(Camera.main.transform.position, transform.position) / 50.0f;
        gameObject.transform.localScale = Vector3.one * distToCam * 0.5f;

        gameObject.transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, -180, 0));

        progress += (Time.deltaTime * 0.2f);
        transform.position = (1.0f - progress) * startPos + progress * (Vector3.up * distToCam + startPos)  + (transform.right * Mathf.Sin(progress * Mathf.PI *(5 +(noise *3))) * 1 + Vector3.up * Mathf.Sin(progress * Mathf.PI) * (5+ noise)) * distToCam;

        timer += Time.deltaTime;

         txtMeshPro.alpha = Mathf.Lerp(txtMeshPro.alpha, 0, progress * progress* progress );

        if (timer > 3 || progress>=0.5f) Destroy(this.gameObject);


        txtMeshPro.enabled = true;
    }
}
