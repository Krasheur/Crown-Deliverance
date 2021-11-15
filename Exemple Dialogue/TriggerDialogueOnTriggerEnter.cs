using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerDialogueOnTriggerEnter : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogue;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            dialogue.TriggerDialogue(dialogue.gameObject.GetComponent<Character>());
            Destroy(gameObject);
        }
    }
}
