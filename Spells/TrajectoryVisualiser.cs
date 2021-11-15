using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrajectoryVisualiser : MonoBehaviour
{
    Material mat;
    Material mat2;
    public Vector3 startPos;
    public Vector3 position;
    public float elevation;
    public float interruption;
    public float radius;
    public int locked = 0;
    public Color colorArea;
    Spell spell;

    List<CharacterFightMarker> lastMarkers;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat2 = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        lastMarkers = new List<CharacterFightMarker>();
        spell = transform.parent.GetComponent<Spell>();
    }

    // Update is called once per frame
    void Update()
    {
        startPos = spell.FirePoint;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = position;
        mat.SetFloat("elevation", elevation);
        mat.SetFloat("interruption", interruption);
        mat.SetInt("locked", locked);
        mat.SetVector("endPos", transform.position);
        mat.SetVector("startPos", startPos);
        transform.GetChild(0).localScale = Vector3.one * (radius * 2.0f);
        mat2.SetVector("_Color", colorArea);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, Physics.AllLayers/*LayerMask.NameToLayer("Entity")*/, QueryTriggerInteraction.Ignore);
        if (colliders != null && colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                Character character = colliders[i].gameObject.GetComponent<Character>();
                if (character && !character.IsDead && !Physics.Linecast(character.transform.position, transform.position + (character.transform.position - transform.position).normalized * 0.01f, (1 << LayerMask.NameToLayer("Environnement")) | (1)))
                {
                    if (character)
                    {
                        FeedBack.HighlightEntity(character);
                    }
                }
            }
        }
    }
}
