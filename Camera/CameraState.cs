using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraState : MonoBehaviour
{
    Vector3 cameraInitialPosition;
    public static float shakeMagnitude = 0.02f;
    public static float shakeSpeed = 4f;
    public Camera mainCam;

    public static CameraState instance = null;

    [SerializeField] CinemachineFreeLook vc;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startIntensity;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        //_cinemachineVirtualCamera = vc.GetComponent<CinemachineVirtualCamera>();
        //_cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float timer)
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = timer;
        _shakeTimerTotal = timer;
        _startIntensity = intensity;
    }
       
    public void ShakeCameraStop()
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        _shakeTimer = 0f;
        _shakeTimerTotal = 0f;
        _startIntensity = 0f;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
        }
    }

    public void Shake(float _shakeMagnitude, float _shakeTime)
    {
        if (_shakeMagnitude != shakeMagnitude) shakeMagnitude = _shakeMagnitude;
        cameraInitialPosition = mainCam.transform.position;
        InvokeRepeating("StartShake", 0f, 0.0005f);
        Invoke("StopShake", _shakeTime);
    }

    public void Shake(float _shakeMagnitude)
    {
        if (_shakeMagnitude != shakeMagnitude) shakeMagnitude = _shakeMagnitude;
        cameraInitialPosition = mainCam.transform.position;
        InvokeRepeating("StartShake", 0f, 0.0005f);
    }

    public void ShakeStop()
    {
        Invoke("StopShake", 0f);
    }

    void StartShake()
    {
        float camOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float camOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        Vector3 camNewPos = mainCam.transform.position;
        camNewPos.x += camOffsetX;
        camNewPos.y += camOffsetY;
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camNewPos, Time.deltaTime * shakeSpeed);
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCam.transform.position = cameraInitialPosition;
    }
}
