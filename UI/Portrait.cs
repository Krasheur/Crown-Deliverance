using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Portrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Coroutine coroutineMove;

    static bool movingPortrait = false;

    protected Character character; //Character used for the portrait

    private Image image;

    private int lastPV; //Character PVs in the last frame
    private int lastARMOR; //Character ARMORs in the last frame

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider armorSlider;
    [SerializeField] GameObject alteration_images;

    [SerializeField] string initialClasse;

    [SerializeField] private ToolTipPopUp toolTipPopUp;

    [SerializeField] GameObject panelDeath;

    AlterationIcon[] alterations = null;

    PlayerManager pm;
    CharacterFightMarker marker;
    bool isCasting;
    float timer;
    Vector3 targetPosition = Vector3.zero;
    public Character Character { get => character; set => character = value; }

    public Vector3 TargetPosition
    {
        get => targetPosition;
        set
        {
            targetPosition = value;
            if (!gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                transform.position = targetPosition;
                coroutineMove = null;
                gameObject.SetActive(true);
                return;
            }
            if (coroutineMove == null)
            {
                if (Mathf.Abs(targetPosition.x - transform.position.x) > FightHUD.main.OffsetPortrait * FightHUD.main.transform.localScale.x)
                {
                    coroutineMove = StartCoroutine(Move());
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }
    }    

    void Start()
    {
        image = GetComponentInChildren<Image>();
        image.material = new Material(image.material);
        if (initialClasse == "Fairy")
        {
            Character = PlayerManager.main.PlayerFairy;
            image.sprite = Character.Portrait;            
            healthSlider.maxValue = Character.PvMax;
            armorSlider.maxValue = Character.ArmorMax;
        }
        else if(initialClasse == "Rogue")
        {
            Character = PlayerManager.main.PlayerRogue;
            image.sprite = Character.Portrait;            
            healthSlider.maxValue = Character.PvMax;
            armorSlider.maxValue = Character.ArmorMax;
        }
        else if (initialClasse == "Warrior")
        {
            Character = PlayerManager.main.PlayerTank;
            image.sprite = Character.Portrait;            
            healthSlider.maxValue = Character.PvMax;
            armorSlider.maxValue = Character.ArmorMax;
        }

        lastPV = 0;
        lastARMOR = 0;
        healthSlider.minValue = 0;
        armorSlider.minValue = 0;    
        
        toolTipPopUp = GameObject.Find("TooltipPopUp").GetComponent<ToolTipPopUp>();
        pm = PlayerManager.main;       
        panelDeath.SetActive(false);
    }

    IEnumerator Move()
    {
        float factor = 0.0f;
        Vector3 lastPos = transform.position;

        while (movingPortrait)
        {
            yield return null;
        }
        movingPortrait = true;

        while (factor < 1)
        {
            factor = Mathf.Min(factor + Time.deltaTime * 4.0f, 1);

            if (factor < 1)
            {
                transform.position = Vector3.Lerp(lastPos, targetPosition, factor) - Mathf.Pow(Mathf.Sin(factor * Mathf.PI), 0.05f) * Vector3.up * 100;
            }
            else
            {
                transform.position = targetPosition;
            }

            yield return null;
        }
        movingPortrait = false;
        coroutineMove = null;
    }

    // Update is called once per frame
    void Update()
    {       
        if(healthSlider.value != character.PV)
        {
            healthSlider.value = character.PV;
        }

        if (armorSlider.value != character.Armor)
        {
            armorSlider.value = character.Armor;
        }

        if (character.IsDead)
        {
            panelDeath.SetActive(true);
        }
        else
        {
            panelDeath.SetActive(false);
        }

        if (pm.FocusedCharacter.SpellCasted)
        {
            timer = 0;
            isCasting = true;
        }
        else
        {
            timer += Time.deltaTime;          
        }

        if(timer > 1.0f && isCasting)
        {
            isCasting = false;
            timer = 0;
        }

        if (character == pm.PlayerFairy || character == pm.PlayerTank || character == pm.PlayerRogue)
        {
            if (pm.FocusedCharacter.IsDead)
            {
                if (!pm.PlayerFairy.IsDead)
                {
                    CameraBehaviour.instance.target = pm.PlayerFairy.GetComponent<NavMeshAgent>();
                    pm.FocusedCharacter = pm.PlayerFairy;
                }
                else if (!pm.PlayerRogue.IsDead)
                {
                    CameraBehaviour.instance.target = pm.PlayerRogue.GetComponent<NavMeshAgent>();
                    pm.FocusedCharacter = pm.PlayerRogue;
                }
                else if(!pm.PlayerTank.IsDead)
                {
                    CameraBehaviour.instance.target = pm.PlayerTank.GetComponent<NavMeshAgent>();
                    pm.FocusedCharacter = pm.PlayerTank;
                }
            }
        }

        marker = character.GetComponentInChildren<CharacterFightMarker>();
        if (marker)
        {
            image.material.SetInt("selected", marker.Selected ? 1 : 0);
            image.material.SetColor("_Color", marker.Color);
        }
        else
        {
            image.material.SetInt("selected", 0);
        }       
    }

    public Character GetCharacter()
    {
        return character;
    }

    public void SetCharacter(Character _character)
    {
        character = _character;
        Material mat = image != null ? image.material : null;
        image = GetComponentInChildren<Image>();
        mat = mat == null ? image.material : mat;
        image.sprite = character.Portrait;
        image.material = mat;

        lastPV = 0;

        healthSlider.minValue = 0;
        healthSlider.maxValue = character.PvMax;
        healthSlider.value = character.PV;

        armorSlider.minValue = 0;
        armorSlider.maxValue = character.ArmorMax;
        armorSlider.value = character.Armor;
    }

    public string GetTooltipInfoText()
    {                 
        return character.GetTooltipInfoText();
    }

    IEnumerator SelectCharacter()
    {
        while (true)
        {
            FeedBack.HighlightEntity(character);
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(SelectCharacter());
        toolTipPopUp.DisplayInfoPortrait(this);
        VisuPortraitsPermHUD.FocusedPortrait = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        toolTipPopUp.HideInfo();
        VisuPortraitsPermHUD.FocusedPortrait = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {        
        if (pm.FocusedCharacter.State == CHARACTER_STATE.FREE && character.GetComponent<Character>().enabled && !isCasting)
        {
            CameraBehaviour.instance.target = character.GetComponent<NavMeshAgent>();
            pm.FocusedCharacter = character;
        }        
    }
}
