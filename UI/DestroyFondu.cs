using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyFondu : MonoBehaviour
{  
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 3)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {       
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Destroy"))
        {
            Destroy(gameObject);
        }
    }
}
