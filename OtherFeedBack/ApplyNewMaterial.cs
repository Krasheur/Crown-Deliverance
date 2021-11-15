using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyNewMaterial : MonoBehaviour
{
    [SerializeField] Material material;
    MeshRenderer meshR;
    SkinnedMeshRenderer skinMeshR;

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

    void Start()
    {
        skinMeshR = GetSkinnedMeshRenderer(transform.parent);
        if (skinMeshR)
        {
            Material[] materials = skinMeshR.materials;
            Material[] newMaterials = new Material[materials.Length + 1];
            for (int i = 0; i < materials.Length; i++)
            {
                newMaterials[i] = materials[i];
            }
            newMaterials[materials.Length] = new Material(material);
            skinMeshR.materials = newMaterials;
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
                newMaterials[materials.Length] = new Material(material);
                meshR.materials = newMaterials;
            }
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
                if (!HaveSameNames(materials[i], material))
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
                if (!HaveSameNames(materials[i], material))
                {
                    newMaterials[cursor] = materials[i];
                    cursor++;
                }
            }
            meshR.materials = newMaterials;
        }
    }
}
