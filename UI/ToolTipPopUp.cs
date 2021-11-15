using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipPopUp : MonoBehaviour
{
    private static ToolTipPopUp main = null;
    [SerializeField] private GameObject popupCanvasObject;
    [SerializeField] private RectTransform popupObject;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float padding;

    private Canvas popupCanvas;

    public static ToolTipPopUp Main { get => main; set => main = value; }

    private void Awake()
    {
        if (Main != null)
        {
            Destroy(gameObject);
            return;
        }
        Main = this;

        popupCanvas = popupCanvasObject.GetComponent<Canvas>();
    }

    private void OnDestroy()
    {
        if(Main == this)
        {
            Main = null;
        }
    }

    private void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        if(!popupCanvasObject.activeSelf)
        {
            return;
        }

        Vector3 newPos = Input.mousePosition + offset;
        newPos.z = 0f;
        float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;

        if(rightEdgeToScreenEdgeDistance < 0)
        {
            newPos.x += rightEdgeToScreenEdgeDistance;
        }

        float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;

        if (leftEdgeToScreenEdgeDistance > 0)
        {
            newPos.x += leftEdgeToScreenEdgeDistance;
        }

        float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;

        if (topEdgeToScreenEdgeDistance < 0)
        {
            newPos.y += topEdgeToScreenEdgeDistance;
        }
        popupObject.transform.position = newPos;
    }

    public void DisplayInfoChangeLvl(ChangeLvl _changeLvl)
    {
        infoText.text = "<color=green><size=35>" + _changeLvl.NameInToolTip + "</size></color>";
        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void DisplayInfoPortrait(Portrait portrait)
    {
        StringBuilder builder = new StringBuilder();

        if(portrait.GetCharacter().Hostility == CHARACTER_HOSTILITY.ALLY)
        {
            builder.Append("<color=green>");
        }
        else if(portrait.GetCharacter().Hostility == CHARACTER_HOSTILITY.ENEMY)
        {
            builder.Append("<color=red>");
        }
        else if (portrait.GetCharacter().Hostility == CHARACTER_HOSTILITY.NEUTRAL)
        {
            builder.Append("<color=yellow>");
        }

        builder.Append("<size=35>" + portrait.GetCharacter().Name + "</size></color>").AppendLine();
        builder.Append("<size=5>\n</size>").AppendLine();
        builder.Append(portrait.GetTooltipInfoText());

        infoText.text = builder.ToString();

        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void DisplayInfoCharacter(Character character)
    {
        StringBuilder builder = new StringBuilder();

        if (character.Hostility == CHARACTER_HOSTILITY.ALLY)
        {
            builder.Append("<color=green>");
        }
        else if (character.Hostility == CHARACTER_HOSTILITY.ENEMY)
        {
            builder.Append("<color=red>");
        }
        else if (character.Hostility == CHARACTER_HOSTILITY.NEUTRAL)
        {
            builder.Append("<color=yellow>");
        }

        builder.Append("<size=35>" + character.Name + "</size></color>").AppendLine();
        builder.Append("<size=5>\n</size>").AppendLine();
        builder.Append(character.GetTooltipInfoText());

        infoText.text = builder.ToString();

        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void DisplayInfoSpell(SpellHolder spellholder, ToolTipSpell button)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<size=35><color=purple>" + spellholder.Spell.Name + "</size></color>").AppendLine();
        builder.Append("<size=5>\n</size>").AppendLine();
        builder.Append("<size=30>Cost : " + (spellholder.Spell.Cost + spellholder.CostModifier) + " AP" + "</size>").AppendLine();        
        builder.Append("<size=30>Cooldown : " + spellholder.Cooldown + " turns" + "\n</size>").AppendLine();
        builder.Append(button.GetTooltipInfoText(spellholder));
        
        infoText.text = "" + builder;

        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void DisplayInfoPickable(Pickable pickable, Character character)
    {
        if(pickable != null)
        {
            StringBuilder builder = new StringBuilder();
            pickable.OnGetDescription(ref builder);

            if (character != null)
            {
                builder.Append("<size=18><color=yellow>                                                              " + "Value : " + pickable.Value + " g</size></color>").AppendLine();
            }
            else
            {
                builder.Append("<size=18><color=yellow>                                                              " + "Value : " + pickable.Value + " g</size></color>").AppendLine();
            }

            infoText.text = "" + builder;

            popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
        }
    }    

    public void DisplayInfoEquipable(Equipable equipable, Character character)
    {
        StringBuilder builder = new StringBuilder();

        equipable.OnGetDescription(ref builder);

        if (character != null)
        {
            builder.Append("<size=18><color=yellow>                                                                " + "Value : " + (int)(equipable.Value * (character.IsMerchant ? 1.5f : 1)) + " g</size></color>").AppendLine();
        }
        else
        {
            builder.Append("<size=18><color=yellow>                                                                " + "Value : " + equipable.Value + " g</size></color>").AppendLine();
        }

        infoText.text = "" + builder;

        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void DisplayInfoSkillTree(ToolTipSkillTree button, int index)
    {
        StringBuilder builder = new StringBuilder();

        if (PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().characterAlteration.GetSpellAlteration(PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().skillTree[index].alterationName).DisplayName != "")
        {
            builder.Append("<size=25>" + PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().characterAlteration.GetSpellAlteration(PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().skillTree[index].alterationName).DisplayName + "</size>");
        }
        else
        {
            builder.Append("<size=25>" + PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().skills[index].alterationName + "</size>");
        }
        builder.Append("<size=5>\n</size>").AppendLine();
        builder.Append("<size=25>Cost : " + PlayerManager.main.FocusedCharacter.GetComponent<SkillTreeReader>().skillTree[index].cost + " point(s)</size>\n").AppendLine();        
        builder.Append(button.GetTooltipInfoText());

        infoText.text = "" + builder;        

        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void HideInfo()
    {
        popupCanvasObject.SetActive(false);
    }
}
