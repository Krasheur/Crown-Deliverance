using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Footsteps : MonoBehaviour
{
    public enum FootKind
    {
        Light,
        Medium,
        Heavy,
        FootKindCount
    }

    public enum GroundKind
    {
        Dirt = 8,
        Mud =  16,
        Grass = 32,
        Gravel = 64,
        Rock = 128,
        Wood = 256,
        Water = 512,
        GroundKindCount
    }

    private float timer = 0f;

    private NavMeshHit hit;

    public void PlayFootsteps()
    {
        bool tmp = NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas);

        if (tmp && timer == 0f && (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f))
        {
            
            switch ((GroundKind)hit.mask)
            {
                case GroundKind.Dirt:
                    AkSoundEngine.PostEvent("Dirt", transform.gameObject);
                    break;

                case GroundKind.Mud:
                    AkSoundEngine.PostEvent("Mud", transform.gameObject);
                    break;

                case GroundKind.Grass:
                    AkSoundEngine.PostEvent("Grass", transform.gameObject);
                    break;

                case GroundKind.Gravel:
                    AkSoundEngine.PostEvent("Gravel", transform.gameObject);
                    break;

                case GroundKind.Rock:
                    AkSoundEngine.PostEvent("Rock", transform.gameObject);
                    break;

                case GroundKind.Wood:
                    AkSoundEngine.PostEvent("Wood", transform.gameObject);
                    break;

                case GroundKind.Water:
                    AkSoundEngine.PostEvent("Water", transform.gameObject);
                    break;

                default:
                    break;
            }
        }
    }
}
