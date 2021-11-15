using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbTrigger : MonoBehaviour
{
    [SerializeField] MusicsManager.AmbKind ambName;

    private bool isPlaying = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isPlaying)
        {
            if (SceneManager.GetActiveScene().buildIndex > 2 && other.gameObject.GetComponent<Character>() == PlayerManager.main.FocusedCharacter)
            {
                //Start Music
                MusicsManager.instance.ambState.Add(ambName);
                isPlaying = true;
                //
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex > 2 && other.gameObject.GetComponent<Character>() == PlayerManager.main.FocusedCharacter)
        {
            //End Music
            MusicsManager.instance.ambState.Remove(ambName);
            isPlaying = false;
            //
        }
    }

    private void OnDestroy()
    {
        //End Music
        MusicsManager.instance.ambState.Remove(ambName);
        //
    }
}
