using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text stack;

    [SerializeField]
    private ToolTipPopUp toolTipPopUp;

    private LootWindow lootWindow;

    public Image MyIcon { get => icon; }
    public Text MyTitle { get => title; }
    public Text MyStack { get => stack; }

    public ItemInventory MyLoot { get; set; }    

    void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {        
        if(PlayerManager.main.FocusedCharacter.PutInInventory(MyLoot.Item,-1, MyLoot.Nb))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(MyLoot);
            toolTipPopUp.HideInfo();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipPopUp.DisplayInfoPickable(MyLoot.Item, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipPopUp.HideInfo();
    }
}
