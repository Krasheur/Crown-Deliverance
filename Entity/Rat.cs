using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour
{
    [SerializeField] float refresh_time;
    float timer = 0.0f;
    float proximityTimer = 0;
    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Vector3 randomDirection = Random.insideUnitSphere * 4.0f;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 4.0f, NavMesh.AllAreas);

        agent.SetDestination(hit.position);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        proximityTimer = Mathf.Max(0, proximityTimer - Time.deltaTime);
        if (timer > refresh_time)
        {
            agent.speed = 3.5f;
            Vector3 randomDirection = Random.insideUnitSphere * 10.0f;

            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.AllAreas);

            agent.SetDestination(hit.position);
            timer = 0;
        }

        timer += Time.deltaTime;

        if (agent.hasPath)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (proximityTimer <= 0 && (collider.gameObject.tag == "Character" || collider.gameObject.tag == "Player"))
        {
            agent.speed = 5f;
            proximityTimer = 2.0f;
            Transform startTransform = transform;
            transform.rotation = Quaternion.LookRotation(transform.position - collider.gameObject.transform.position);
            Vector3 runTo = transform.position + transform.forward * 5.0f;
            NavMeshHit hit;
            NavMesh.SamplePosition(runTo, out hit, 5, NavMesh.AllAreas);
            timer = 0;

            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;

            agent.ResetPath();
            agent.SetDestination(hit.position);

            if (gameObject) AkSoundEngine.PostEvent("Ratclure_Play", gameObject) ;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character" || other.gameObject.tag == "Player")
        {
            proximityTimer = 0;
        }
    }
}