using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAPBar : MonoBehaviour
{
    [SerializeField] Portrait plocalcharacter;
    [SerializeField] GameObject actionPoints;

    Character localcharacter;

    void Start()
    {
        localcharacter = plocalcharacter.GetCharacter();        
    }

    // Update is called once per frame
    void Update()
    {
        if (localcharacter != plocalcharacter.GetCharacter())
        {
            localcharacter = plocalcharacter.GetCharacter();            
        }

        if(localcharacter.State != CHARACTER_STATE.FIGHT)
        {
            actionPoints.SetActive(false);
        }
        else
        {
            if (!actionPoints.active)
            {
                actionPoints.SetActive(true);
                actionPoints.GetComponent<VisuActionPoints>().InitializePA();
            }
        }
    }
}
