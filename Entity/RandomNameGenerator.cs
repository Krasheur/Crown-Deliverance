using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    [SerializeField] string[] names;
    // Start is called before the first frame update
    void Start()
    {
        Character character = GetComponentInParent<Character>();
        if (character && names != null && names.Length > 0)
        {
            character.name = names[Random.Range(0, names.Length)];
        }
        Destroy(gameObject);
    }
}
