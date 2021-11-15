using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] ImageOnMouse itemMouse;

    public void Drop()
    {
        if (itemMouse.Item != null)
        {
            itemMouse.Item.EjectHim(PlayerManager.main.FocusedCharacter.transform.position, PlayerManager.main.FocusedCharacter.transform.forward);
            itemMouse.Item = null;
            itemMouse.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
            itemMouse.transform.parent.GetComponentInChildren<Image>().sprite = null;
            itemMouse.transform.parent.GetComponentInChildren<ImageOnMouse>().Item = null;
        }
    }
}
