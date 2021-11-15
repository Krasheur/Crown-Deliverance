using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    [SerializeField] string[] voices =null;

    public void TriggerDialogue(Character _character)
    {
        //For set talker position
        DialogueManager.Main.talker = transform;
        //
        //Sound
        if (voices != null)
        {
            for (int i = 0; i < voices.Length; i++)
            {
                DialogueManager.Main.voices.Add(voices[i]);
            }
            if (voices.Length != 0) DialogueManager.Main.voicesCount = 0;
        }
        //

        PlayerManager.main.FocusedCharacter.State = CHARACTER_STATE.LOCKED;        
        dialogue.name = _character.name;
        GameObject.Find("Photo Dialogue").GetComponent<Image>().sprite = _character.Portrait;
        GameObject.Find("PermanentHUD").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PermanentHUD").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("PermanentHUD").GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (_character.IsMerchant)
        {
            TradingManager.main.CurrentTrader = _character;
            AkSoundEngine.PostEvent("MerchandUp_Play", gameObject);
        }
        DialogueManager.Main.StartDialogue(_character.gameObject.GetComponent<DialogueTrigger>().dialogue);
    }
}
