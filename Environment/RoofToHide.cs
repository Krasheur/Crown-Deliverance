using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofToHide : MonoBehaviour
{
    static public List<RoofToHide> roofs;
    static bool hide = false;

    public static bool Hide
    {
        get => hide;
        set
        {
            if (roofs != null && value != hide)
            {
                for (int i = 0; i < roofs.Count; i++)
                {
                    roofs[i].Hidden = value;
                }
            }
            hide = value;
        }
    }

    ParticleSystem[] particleSystems;
    bool hidden = false;
    public bool Hidden
    {
        get => hidden;
        set
        {
            hidden = value;
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = !hidden;
            }
            for (int j = 0; j < particleSystems.Length; j++)
            {
                particleSystems[j].gameObject.SetActive(!hidden);
                if (!hidden) particleSystems[j].Play();
            }
        }
    }

    void Start()
    {
        if (roofs == null)
        {
            roofs = new List<RoofToHide>();
        }
        roofs.Add(this);
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnDestroy()
    {
        if (roofs != null)
        {
            roofs.Remove(this);
        }
    }
}
