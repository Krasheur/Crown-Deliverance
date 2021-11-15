using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisuActionPoints : MonoBehaviour
{
    public static VisuActionPoints main = null;
    [SerializeField] Portrait plocalcharacter;
    [SerializeField] GameObject prefabPA;

    [SerializeField] Sprite green;
    [SerializeField] Sprite yellow;
    [SerializeField] Sprite red;
    [SerializeField] Sprite blue;

    float offsetPA = 45.0f;

    Character localcharacter;

    List<GameObject> listPA = new List<GameObject>();
    int movementCost = 0;
    int spellCost = 0;

    public int MovementCost { get => movementCost; set => movementCost = value; }
    public int SpellCost { get => spellCost; set => spellCost = value; }

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        localcharacter = plocalcharacter.GetCharacter();        
        offsetPA = 44.0f;
    }

    // Update is called once per frame
    void Update()
    {        
        if (localcharacter != plocalcharacter.GetCharacter())
        {
            localcharacter = plocalcharacter.GetCharacter();
            InitializePA();
        }

        UpdateActionPoints();
    }

    void UpdateActionPoints()
    {                 
        if(localcharacter.State != CHARACTER_STATE.FIGHT)
        {
            spellCost = 0;
        }

        for (int a = 0; a < listPA.Count; ++a)
        {
            listPA[a].transform.position = gameObject.transform.position + new Vector3(offsetPA / 2, 0, 0) + (a - listPA.Count / 2.0f) * Vector3.right * offsetPA * transform.localScale.x;

            if (a < localcharacter.CurrentPa - movementCost - spellCost)
            {
                listPA[a].GetComponent<Image>().sprite = green;
            }
            else if (a < localcharacter.CurrentPa)
            {
                listPA[a].GetComponent<Image>().sprite = yellow;
            }
            else
            {
                listPA[a].GetComponent<Image>().sprite = red;
            }
        }

        if (localcharacter.MobilityMov)
        {
            if(listPA.Count>0) listPA[listPA.Count - 1].GetComponent<Image>().sprite = blue;
        }
        else
        {
            if (listPA.Count > 0) listPA[listPA.Count - 1].GetComponent<Image>().sprite = red;
        }
    }

    public void InitializePA()
    {
        for (int x = 0; x < listPA.Count; ++x)
        {
            Destroy(listPA[x].gameObject);
        }
        listPA.Clear();

        GameObject newPA;
        
        for (int i = 0; i <= localcharacter.TotalPa; i++)
        {
            newPA = Instantiate(prefabPA, gameObject.transform);
            listPA.Add(newPA);
        }
    }
}
