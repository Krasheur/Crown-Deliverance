using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Character character;
    [SerializeField] GameObject box;
    [SerializeField] bool isKing = false;

    float timer = 0f;
    bool isDead = false;

    public Character Character { get => character; set => character = value; }

    Rigidbody[] rbs;

    private void Update()
    {
        if (!isDead)
        {
            if (isKing)
            {
                Time.timeScale = 0.5f;
            }            

            timer += Time.deltaTime;
            if (timer > 2f)
            {
                isDead = true;

                if (!isKing)
                {
                    GameObject loot = Instantiate(box, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                    ItemGenerator itGen = GetComponentInChildren<ItemGenerator>();
                    if (itGen != null)
                    {
                        itGen.gameObject.transform.parent = loot.transform;
                    }

                    loot.GetComponent<Chest>().Owner = this;

                    Container thisOne = null;
                    loot.TryGetComponent<Container>(out thisOne);
                    if (thisOne == null)
                    {
                        thisOne = loot.AddComponent<Container>();

                        for (int it = 0; it < character.Inventory.Length; it++)
                        {
                            int rand = Random.Range(1, 11);
                            if (character.Inventory[it].Nb > 0 && (rand <= 9 || character.Inventory[it].Item.Rarity == RARITY.E_UNIC)) thisOne.PutInto(character.Inventory[it]);
                        }
                        thisOne.PutMonnaie(character.Gold);
                    }
                }                
            }
        }
        else
        {
            Time.timeScale = 1;

            rbs = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = true;
            }

            this.enabled = false;
        }
    }
}
