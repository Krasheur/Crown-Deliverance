using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


public enum RARITY
{
    E_COMMON,   
    E_RARE,     
    E_LEGENDARY,
    E_EPIC,     
    E_UNIC, 
    E_COUNT,
}

public class Pickable : Entity
{
    [Space(10)]
    [Header("Pickable")]
    [SerializeField] protected RARITY rarity;
    protected OnString onGetDescription;
    [SerializeField] protected Sprite img;
    [SerializeField] protected int value;
    private Vector3 startPos;
    private bool canTake= true;

    private bool takeHim = false;
    private Character entity;
    private float progress = 0f;

    private bool eject = false;
    private Vector3 targetFly;

    public OnString OnGetDescription { get => onGetDescription; set => onGetDescription = value; }

    public RARITY Rarity { get => rarity; set => rarity = value; }
    public bool CanTake { get => canTake; }
    public Sprite Img { get => img; set => img = value; }
    public int Value { get => value; set => this.value = value; }

    protected override void  Awake()
    {
        base.Awake();
    }
    protected override void  Start()
    {
        base.Start();
        onGetDescription += Description;
    }

    void Description(ref StringBuilder _descriptionConcat)
    {
        _descriptionConcat.Append("<size=35>"); 
        switch (rarity)
        {
            case RARITY.E_COMMON:
                _descriptionConcat.Append("<color=white>");
                break;
            case RARITY.E_RARE:
                _descriptionConcat.Append("<color=green>");
                break;
            case RARITY.E_LEGENDARY:
                _descriptionConcat.Append("<color=purple>");
                break;
            case RARITY.E_EPIC:
                _descriptionConcat.Append("<color=blue>");
                break;
            case RARITY.E_UNIC:
                _descriptionConcat.Append("<color=yellow>");
                break;
            default:
                _descriptionConcat.Append("<color=white>");
                break;
        }
        _descriptionConcat.Append(name).Append("</size></color>\n").AppendLine();

        if(transform.GetComponentInChildren<ItemEffectAddArmor>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectAddArmor>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectHeal>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectHeal>().GetDescription());
        }        
    }
    public void TakeHim(Character _entity)
    {
        if(canTake)
        {
            takeHim = true;
            entity = _entity;
            startPos = this.transform.position;
            canTake = false;

            Collider coll;
            if (TryGetComponent<Collider>(out coll)) coll.enabled = false;

        }
    }

    public void EjectHim(Vector3 _position, Vector3 _forward)
    {
        MeshRenderer meshoui;
        if (TryGetComponent<MeshRenderer>(out meshoui)) meshoui.enabled = true;

        eject = true;
        this.transform.position = _position;
        canTake = false;

        RaycastHit ray;
        if(Physics.Raycast((_position +_forward*5),( - Vector3.up*10),out ray))
        {
            targetFly = ray.point;
            CapsuleCollider capsColl;
            if(TryGetComponent<CapsuleCollider>(out capsColl)) targetFly.y += capsColl.radius;
            BoxCollider boxColl;
            if(TryGetComponent<BoxCollider>(out boxColl)) targetFly.y += boxColl.size.y*0.5f;
            SphereCollider sphereColl;
            if(TryGetComponent<SphereCollider>(out sphereColl)) targetFly.y += sphereColl.radius;
        }
        else
        {
            targetFly = _position + _forward * 5;
        }


        startPos = _position;
    }


    override protected void Update()
    {
        base.Update();
        if (takeHim)
        {
            progress += (Time.deltaTime * 1.1f) ;
            transform.position = (1.0f - progress) *  startPos + progress * entity.transform.position + Vector3.up * Mathf.Sin(progress * Mathf.PI) * 1.2f;

            if (progress >= 1)
            {
                MeshRenderer meshoui;
                if (TryGetComponent<MeshRenderer>(out meshoui)) meshoui.enabled = false;
                entity.PutInInventory(this);
                takeHim = false;
                progress = 0;
                entity = null;
                canTake = true;
            }
        }
    
        if(eject)
        {

            progress += (Time.deltaTime * 1);
            transform.position = (1.0f - progress) * startPos + progress * targetFly + Vector3.up* 1.2f * Mathf.Sin(progress * Mathf.PI) ;

            if (progress >= 1)
            {
                eject = false;
                progress = 0;
                canTake = true;

                Collider coll;
                if (TryGetComponent<Collider>(out coll)) coll.enabled = true;

            }

        }
    }
}
