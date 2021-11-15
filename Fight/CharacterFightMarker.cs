using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightMarkerState
{
    ALLY,
    ALLY_SELECTED,
    NEUTRAL,
    NEUTRAL_SELECTED,
    HOSTILE,
    HOSTILE_SELECTED,
}

public class CharacterFightMarker : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] FightMarkerState state;
    [SerializeField] Material matHighlight;
    bool justSelected = false;
    Material mat = null;
    Character character = null;
    Entity entity = null;
    bool selected = false;
    MeshRenderer meshR;
    SkinnedMeshRenderer skinMeshR;
    Container container;
    MeshRenderer childMR;

    public FightMarkerState State { get => state; set => state = value; }
    public bool Selected
    {
        get => selected;
        set
        {
            selected = value;
            if (selected) justSelected = true;
        }
    }
    public Color Color
    {
        get
        {
            return colors[(int)state];
        }
    }


    private SkinnedMeshRenderer GetSkinnedMeshRenderer(Transform _trsf)
    {
        SkinnedMeshRenderer comp;
        if (_trsf.name == "mesh_GRP")
        {
            comp = _trsf.GetComponentInChildren<SkinnedMeshRenderer>();
            if (comp != null) return comp;
        }

        for (int i = 0; i < _trsf.childCount; i++)
        {
            if (_trsf.GetChild(i).name == "mesh_GRP")
            {
                comp = GetSkinnedMeshRenderer(_trsf.GetChild(i));
                if (comp != null) return comp;
            }
        }

        for (int i = 0; i < _trsf.childCount; i++)
        {
            comp = GetSkinnedMeshRenderer(_trsf.GetChild(i));
            if (comp != null) return comp;
        }

        return null;
    }

    private void Awake()
    {
        childMR = transform.GetChild(0).GetComponent<MeshRenderer>();
        matHighlight = new Material(matHighlight);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            mat = meshRenderer.material;
        }
        if (!transform.parent) { Destroy(gameObject); return; }
        skinMeshR = GetSkinnedMeshRenderer(transform.parent);
        if (skinMeshR)
        {
            Material[] materials = skinMeshR.materials;
            Material[] newMaterials = new Material[materials.Length + 1];
            for (int i = 0; i < materials.Length; i++)
            {
                newMaterials[i] = materials[i];
            }
            newMaterials[materials.Length] = matHighlight;
            skinMeshR.materials = newMaterials;

            //if (skinMeshR.sharedMesh.subMeshCount == materials.Length && skinMeshR.sharedMesh.isReadable)
            //{
            //    skinMeshR.sharedMesh.subMeshCount = skinMeshR.sharedMesh.subMeshCount + 1;
            //    skinMeshR.sharedMesh.SetTriangles(skinMeshR.sharedMesh.triangles, skinMeshR.sharedMesh.subMeshCount - 1);
            //}
        }
        else
        {
            meshR = transform.parent.GetComponent<MeshRenderer>();
            if (meshR)
            {
                Material[] materials = meshR.materials;
                Material[] newMaterials = new Material[materials.Length + 1];
                for (int i = 0; i < materials.Length; i++)
                {
                    newMaterials[i] = materials[i];
                }
                newMaterials[materials.Length] = matHighlight;
                meshR.materials = newMaterials;
                //MeshFilter meshFilter;
                //if (meshR.TryGetComponent<MeshFilter>(out meshFilter) && meshFilter.mesh.isReadable && meshFilter.mesh.subMeshCount == materials.Length)
                //{
                //    meshFilter.mesh.subMeshCount = meshFilter.mesh.subMeshCount + 1;
                //    meshFilter.mesh.SetTriangles(meshFilter.mesh.triangles, meshFilter.mesh.subMeshCount - 1);
                //}
            }
        }
        entity = transform.parent.GetComponent<Entity>();
        character = transform.parent.GetComponent<Character>();
        container = transform.parent.GetComponent<Container>();

        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (character && !character.IsDead)
        {
            state = (FightMarkerState)((int)character.Hostility * 2 + (selected ? 1 : 0));
            mat.SetColor("_Color", colors[(int)state]);
            childMR.enabled = true;
            childMR.material.SetColor("_Color", colors[(int)state]);
            matHighlight.SetColor("_Color", colors[(int)state]);
            matHighlight.SetFloat("_Intensity", selected ? -0.5f : 0);
        }
        else if (entity)
        {
            state = FightMarkerState.NEUTRAL_SELECTED;
            childMR.enabled = false;
            mat.SetColor("_Color", new Color(0, 0, 0, 0));
            matHighlight.SetFloat("_Intensity", selected ? -0.5f : 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).Rotate(0, Time.deltaTime * 10.0f, 0);
        if (!justSelected) selected = false;
        UpdateVisual();
        justSelected = false;
        if (((!character && !entity) || entity.IsDead ||
            (!selected && ((character && character.State == CHARACTER_STATE.FREE) || (!character && entity))))
            && !container)
        {
            Destroy(gameObject);
        }
    }

    bool HaveSameNames(Material _mat1, Material _mat2)
    {
        string name1 = _mat1.name.Contains(" (Instance)") ? _mat1.name.Substring(0, _mat1.name.Length - (" (Instance)").Length) : _mat1.name;
        string name2 = _mat2.name.Contains(" (Instance)") ? _mat2.name.Substring(0, _mat2.name.Length - (" (Instance)").Length) : _mat2.name;

        if (name1 == name2)
        {
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        if (skinMeshR)
        {
            Material[] materials = skinMeshR.materials;
            Material[] newMaterials = new Material[materials.Length - 1];
            int cursor = 0;
            for (int i = 0; i < materials.Length; i++)
            {
                if (!HaveSameNames(materials[i], matHighlight))
                {
                    newMaterials[cursor] = materials[i];
                    cursor++;
                }
            }
            skinMeshR.materials = newMaterials;
        }
        else if (meshR)
        {
            Material[] materials = meshR.materials;
            Material[] newMaterials = new Material[materials.Length - 1];
            int cursor = 0;
            for (int i = 0; i < materials.Length; i++)
            {
                if (!HaveSameNames(materials[i], matHighlight))
                {
                    newMaterials[cursor] = materials[i];
                    cursor++;
                }
            }
            meshR.materials = newMaterials;
        }
    }
}
