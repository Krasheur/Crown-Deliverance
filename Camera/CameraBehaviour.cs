using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraBehaviour : MonoBehaviour
{
    public NavMeshAgent target = null;
    public Cinemachine.CinemachineFreeLook thirdPersonCamera;
    public Camera mainCamera;
    public float smoothCameraStopSpeed = 3f;

    public Transform follow;
    public float turnSmoothTime = 0.1f;
    public float followSpeed = 6f;
    float turnSmoothVelocity;

    public static CameraBehaviour instance = null;

    public bool isGoingToThirdPersonView = false;
    public bool isGoingToOrbitalView = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }

        instance = this;
        WaveManager.Awake();
    }

    void Update()
    {
        if (target != null)
        {
            follow.transform.position = Vector3.Lerp(follow.transform.position, target.transform.position, Time.deltaTime * smoothCameraStopSpeed);
        }
        else
        {
            MoveFollow();
        }

        //Combat Mode activated
        if(isGoingToOrbitalView)
        {
            GoToOrbitalView();
        }        
        
        //ThirdPersonView Mode activated
        if(isGoingToThirdPersonView)
        {
            GoToThirdPersonView();
        }
    }

    public void TurnCameraAround()
    {
        if (Input.GetButtonDown("WheelClick"))
        {
            thirdPersonCamera.m_XAxis.m_InputAxisName = "Mouse X";
        }
        else if (Input.GetButtonUp("WheelClick"))
        {
            thirdPersonCamera.m_XAxis.m_InputAxisName = null;
        }

        if (thirdPersonCamera.m_XAxis.m_InputAxisName == null)
        {
            thirdPersonCamera.m_XAxis.m_InputAxisValue = Mathf.Lerp(thirdPersonCamera.m_XAxis.m_InputAxisValue, 0f, Time.deltaTime * smoothCameraStopSpeed);

            if (thirdPersonCamera.m_XAxis.m_InputAxisValue >0f && thirdPersonCamera.m_XAxis.m_InputAxisValue <= 0.01f)
            {
                thirdPersonCamera.m_XAxis.m_InputAxisValue = 0f;
            }
            else if (thirdPersonCamera.m_XAxis.m_InputAxisValue < 0f && thirdPersonCamera.m_XAxis.m_InputAxisValue >= -0.01f)
            {
                thirdPersonCamera.m_XAxis.m_InputAxisValue = 0f;
            }
        }
    }

    public void MoveFollow()
    {
        Vector3 speed = new Vector3(0f, 0f, 0f);

        speed.x = Input.GetAxisRaw("Horizontal");
        speed.z = Input.GetAxisRaw("Vertical");

        if (speed.x != 0f && speed.z != 0f)
        {
            speed.Normalize();
        }

        if (speed.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(speed.x, speed.z) * Mathf.Rad2Deg + CameraBehaviour.instance.mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(follow.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            follow.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 tmpPos = follow.position + moveDir.normalized * followSpeed * Time.deltaTime;

            //NavMeshHit hit;
            //if(NavMesh.Raycast(follow.position, tmpPos, out hit, 1))
            if (Physics.Raycast(tmpPos, -follow.up, 2f))
            {
                follow.position = tmpPos;
            }
        }
    }

    public void GoToOrbitalView()
    {
        thirdPersonCamera.m_YAxis.m_InputAxisName = "Mouse ScrollWheel";
        thirdPersonCamera.m_YAxis.Value = Mathf.Lerp(thirdPersonCamera.m_YAxis.Value, 1f, Time.deltaTime);

        if(Input.GetAxis("Mouse ScrollWheel") != 0f || thirdPersonCamera.m_YAxis.Value >= 0.7f)
        {
            isGoingToOrbitalView = false;
        }
    }    
    
    public void GoToThirdPersonView()
    {
        thirdPersonCamera.m_YAxis.m_InputAxisName = "";
        thirdPersonCamera.m_YAxis.Value = Mathf.Lerp(thirdPersonCamera.m_YAxis.Value, -1f, Time.deltaTime);

        if (thirdPersonCamera.m_YAxis.Value <= 0f)
        {
            isGoingToThirdPersonView = false;
        }
    }
}