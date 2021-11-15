using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Container : MonoBehaviour
{

    [SerializeField] private List<ItemInventory> inventory = new List<ItemInventory>();
    [SerializeField] private List<GameObject> objetWhitItem = new List<GameObject>();
    [SerializeField] private int gold=0;
    [SerializeField] private int placeMax =5;
    bool done = false;

    private void Start()
    {
        for(int it= 0; it < objetWhitItem.Count; it++)
        {
            PutInInventory(objetWhitItem[it].GetComponent<Pickable>());
        }
    }

    private bool PutInInventory(Pickable _obj, int _place = -1, int _nb=1)
    {
        if (_nb == 0) return false;
        Collider coll;
        if (_obj.TryGetComponent<Collider>(out coll)) coll.enabled = false;
        MeshRenderer meshoui;
        if (_obj.TryGetComponent<MeshRenderer>(out meshoui)) meshoui.enabled = false;

        int firstFree = -1;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (   inventory[i].Nb != 0
                && inventory[i].Item.Name == _obj.Name
                && (_obj as Equipable) == null)
            {
                inventory[i].Nb = inventory[i].Nb + _nb;
                _obj.gameObject.SetActive(true);
                return true;
            }
            else if (firstFree < 0 && inventory[i].Nb == 0)
            {
                firstFree = i;
            }
        }
        
        if (inventory.Count < PlaceMax)
        {
            ItemInventory aAdd = new ItemInventory(_obj, _nb);
            _obj.gameObject.SetActive(true);
           if(firstFree>=0) inventory.Add( aAdd);
           else inventory.Add( aAdd);
            return true;
        }
        return false;
    }

    public List<ItemInventory> Inventory { get => inventory; set => inventory = value; }
    public int Golds { get => gold; set => gold = value; }
    public int PlaceMax { get => placeMax; set => placeMax = value; }

    public ItemInventory GetItemInvotary(int _nb)
    {
        if(_nb < inventory.Count)
        {
            ItemInventory aRemove = inventory[_nb];
            inventory.Remove(inventory[_nb]);
            return aRemove;
        }
        else
        {
            ItemInventory truvnull = new ItemInventory(null, 0);
            return truvnull;
        }
    }
    public ItemInventory GetItemInvotary(string _name)
    {
        for(int i=0; i<inventory.Count; i++)
        {
            if(inventory[i].Item.Name == _name)
            {
                ItemInventory aRemove = inventory[i];
                inventory.Remove(inventory[i]);
                return aRemove;
            }
        }

        ItemInventory truvnull = new ItemInventory(null,0);

        return truvnull;
    }

    public List<ItemInventory> GetAll()
    {
        List<ItemInventory> all = Inventory;
        inventory.RemoveRange(0, inventory.Count-1);
        return all;
    }
    public void PutInto(ItemInventory _itInvotary)
    {
        inventory.Add(_itInvotary);
    }

    public void PutMonnaie(int _nb)
    {
        if(_nb ==0)
        {
            Golds += _nb;
            PutInInventory(ObjManager.main.Golds.GetComponent<Pickable>(), -1, Golds);
           placeMax++;

        }
        else
        {
            PutInInventory(ObjManager.main.Golds.GetComponent<Pickable>(),-1, _nb);
        }
    }

    public void MergeListObject(List<GameObject> _listObject)
    {
        for(int i =0; i < _listObject.Count; i++)
        {
           PutInInventory(_listObject[i].GetComponent<Pickable>());
        }
        if(Golds>0)
        {
            ItemInventory aAdd = new ItemInventory(ObjManager.main.Golds.GetComponent<Pickable>(), Golds);
            inventory.Add(aAdd);
            placeMax++;
        }
    }

    public void ShowLoot()
    {
        if (gold > 0 && !done)
        {
            PutInInventory(ObjManager.main.Golds.GetComponent<Pickable>(), -1, gold);
            done = true;
        }
        AkSoundEngine.PostEvent("OpenLoot_Play", gameObject); // Play Loot Sound
        LootWindow.Main.CreatePages(inventory);
    }

}
