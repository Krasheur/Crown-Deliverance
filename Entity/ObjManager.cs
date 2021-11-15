using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjManager : MonoBehaviour
{
    public static ObjManager main;

    [Header("Item Base")]
    [SerializeField] public GameObject[] objectTypeConsommable;
    [SerializeField] public GameObject[] objectTypeArmorFee;
    [SerializeField] public GameObject[] objectTypeWeaponFee;
    [SerializeField] public GameObject[] objectTypeArmorRogue;
    [SerializeField] public GameObject[] objectTypeWeaponRogue;
    [SerializeField] public GameObject[] objectTypeArmorTank;
    [SerializeField] public GameObject[] objectTypeWeaponTank;
    [SerializeField] public GameObject[] objectTypeWeaponShield;

    [Space(10)]
    [Header("Item Effect")]
    [SerializeField] public List<GameObject> itemEffectArmorFee;
    [SerializeField] public List<GameObject> itemEffectArmorRogue;
    [SerializeField] public List<GameObject> itemEffectArmorTank;
    [SerializeField] public List<GameObject> itemEffectWeaponFee;
    [SerializeField] public List<GameObject> itemEffectWeaponRogue;
    [SerializeField] public List<GameObject> itemEffectWeaponTank;
    [SerializeField] public List<GameObject> itemEffectConsummable;
    [SerializeField] public List<GameObject> itemEffectNotSpecificly;
    [Space(3)]
    [Header("ItemEffect for high Level")]
    [SerializeField] public List<GameObject> itemEffectAlterationWeapon;
    [SerializeField] public List<GameObject> itemEffectAlterationArmor;

    [Header("Sprite Popo")]
    [SerializeField] public Sprite[] imgPopoConsommableHeal;
    [SerializeField] public Sprite[] imgPopoConsommableArmor;

    [Space(10)]
    [SerializeField] GameObject golds;

    public GameObject Golds { get => golds; }


    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;

        golds = Instantiate(Golds);
        SceneManager.MoveGameObjectToScene(golds, SceneManager.GetSceneByName("Permanent"));
        golds.name = "Gold";
        Collider coll;
        if (golds.TryGetComponent<Collider>(out coll)) coll.enabled = false;
        MeshRenderer meshoui;
        if (golds.TryGetComponent<MeshRenderer>(out meshoui)) meshoui.enabled = false;

    }
}
