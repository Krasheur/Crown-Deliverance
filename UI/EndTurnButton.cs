using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] Portrait localCharacter;

    [SerializeField] Sprite fairySprite;
    [SerializeField] Sprite rogueSprite;
    [SerializeField] Sprite warriorSprite;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(localCharacter.GetCharacter().State != CHARACTER_STATE.FREE && localCharacter.GetCharacter().CurrentFight != null)
        {
            GetComponent<CanvasGroup>().alpha = 1;
            GetComponent<CanvasGroup>().interactable = true;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().interactable = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        if(localCharacter.GetCharacter() == PlayerManager.main.PlayerFairy)
        {
            this.GetComponent<Image>().sprite = fairySprite;
        }
        else if(localCharacter.GetCharacter() == PlayerManager.main.PlayerRogue)
        {
            this.GetComponent<Image>().sprite = rogueSprite;
        }
        else if (localCharacter.GetCharacter() == PlayerManager.main.PlayerTank)
        {
            this.GetComponent<Image>().sprite = warriorSprite;
        }
    }

    public void EndTurn()
    {
        Fight currentFight = GameObject.Find("Fight(Clone)").GetComponent<Fight>();

        if (currentFight.fighters[currentFight.current].character.SpellCasted != null)
        {
            currentFight.fighters[currentFight.current].character.SpellCasted.Cancel();
        }

        if (currentFight.fighters[currentFight.current].character.Hostility == CHARACTER_HOSTILITY.ALLY)
        {
            AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
            currentFight.EndTurn();
        }
    }
}
