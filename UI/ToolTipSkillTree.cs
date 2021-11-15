using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipSkillTree : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ToolTipPopUp toolTipPopUp;
    [SerializeField] int index;
    [SerializeField] bool increasePA;
    [SerializeField] bool increaseCooldown;

    // Start is called before the first frame update
    void Start()
    {
        toolTipPopUp = GameObject.Find("TooltipPopUp").GetComponent<ToolTipPopUp>();
    }

    public string GetTooltipInfoText()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<size=20>");
        builder.Append(PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().characterAlteration.GetSpellAlteration(PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().skillTree[index].alterationName).DescriptionFull).AppendLine();
                
        if(increasePA)
        {
            builder.Append("<color=red>(Spell Cost +1)</color>");
        }
        else if(increaseCooldown)
        {
            builder.Append("<color=red>(Cooldown +1)</color>");
        }
        builder.Append("</size>");
        return builder.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {        
        toolTipPopUp.DisplayInfoSkillTree(this, index);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipPopUp.HideInfo();        
    }
}
