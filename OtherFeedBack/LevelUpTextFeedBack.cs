using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelUpTextFeedBack : MonoBehaviour
{
    private float progress = 0f;
    private Vector3 startPos;
    private float timer = 0;
    private TextMeshPro txtMeshPro;
    bool increase = true;
    CameraBehaviour camBehav; 

    // Start is called before the first frame update
    void Start()
    {
        camBehav = InputManager.main.GetComponentInChildren<CameraBehaviour>();
        txtMeshPro = GetComponent<TextMeshPro>();
        txtMeshPro.fontSize = Mathf.Max((float)50.0f * (camBehav.thirdPersonCamera.m_YAxis.Value + 0.01f), 20.0f);
        txtMeshPro.color = new Color(1, 1, 0);

        txtMeshPro.text = "Level    Up !";
        txtMeshPro.alpha = 0;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, -180, 0));
        txtMeshPro.fontSize = Mathf.Max((float)50.0f * (camBehav.thirdPersonCamera.m_YAxis.Value + 0.01f), 20.0f);

        progress += (Time.deltaTime * 0.3f);
        transform.position = (1.0f - progress) * startPos + transform.right * Mathf.Cos(progress * Mathf.PI) *2 + progress * (Vector3.up + startPos) + Vector3.up * Mathf.Sin(progress * Mathf.PI);

        timer += Time.deltaTime;

        if (progress < 0.6)
        {
            txtMeshPro.alpha = Mathf.Lerp(txtMeshPro.alpha, 1, progress);
        }
        else
        {
            txtMeshPro.alpha = Mathf.Lerp(txtMeshPro.alpha, 0, Time.deltaTime);
        }

        if (txtMeshPro.alpha <= 0.001) Destroy(this.gameObject);

    }
}
