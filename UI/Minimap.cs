using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    static public Minimap main;
    PlayerManager pm;    

    [SerializeField] int ZoomMin = 10;
    [SerializeField] int ZoomMax = 20;    

    void Start()
    {
        pm = PlayerManager.main;
    }
    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
    }

    void LateUpdate()
    {
        if (pm != null)
        {


            Vector3 newPosition = pm.FocusedCharacter.GetComponent<Transform>().position;
            newPosition.y += 250.0f;
            transform.position = newPosition;
        }
        //transform.rotation = Quaternion.Euler(90.0f, player.eulerAngles.y, 0f); For rotate minimap
    }

    public void Zoom()
    {
        if(GetComponent<Camera>().orthographicSize > ZoomMin)
        {
            AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
            GetComponent<Camera>().orthographicSize--;
        }
    }

    public void Dezoom()
    {
        if (GetComponent<Camera>().orthographicSize < ZoomMax)
        {
            AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
            GetComponent<Camera>().orthographicSize++;
        }
    }
}
