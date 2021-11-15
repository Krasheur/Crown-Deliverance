using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipEquipable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    enum EQUIPABLE
    {
        RIGHT_HAND,
        LEFT_HAND,
        BODY
    };
    
    [SerializeField] EQUIPABLE type;
    [SerializeField] private ToolTipPopUp toolTipPopUp;        

    public Equipable equipable;

    // Start is called before the first frame update
    void Start()
    {
        toolTipPopUp = GameObject.Find("TooltipPopUp").GetComponent<ToolTipPopUp>();
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch(type)
        {
            case EQUIPABLE.RIGHT_HAND:
                if (PlayerManager.main.FocusedCharacter.RightHand != null)
                {
                    toolTipPopUp.DisplayInfoEquipable(PlayerManager.main.FocusedCharacter.RightHand, PlayerManager.main.FocusedCharacter);
                }
                break;
            case EQUIPABLE.LEFT_HAND:
                if (PlayerManager.main.FocusedCharacter.LeftHand != null)
                {
                    toolTipPopUp.DisplayInfoEquipable(PlayerManager.main.FocusedCharacter.LeftHand, PlayerManager.main.FocusedCharacter);
                }
                break;
            case EQUIPABLE.BODY:
                if (PlayerManager.main.FocusedCharacter.Body != null)
                {
                    toolTipPopUp.DisplayInfoEquipable(PlayerManager.main.FocusedCharacter.Body, PlayerManager.main.FocusedCharacter);
                }
                break;            
        }             
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipPopUp.HideInfo();        
    }
}
