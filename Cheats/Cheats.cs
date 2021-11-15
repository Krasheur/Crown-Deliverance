using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cheats : MonoBehaviour
{
    static public Cheats main;

    bool cheatsActivated = false;
    public bool CheatsActivated { get => cheatsActivated; set => cheatsActivated = value; }

    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Quote))
        {
            if(CheatsActivated)
            {
                CheatsActivated = false;                
            }
            else
            {
                CheatsActivated = true;
            }
        }

        if (CheatsActivated)
        {
            if (Input.GetKeyDown(KeyCode.H)) //Heal
            {
                PlayerManager.main.PlayerFairy.PV = PlayerManager.main.PlayerFairy.PvMax;
                PlayerManager.main.PlayerFairy.Armor = PlayerManager.main.PlayerFairy.ArmorMax;
                PlayerManager.main.PlayerRogue.PV = PlayerManager.main.PlayerRogue.PvMax;
                PlayerManager.main.PlayerRogue.Armor = PlayerManager.main.PlayerRogue.ArmorMax;
                PlayerManager.main.PlayerTank.PV = PlayerManager.main.PlayerTank.PvMax;
                PlayerManager.main.PlayerTank.Armor = PlayerManager.main.PlayerTank.ArmorMax;
            }

            if (Input.GetKeyDown(KeyCode.R)) //Respawn
            {
                PlayerManager.main.PlayerFairy.PV = PlayerManager.main.PlayerFairy.PvMax;
                PlayerManager.main.PlayerFairy.Armor = PlayerManager.main.PlayerFairy.ArmorMax;
                PlayerManager.main.PlayerFairy.State = CHARACTER_STATE.FREE;
                PlayerManager.main.PlayerFairy.GetComponent<Character>().enabled = true;
                PlayerManager.main.PlayerFairy.GetComponent<NavMeshAgent>().enabled = true;

                PlayerManager.main.PlayerRogue.PV = PlayerManager.main.PlayerRogue.PvMax;
                PlayerManager.main.PlayerRogue.Armor = PlayerManager.main.PlayerRogue.ArmorMax;
                PlayerManager.main.PlayerRogue.State = CHARACTER_STATE.FREE;
                PlayerManager.main.PlayerRogue.GetComponent<Character>().enabled = true;
                PlayerManager.main.PlayerRogue.GetComponent<NavMeshAgent>().enabled = true;

                PlayerManager.main.PlayerTank.PV = PlayerManager.main.PlayerTank.PvMax;
                PlayerManager.main.PlayerTank.Armor = PlayerManager.main.PlayerTank.ArmorMax;
                PlayerManager.main.PlayerTank.State = CHARACTER_STATE.FREE;
                PlayerManager.main.PlayerTank.GetComponent<Character>().enabled = true;
                PlayerManager.main.PlayerTank.GetComponent<NavMeshAgent>().enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E)
                && PlayerManager.main.FocusedCharacter
                && PlayerManager.main.FocusedCharacter.CurrentFight) //End Turn
            {
                Fight currentFight = PlayerManager.main.FocusedCharacter.CurrentFight;

                currentFight.fighters[currentFight.current].character.SpellCasted?.Cancel();
                currentFight.EndTurn();
            }

            if (Input.GetKeyDown(KeyCode.K)) //IsKillable
            {
                if (PlayerManager.main.PlayerFairy.IsKillable)
                {
                    PlayerManager.main.PlayerFairy.IsKillable = false;
                }
                else
                {
                    PlayerManager.main.PlayerFairy.IsKillable = true;
                }

                if (PlayerManager.main.PlayerRogue.IsKillable)
                {
                    PlayerManager.main.PlayerRogue.IsKillable = false;
                }
                else
                {
                    PlayerManager.main.PlayerRogue.IsKillable = true;
                }

                if (PlayerManager.main.PlayerTank.IsKillable)
                {
                    PlayerManager.main.PlayerTank.IsKillable = false;
                }
                else
                {
                    PlayerManager.main.PlayerTank.IsKillable = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.D)) //End Dialogue
            {
                if (DialogueManager.Main.animator.GetBool("isOpen"))
                {
                    DialogueManager.Main.EndDialogue();
                }
            }

            if (Input.GetKeyDown(KeyCode.P)) //Add Skill Points
            {
                PlayerManager.main.FocusedCharacter.availableSkillTreePoints += 100;
            }

            if (Input.GetKeyDown(KeyCode.X)) //Add XP Points
            {
                PlayerManager.main.PlayerFairy.GainExp(100);
                PlayerManager.main.PlayerRogue.GainExp(100);
                PlayerManager.main.PlayerTank.GainExp(100);
            }

            if (Input.GetKeyDown(KeyCode.C)) //Reset Cooldowns
            {
                for (int i = 0; i < PlayerManager.main.PlayerFairy.SpellList.Length; i++)
                {
                    PlayerManager.main.PlayerFairy.SpellList[i].TurnsRemaining = 0;
                }

                for (int i = 0; i < PlayerManager.main.PlayerRogue.SpellList.Length; i++)
                {
                    PlayerManager.main.PlayerRogue.SpellList[i].TurnsRemaining = 0;
                }

                for (int i = 0; i < PlayerManager.main.PlayerTank.SpellList.Length; i++)
                {
                    PlayerManager.main.PlayerTank.SpellList[i].TurnsRemaining = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.V)) //Recharge PA
            {
                for (int i = 0; i < PlayerManager.main.PlayerFairy.SpellList.Length; i++)
                {
                    PlayerManager.main.PlayerFairy.CurrentPa = PlayerManager.main.PlayerFairy.TotalPa;
                    PlayerManager.main.PlayerFairy.MobilityMov = true;
                }

                for (int i = 0; i < PlayerManager.main.PlayerRogue.SpellList.Length; i++)
                {
                    PlayerManager.main.PlayerRogue.CurrentPa = PlayerManager.main.PlayerRogue.TotalPa;
                    PlayerManager.main.PlayerRogue.MobilityMov = true;
                }

                for (int i = 0; i < PlayerManager.main.PlayerTank.SpellList.Length; i++)
                {
                    PlayerManager.main.PlayerTank.CurrentPa = PlayerManager.main.PlayerTank.TotalPa;
                    PlayerManager.main.PlayerTank.MobilityMov = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.M)) //Enable/Disabled Cursor
            {
                if (FollowCursor.main.GetComponentInParent<CanvasGroup>().alpha == 1)
                {
                    FollowCursor.main.HideCursor();
                }
                else
                {
                    FollowCursor.main.ShowCursor();
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Character[] characters = FindObjectsOfType<Character>();
                DamageStruct dmg = new DamageStruct();
                dmg.amountDamag = 1000000000;
                for (int i = 0; i < characters.Length; i++)
                {
                    if (characters[i].Hostility == CHARACTER_HOSTILITY.ENEMY) characters[i].ChangePv(dmg);
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PlayerManager.main && PlayerManager.main.FocusedCharacter
                    && PlayerManager.main.FocusedCharacter.CurrentFight)
                {
                    DamageStruct dmg = new DamageStruct();
                    dmg.amountDamag = 1000000000;
                    Fight fight = PlayerManager.main.FocusedCharacter.CurrentFight;
                    for (int i = 0; i < fight.fighters.Count; i++)
                    {
                        if (fight.fighters[i].character.Hostility == CHARACTER_HOSTILITY.ENEMY) fight.fighters[i].character.ChangePv(dmg);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                PauseMenu pause = FindObjectOfType<PauseMenu>();
                if (pause) pause.transform.parent.GetComponent<Canvas>().enabled = !pause.transform.parent.GetComponent<Canvas>().enabled;
            }
        }
    }
}
