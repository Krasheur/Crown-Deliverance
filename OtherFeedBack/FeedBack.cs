using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FeedBack : MonoBehaviour
{
    static public FeedBack main;
    [SerializeField] GameObject text;
    [SerializeField] CharacterFightMarker markerPrefab;

    private void Awake()
    {
        if (main == null)
            main = this;
    }


    static public void HighlightEntity(Entity _entity)
    {
        if (_entity)
        {
            CharacterFightMarker marker = _entity.GetComponentInChildren<CharacterFightMarker>();
            if (!marker)
            {
                marker = Instantiate(main.markerPrefab, _entity.transform);
                MeshFilter mf = _entity.GetComponent<MeshFilter>();
                if (mf)
                {
                    if (mf.sharedMesh)
                    {
                        marker.transform.localPosition = mf.sharedMesh.bounds.min;
                    }
                    else if (mf.mesh)
                    {
                        marker.transform.localPosition = mf.mesh.bounds.min;
                    }
                    Vector3 pos = marker.transform.localPosition;
                    pos.x = 0;
                    pos.z = 0;
                    marker.transform.localPosition = pos;
                    marker.transform.localScale = Vector3.one * 2.0f;
                }
            }
            marker.Selected = true;
        }
    }

    public void CreateNewText(Vector3 _pos, string _text, Color _color)
    {
        GameObject feedBack = Instantiate(text);
        feedBack.transform.position = _pos;
        TextMeshPro txtMeshPro = feedBack.GetComponent<TextMeshPro>();
        txtMeshPro.text = _text;
        txtMeshPro.color = _color;
        txtMeshPro.enabled = false;
        feedBack.AddComponent<DamageComportement>().Init();
    }

    public void FeedBackHit(Vector3 _pos, DamageStruct _damageStruct)
    {
        //txtMeshPro.text = _damageStruct.amountDamag.ToString();

        if (_damageStruct.dodged)
        {
            CreateNewText(_pos, "Dodge", Color.grey);
        }
        else
        {
            string crit = (_damageStruct.criticalHit > 0) ? "Critical Hit  " : "";
            if (_damageStruct.amountDamagToHp > 0)
            {
                CreateNewText(_pos, crit + _damageStruct.amountDamagToHp,
                    (_damageStruct.criticalHit > 0) ? new Color(0.7f, 0.5f, 0.1f) : Color.red);
            }
            if (_damageStruct.amountDamagToArmor > 0)
            {
                CreateNewText(_pos, crit + _damageStruct.amountDamagToArmor,
                    (_damageStruct.criticalHit > 0) ? new Color(1f, 0.9f, 0.4f) : Color.white);
            }
            if (_damageStruct.amountHeal > 0)
            {
                CreateNewText(_pos, _damageStruct.amountHeal.ToString(), Color.cyan);
            }
            if (_damageStruct.amountArmor > 0)
            {
                CreateNewText(_pos, _damageStruct.amountArmor.ToString(), Color.blue);
            }
        }
    }
    public void LevelUpTxt(Vector3 _pos)
    {
        GameObject feedBack = Instantiate(text);
        feedBack.transform.position = _pos;
        feedBack.AddComponent<LevelUpTextFeedBack>();
    }
    public void XpOwnTxt(Vector3 _pos, int _amount)
    {
        GameObject feedBack = Instantiate(text);
        feedBack.transform.position = _pos;
        feedBack.AddComponent<OwnXpTxt>();
        feedBack.GetComponent<OwnXpTxt>().Amount = _amount;

    }

}
