using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow main;

    [SerializeField]
    private LootButton[] lootButtons;

    private CanvasGroup canvasGroup;

    private List<List<ItemInventory>> pages = new List<List<ItemInventory>>();
    private List<ItemInventory> currentContainerInventory;

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextBtn, previousBtn;

    public static LootWindow Main 
    {
        get
        {
            if(main == null)
            {
                main = GameObject.FindObjectOfType<LootWindow>();
            }
            return main;
        }
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }    

    public void CreatePages(List<ItemInventory> items)
    {        
        List<ItemInventory> page = new List<ItemInventory>();
        currentContainerInventory = items;
        pages.Clear();

        for(int i = 0; i < items.Count; i++)
        {
            page.Add(items[i]);
            Debug.Log(items[i].Item.Name);

            if (page.Count == 4 || i == items.Count - 1)
            {                
                pages.Add(page);
                page = new List<ItemInventory>();
            }
        }

        for (int i = 0; i < 4; i++)
        {
            lootButtons[i].gameObject.SetActive(false);
        }

        AddLoot();
        Open();
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            previousBtn.SetActive(pageIndex > 0);
            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

            for(int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].Item.Img;

                    lootButtons[i].MyLoot = pages[pageIndex][i];
                    lootButtons[i].MyLoot.Item = pages[pageIndex][i].Item;
                    lootButtons[i].MyLoot.Nb = pages[pageIndex][i].Nb;                    

                    lootButtons[i].gameObject.SetActive(true);

                    string title = null;

                    switch (pages[pageIndex][i].Item.Rarity)
                    {
                        case RARITY.E_COMMON:
                            title = string.Format("<color={0}>{1}</color>", Color.white, pages[pageIndex][i].Item.Name);
                            break;

                        case RARITY.E_RARE:
                            title = string.Format("<color={0}>{1}</color>", Color.green, pages[pageIndex][i].Item.Name);
                            break;

                        case RARITY.E_EPIC:
                            title = string.Format("<color={0}>{1}</color>", Color.magenta, pages[pageIndex][i].Item.Name);
                            break;

                        case RARITY.E_LEGENDARY:
                            title = string.Format("<color={0}>{1}</color>", Color.yellow, pages[pageIndex][i].Item.Name);
                            break;

                        case RARITY.E_UNIC:
                            title = string.Format("<color={0}>{1}</color>", Color.red, pages[pageIndex][i].Item.Name);
                            break;
                    }                    

                    lootButtons[i].MyTitle.text = title;
                    lootButtons[i].MyStack.text = lootButtons[i].MyLoot.Nb.ToString();
                }
            }
        }
    }

    public void ClearButtons()
    {
        foreach(LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if(pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(ItemInventory loot)
    {
        pages[pageIndex].Remove(loot);
        currentContainerInventory.Remove(loot);

        if(pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);

            if(pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }
        }
        ClearButtons();
        AddLoot();
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
