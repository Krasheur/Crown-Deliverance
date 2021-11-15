using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillTreeUi : MonoBehaviour
{
    Button[] buttons = new Button[36];

    Text[] buttonsText = new Text[36];

    [SerializeField] Text pointsText;

    [SerializeField] Sprite skillTreeFairy;
    [SerializeField] Sprite skillTreeRogue;
    [SerializeField] Sprite skillTreeTank;

    [SerializeField] Image background;

    [SerializeField] Sprite skillAvailable;
    [SerializeField] Sprite skillUnavailable;

    private Character character;

    void Start()
    {        
        for (int i = 1; i < transform.childCount; i++)
        {
            buttons[i - 1] = transform.GetChild(i).GetComponent<Button>();            
        }
    }

    void Update()
    {
        if (character != PlayerManager.main.FocusedCharacter)
        {
            character = PlayerManager.main.FocusedCharacter;
        }

        if(character == PlayerManager.main.PlayerFairy)
        {
            background.sprite = skillTreeFairy;
        }
        else if(character == PlayerManager.main.PlayerRogue)
        {
            background.sprite = skillTreeRogue;
        }
        else if (character == PlayerManager.main.PlayerTank)
        {
            background.sprite = skillTreeTank;
        }

        pointsText.text = "" + character.availableSkillTreePoints;

        if (character.GetComponent<SkillTreeReader>() != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (character.GetComponent<SkillTreeReader>().skillTree[i].unlocked)
                {
                    buttons[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                }
                else
                {
                    buttons[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                }

                if (character.GetComponent<SkillTreeReader>().IsSkillBlock(i))
                {
                    buttons[i].GetComponent<Image>().sprite = skillUnavailable;
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = skillAvailable;
                }
            }
        }
    }

    public void UnlockedVisual(int id_skill)
    {
        if (character.GetComponent<SkillTreeReader>() != null)
        {
            character.GetComponent<SkillTreeReader>().UnlockSkill(id_skill);
        }
    }
}
