using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheet : MonoBehaviour
{
    [SerializeField] Sprite fairyBackground;
    [SerializeField] Sprite rogueBackground;
    [SerializeField] Sprite tankBackground;
    [SerializeField] GameObject gold;
    [SerializeField] GameObject levelUpButtons;

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.main.FocusedCharacter.NbTokenLevelup > 0)
        {
            levelUpButtons.GetComponent<CanvasGroup>().alpha = 1;
            levelUpButtons.GetComponent<CanvasGroup>().interactable = true;
            levelUpButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            levelUpButtons.GetComponent<CanvasGroup>().alpha = 0;
            levelUpButtons.GetComponent<CanvasGroup>().interactable = false;
            levelUpButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        if(PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerFairy && transform.GetChild(0).GetComponent<Image>().sprite != fairyBackground && GetComponent<CanvasGroup>().alpha == 1)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = fairyBackground;
            gold.transform.localPosition = new Vector3(-224.4f,-37.1f,0.0f);
        }
        else if(PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerRogue && transform.GetChild(0).GetComponent<Image>().sprite != rogueBackground && GetComponent<CanvasGroup>().alpha == 1)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = rogueBackground;
            gold.transform.localPosition = new Vector3(-224.4f, -131.4f, 0.0f);
        }
        else if (PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerTank && transform.GetChild(0).GetComponent<Image>().sprite != tankBackground && GetComponent<CanvasGroup>().alpha == 1)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = tankBackground;
            gold.transform.localPosition = new Vector3(-224.4f, -131.4f, 0.0f);
        }        

        if (transform.GetChild(1).GetChild(0).GetComponent<Text>().text != PlayerManager.main.FocusedCharacter.name)
        {
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerManager.main.FocusedCharacter.name;
        }

        transform.GetChild(1).GetChild(1).GetComponent<Text>().text = CharacterStats();

        gold.GetComponent<Text>().text = PlayerManager.main.FocusedCharacter.Gold.ToString();
    }

    string CharacterStats()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<size=25>Level : " + PlayerManager.main.FocusedCharacter.Level + "    Exp : " + PlayerManager.main.FocusedCharacter.GetExp + " / " + PlayerManager.main.FocusedCharacter.ExpToLevelUp).AppendLine();
        builder.Append("Life        : " + PlayerManager.main.FocusedCharacter.PV + " / " + PlayerManager.main.FocusedCharacter.PvMax).AppendLine();         
        builder.Append("Armor : " + PlayerManager.main.FocusedCharacter.Armor + " / " + PlayerManager.main.FocusedCharacter.ArmorMax + "</size>\n").AppendLine();

        builder.Append("<size=20>Stat Points Available : " + PlayerManager.main.FocusedCharacter.NbTokenLevelup + "\n\n");
        builder.Append("Strength                  :  " + PlayerManager.main.FocusedCharacter.StrengthBase + " (+" + PlayerManager.main.FocusedCharacter.StrengthBonus + ")" + " = " + PlayerManager.main.FocusedCharacter.GetStrength + "\n");
        builder.Append("Constitution        :  " + PlayerManager.main.FocusedCharacter.ConstitutionBase + " (+" + PlayerManager.main.FocusedCharacter.ConstitutionBonus + ")" + " = " + PlayerManager.main.FocusedCharacter.GetConstitution + "\n");
        builder.Append("Dexterity                 :  " + PlayerManager.main.FocusedCharacter.DexterityBase + " (+" + PlayerManager.main.FocusedCharacter.DexterityBonus + ")" + " = " + PlayerManager.main.FocusedCharacter.GetDexterity + "\n");
        builder.Append("Intelligence          :  " + PlayerManager.main.FocusedCharacter.IntelligenceBase + " (+" + PlayerManager.main.FocusedCharacter.IntelligenceBonus + ")" + " = " + PlayerManager.main.FocusedCharacter.GetIntelligence + "\n");
        builder.Append("Mobility                   :  " + PlayerManager.main.FocusedCharacter.MobilityBase + " (+" + (PlayerManager.main.FocusedCharacter.GetMobility - (PlayerManager.main.FocusedCharacter.MobilityBase )) + ")" + " = " + PlayerManager.main.FocusedCharacter.GetMobility + "\n");
        builder.Append("Dodge                        :  " + PlayerManager.main.FocusedCharacter.DodgeBase + " (+" + (PlayerManager.main.FocusedCharacter.GetDodge - (PlayerManager.main.FocusedCharacter.DodgeBase)) + ")" + " = " + PlayerManager.main.FocusedCharacter.GetDodge + " %\n");
        builder.Append("Critical chance  :  " + PlayerManager.main.FocusedCharacter.CriticalChanceBase + " (+" + (PlayerManager.main.FocusedCharacter.GetCriticalChance - (PlayerManager.main.FocusedCharacter.CriticalChanceBase )) + ")" + " = " + PlayerManager.main.FocusedCharacter.GetCriticalChance + " %\n");
        builder.Append("Critical damage  :  " + PlayerManager.main.FocusedCharacter.CriticalDamageBase + " (+" + (PlayerManager.main.FocusedCharacter.GetCriticalDamage - (PlayerManager.main.FocusedCharacter.CriticalDamageBase)) + ")" + " = " + PlayerManager.main.FocusedCharacter.GetCriticalDamage + " %\n");
        builder.Append("</size>");

        return builder.ToString();
    }

    public void AddStrength()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        PlayerManager.main.FocusedCharacter.StrengthBase++;
        PlayerManager.main.FocusedCharacter.NbTokenLevelup--;
    }

    public void AddConstitution()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        PlayerManager.main.FocusedCharacter.ConstitutionBase++;
        PlayerManager.main.FocusedCharacter.NbTokenLevelup--;
    }

    public void AddDexterity()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        PlayerManager.main.FocusedCharacter.DexterityBase++;
        PlayerManager.main.FocusedCharacter.NbTokenLevelup--;
    }

    public void AddIntelligence()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        PlayerManager.main.FocusedCharacter.IntelligenceBase++;
        PlayerManager.main.FocusedCharacter.NbTokenLevelup--;
    }
}
