using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InformationTP
{
    public int id;
    public int idTogo;
    public string nameSceneTogo;
    public string nameSceneHere;
    public GameObject gameObjOfthisID;
}

public class ManagerCheckPoint : MonoBehaviour
{

    private static List<InformationTP> checkPointList;

    public static List<InformationTP> CheckPointList { get 
        {
            if (checkPointList == null)
            {
                checkPointList = new List<InformationTP>();
            }
            return checkPointList;
        }
        set
        {
            if (checkPointList == null)
            {
                checkPointList = new List<InformationTP>();
            }
            checkPointList = value;
        }
    }
}
