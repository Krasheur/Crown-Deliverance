using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Ragdoll owner;
    Rigidbody rb;

    public Ragdoll Owner { get => owner; set => owner = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(Vector3.up * 175f);
        rb.AddForce(owner.transform.forward * 80f);
    }

    private void Update()
    {
        if (rb.velocity == Vector3.zero)
        {
            rb.isKinematic = true;
        }
    }
}
