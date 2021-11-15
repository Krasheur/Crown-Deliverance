using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisuPortraitsPermHUD : MonoBehaviour
{
    [SerializeField] Portrait localPortrait;
    [SerializeField] Portrait Portrait2;
    [SerializeField] Portrait Portrait3;

    [SerializeField] GameObject alterationsJ1;
    [SerializeField] GameObject alterationsJ2;
    [SerializeField] GameObject alterationsJ3;

    AlterationIcon[] alterationsIconJ1 = null;
    AlterationIcon[] alterationsIconJ2 = null;
    AlterationIcon[] alterationsIconJ3 = null;

    private static Portrait focusedPortrait;
    public static Portrait FocusedPortrait { get => focusedPortrait; set => focusedPortrait = value; }

    PlayerManager pm;    

    // Start is called before the first frame update
    void Start()
    {
        pm = PlayerManager.main;
        ActualizeCharacters();
    }

    // Update is called once per frame
    void Update()
    {               
         ActualizeCharacters();

        alterationsIconJ1 = localPortrait.Character.GetComponentsInChildren<AlterationIcon>();
        alterationsIconJ2 = Portrait2.Character.GetComponentsInChildren<AlterationIcon>();
        alterationsIconJ3 = Portrait3.Character.GetComponentsInChildren<AlterationIcon>();

        for (int i = 0; i < alterationsIconJ1.Length; i++)
        {
            if (alterationsIconJ1[i].Sprite != null)
            {
                alterationsJ1.transform.GetChild(i).gameObject.SetActive(true);
                alterationsJ1.transform.GetChild(i).GetComponent<Image>().sprite = alterationsIconJ1[i].Sprite;
            }
        }

        for (int i = alterationsIconJ1.Length; i < alterationsJ1.transform.childCount; i++)
        {
            alterationsJ1.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < alterationsIconJ2.Length; i++)
        {
            if (alterationsIconJ2[i].Sprite != null)
            {
                alterationsJ2.transform.GetChild(i).gameObject.SetActive(true);
                alterationsJ2.transform.GetChild(i).GetComponent<Image>().sprite = alterationsIconJ2[i].Sprite;
            }
        }

        for (int i = alterationsIconJ2.Length; i < alterationsJ2.transform.childCount; i++)
        {
            alterationsJ2.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < alterationsIconJ3.Length; i++)
        {
            if (alterationsIconJ3[i].Sprite != null)
            {
                alterationsJ3.transform.GetChild(i).gameObject.SetActive(true);
                alterationsJ3.transform.GetChild(i).GetComponent<Image>().sprite = alterationsIconJ3[i].Sprite;
            }
        }

        for (int i = alterationsIconJ3.Length; i < alterationsJ3.transform.childCount; i++)
        {
            alterationsJ3.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void ActualizeCharacters()
    {
        localPortrait.SetCharacter(pm.FocusedCharacter);

        if (localPortrait.GetCharacter() == pm.PlayerFairy)
        {
            Portrait2.SetCharacter(pm.PlayerRogue);
            Portrait3.SetCharacter(pm.PlayerTank);
        }
        else if (localPortrait.GetCharacter() == pm.PlayerRogue)
        {
            Portrait2.SetCharacter(pm.PlayerFairy);
            Portrait3.SetCharacter(pm.PlayerTank);
        }
        else if (localPortrait.GetCharacter() == pm.PlayerTank)
        {
            Portrait2.SetCharacter(pm.PlayerFairy);
            Portrait3.SetCharacter(pm.PlayerRogue);
        }
    }

    public Portrait GetPortrait(int index)
    {
        switch(index)
        {
            case 1: return localPortrait;
            case 2: return Portrait2;
            case 3: return Portrait3;
            default: return null;
        }        
    }
}
