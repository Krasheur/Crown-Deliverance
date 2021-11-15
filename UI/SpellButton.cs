using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour
{
    [SerializeField] Portrait activePortrait;
    [SerializeField] Button spell_1;
    [SerializeField] Image spellCooldown_1;
    [SerializeField] Button spell_2;
    [SerializeField] Image spellCooldown_2;
    [SerializeField] Button spell_3;
    [SerializeField] Image spellCooldown_3;
    [SerializeField] Button spell_4;
    [SerializeField] Image spellCooldown_4;

    Character localcharacter;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (localcharacter != activePortrait.GetCharacter())
        {
            localcharacter = activePortrait.GetCharacter();
            InitializeSpells();
        }

        if(localcharacter.SpellList[0].TurnsRemaining != 0)
        {
            if (localcharacter.State == CHARACTER_STATE.FIGHT)
            {
                spellCooldown_1.fillAmount = (float)localcharacter.SpellList[0].TurnsRemaining / (float)localcharacter.SpellList[0].Cooldown;
            }
            else
            {
                spellCooldown_1.fillAmount = (float)(localcharacter.SpellList[0].TurnsRemaining - (localcharacter.SpellList[0].Timer / Fight.turnTime)) / localcharacter.SpellList[0].Cooldown;
            }

            spell_1.GetComponentInChildren<Text>().enabled = true;
        } 
        else
        {
            spell_1.GetComponentInChildren<Text>().enabled = false;
            spellCooldown_1.fillAmount = 0;
        }

        if (localcharacter.SpellList[1].TurnsRemaining != 0)
        {
            if (localcharacter.State == CHARACTER_STATE.FIGHT)
            {
                spellCooldown_2.fillAmount = (float)localcharacter.SpellList[1].TurnsRemaining / (float)localcharacter.SpellList[1].Cooldown;
            }
            else
            {
                spellCooldown_2.fillAmount = (float)(localcharacter.SpellList[1].TurnsRemaining - (localcharacter.SpellList[1].Timer / Fight.turnTime)) / localcharacter.SpellList[1].Cooldown;
            }
            spell_2.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {            
            spell_2.GetComponentInChildren<Text>().enabled = false;
            spellCooldown_2.fillAmount = 0;
        }

        if (localcharacter.SpellList[2].TurnsRemaining != 0)
        {
            if (localcharacter.State == CHARACTER_STATE.FIGHT)
            {
                spellCooldown_3.fillAmount = (float)localcharacter.SpellList[2].TurnsRemaining / (float)localcharacter.SpellList[2].Cooldown;
            }
            else
            {
                spellCooldown_3.fillAmount = (float)(localcharacter.SpellList[2].TurnsRemaining - (localcharacter.SpellList[2].Timer / Fight.turnTime)) / localcharacter.SpellList[2].Cooldown;
            }
            spell_3.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {           
            spell_3.GetComponentInChildren<Text>().enabled = false;
            spellCooldown_3.fillAmount = 0;
        }

        if (localcharacter.SpellList[3].TurnsRemaining != 0)
        {
            if (localcharacter.State == CHARACTER_STATE.FIGHT)
            {
                spellCooldown_4.fillAmount = (float)localcharacter.SpellList[3].TurnsRemaining / (float)localcharacter.SpellList[3].Cooldown;
            }
            else
            {
                spellCooldown_4.fillAmount = (float)(localcharacter.SpellList[3].TurnsRemaining - (localcharacter.SpellList[3].Timer / Fight.turnTime)) / localcharacter.SpellList[3].Cooldown;
            }
            spell_4.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {            
            spell_4.GetComponentInChildren<Text>().enabled = false;
            spellCooldown_4.fillAmount = 0;
        }

        spell_1.GetComponentInChildren<Text>().text = "" + localcharacter.SpellList[0].TurnsRemaining;
        spell_2.GetComponentInChildren<Text>().text = "" + localcharacter.SpellList[1].TurnsRemaining;
        spell_3.GetComponentInChildren<Text>().text = "" + localcharacter.SpellList[2].TurnsRemaining;
        spell_4.GetComponentInChildren<Text>().text = "" + localcharacter.SpellList[3].TurnsRemaining;
    }

    void InitializeSpells()
    {
        spell_1.GetComponent<Image>().sprite = localcharacter.SpellList[0].Spell.Thumbnail;
        spell_1.GetComponent<ToolTipSpell>().spell = localcharacter.SpellList[0];        
        

        spell_2.GetComponent<Image>().sprite = localcharacter.SpellList[1].Spell.Thumbnail;
        spell_2.GetComponent<ToolTipSpell>().spell = localcharacter.SpellList[1];
        

        spell_3.GetComponent<Image>().sprite = localcharacter.SpellList[2].Spell.Thumbnail;
        spell_3.GetComponent<ToolTipSpell>().spell = localcharacter.SpellList[2];
        

        spell_4.GetComponent<Image>().sprite = localcharacter.SpellList[3].Spell.Thumbnail;
        spell_4.GetComponent<ToolTipSpell>().spell = localcharacter.SpellList[3];
        
    }    

    public void CastSpell(int i)
    {
        if (localcharacter.State != CHARACTER_STATE.LOCKED && Vector3.Distance(localcharacter.transform.position, localcharacter.NavAgent.destination) < 1.8f
            && localcharacter.CurrentPa >= localcharacter.SpellList[i].Spell.Cost)
        {
            localcharacter.SpellList[i].Activate();                                 
        }
    }
}
