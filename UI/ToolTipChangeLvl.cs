using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipChangeLvl : MonoBehaviour
{
    ToolTipPopUp toolTipPopUp;
    ChangeLvl changeLvl;
    void Start()
    {
        toolTipPopUp = GameObject.Find("TooltipPopUp").GetComponent<ToolTipPopUp>();
        changeLvl = GetComponent<ChangeLvl>();
    }

    private void OnMouseOver()
    {
        toolTipPopUp.DisplayInfoChangeLvl(changeLvl);
    }

    private void OnMouseExit()
    {
        toolTipPopUp.HideInfo();
    }
}
