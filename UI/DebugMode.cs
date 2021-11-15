using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{
    Cheats cheats = null;
    
    // Start is called before the first frame update
    void Start()
    {
        cheats = Cheats.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(cheats.CheatsActivated)
        {
            GetComponent<Text>().enabled = true;
        }
        else
        {
            GetComponent<Text>().enabled = false;
        }
    }
}
