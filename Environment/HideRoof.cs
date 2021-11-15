using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoof : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.main.FocusedCharacter /*&& 
            PlayerManager.main.FocusedCharacter.CurrentFight*/)
        {
            RaycastHit hit;
            Vector3 startPos = PlayerManager.main.FocusedCharacter.transform.position;
            Vector3 direction = CameraBehaviour.instance.transform.position - startPos;
            if (Physics.Raycast(startPos, direction.normalized, out hit, Vector3.Distance(CameraBehaviour.instance.transform.position, startPos), 1 << LayerMask.NameToLayer("HideInTopView"), QueryTriggerInteraction.Collide))
            {
                RoofToHide.Hide = true;
                return;
            }
        }

        RoofToHide.Hide = false;
    }
}
