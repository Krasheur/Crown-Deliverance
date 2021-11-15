using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterationIcon : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] string displayName;

    public Sprite Sprite { get => sprite; }
    public string DisplayName { get => displayName; set => displayName = value; }
}
