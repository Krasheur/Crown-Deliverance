using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipSpell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ToolTipPopUp toolTipPopUp;

    [SerializeField] private VisuActionPoints visuAP;

    public SpellHolder spell;

    // Start is called before the first frame update
    void Start()
    {
        toolTipPopUp = GameObject.Find("TooltipPopUp").GetComponent<ToolTipPopUp>();
    }
    public string GetTooltipInfoText(SpellHolder spell)
    {
        StringBuilder builder = new StringBuilder();
        
        builder.Append(spell.Description).AppendLine();

        return builder.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipPopUp.DisplayInfoSpell(spell, this);
        visuAP.SpellCost = spell.Spell.Cost + spell.CostModifier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipPopUp.HideInfo();
        visuAP.SpellCost = 0;
    }
}
