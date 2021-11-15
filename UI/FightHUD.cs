using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHUD : UI, IComparer<Portrait>
{
    public static FightHUD main = null;

    private Fight currentFight;
    [SerializeField] GameObject prefabPortrait;

    List<Portrait> listPortrait = new List<Portrait>();
    List<Portrait> listPortraitNextTurn = new List<Portrait>();
    float offsetPortrait = 140.0f;

    public float OffsetPortrait { get => offsetPortrait; }

    void Start()
    {
        main = this;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public int Compare(Portrait _portrait_1, Portrait _portrait_2)
    {
        return (_portrait_1.GetCharacter().GetMobility < _portrait_2.GetCharacter().GetMobility) ? 1 : (_portrait_1.GetCharacter().GetMobility > _portrait_2.GetCharacter().GetMobility) ? -1 : 0;
    }

    public void InitializeTimeline(Fight _fight) //A rename après les tests
    {
        currentFight = _fight;
        for (int p = 0; p < listPortrait.Count; p++)
        {
            Destroy(listPortrait[p].gameObject);
            Destroy(listPortraitNextTurn[p].gameObject);
        }
        listPortrait.Clear();
        listPortraitNextTurn.Clear();
        Portrait newPortrait;

        for (int i = 0; i < currentFight.fighters.Count; i++)
        {
            newPortrait = Instantiate(prefabPortrait, gameObject.transform).GetComponent<Portrait>();
            newPortrait.gameObject.SetActive(false);
            newPortrait.SetCharacter(currentFight.fighters[i].character);
            listPortrait.Add(newPortrait);
            newPortrait = Instantiate(prefabPortrait, gameObject.transform).GetComponent<Portrait>();
            newPortrait.gameObject.SetActive(false);
            newPortrait.SetCharacter(currentFight.fighters[i].character);
            listPortraitNextTurn.Add(newPortrait);
        }

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    void Update()
    {
        if (currentFight)
        {
            listPortraitNextTurn.Sort(this);

            if (currentFight.IsNewTurn)
            {
                for (int i = 0; i < Mathf.Min(currentFight.fighters.Count, listPortrait.Count); i++)
                {
                    listPortrait[i].SetCharacter(currentFight.fighters[i].character);
                }

                List<Portrait> tmp = listPortraitNextTurn;
                listPortraitNextTurn = listPortrait;
                listPortrait = tmp;
                currentFight.IsNewTurn = false;

                for (int i = 0; i < Mathf.Min(currentFight.fighters.Count, listPortrait.Count); i++)
                {
                    listPortrait[i].TargetPosition = gameObject.transform.position + new Vector3(OffsetPortrait / 2, 0, 0) + (((i - currentFight.Current + listPortrait.Count * 2) % (listPortrait.Count * 2)) - listPortrait.Count) * Vector3.right * OffsetPortrait * transform.localScale.x;
                    listPortraitNextTurn[i].TargetPosition = gameObject.transform.position + new Vector3(OffsetPortrait / 2, 0, 0) + (((i - currentFight.Current))) * Vector3.right * OffsetPortrait * transform.localScale.x + 20 * Vector3.up;
                }
            }

            for (int i = 0; i < listPortrait.Count; i++)
            {
                if (i < currentFight.Current)
                {
                    listPortrait[i].SetCharacter(listPortraitNextTurn[i].GetCharacter());
                }
                else
                {
                    listPortrait[i].SetCharacter(currentFight.fighters[i].character);
                }
                listPortrait[i].TargetPosition = gameObject.transform.position + new Vector3(OffsetPortrait / 2, 0, 0) + (((i - currentFight.Current + listPortrait.Count * 2) % (listPortrait.Count * 2)) - listPortrait.Count) * Vector3.right * OffsetPortrait * transform.localScale.x;
            }

            for (int i = 0; i < listPortraitNextTurn.Count; i++)
            {
                listPortraitNextTurn[i].TargetPosition = gameObject.transform.position + new Vector3(OffsetPortrait / 2, 0, 0) + (((i - currentFight.Current))) * Vector3.right * OffsetPortrait * transform.localScale.x + 20 * Vector3.up;
            }

            while (currentFight.updateTimeLine > 0)
            {
                Portrait newPortrait = Instantiate(prefabPortrait, gameObject.transform).GetComponent<Portrait>();
                newPortrait.gameObject.SetActive(false);
                newPortrait.SetCharacter(currentFight.fighters[currentFight.fighters.Count - currentFight.updateTimeLine].character);
                listPortrait.Add(newPortrait);
                newPortrait = Instantiate(prefabPortrait, gameObject.transform).GetComponent<Portrait>();
                newPortrait.gameObject.SetActive(false);
                newPortrait.SetCharacter(currentFight.fighters[currentFight.fighters.Count - currentFight.updateTimeLine].character);
                listPortraitNextTurn.Add(newPortrait);
                --currentFight.updateTimeLine;
            }
        }
    }

    public void EndCombat()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        currentFight = null;
        for (int p = 0; p < listPortrait.Count; p++)
        {
            Destroy(listPortrait[p].gameObject);
            Destroy(listPortraitNextTurn[p].gameObject);
        }
        listPortrait.Clear();
        listPortraitNextTurn.Clear();
    }

    public void DestroyPortrait(int _index)
    {
        for (int i = 0; i < listPortraitNextTurn.Count; ++i)
        {
            if (listPortraitNextTurn[i].GetCharacter() == listPortrait[_index].GetCharacter())
            {
                Destroy(listPortraitNextTurn[i].gameObject);
                listPortraitNextTurn.RemoveAt(i);
                break;
            }
        }
        Destroy(listPortrait[_index].gameObject);
        listPortrait.RemoveAt(_index);
    }
}
