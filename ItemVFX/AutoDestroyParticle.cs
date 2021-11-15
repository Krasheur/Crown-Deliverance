using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;

    private void Awake()
    {
        if (!particleSystem) particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem && !particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
