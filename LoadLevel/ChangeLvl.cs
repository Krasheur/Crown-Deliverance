using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ChangeLvl : MonoBehaviour
{


    [SerializeField] string zoneToLoad;
    [SerializeField] string sceneHere;
    [SerializeField] bool colliderOrnot = false;
     bool beginiColliderOrNot = false;
    [SerializeField] bool tpInLvl = false;
    [SerializeField] int id;
    [SerializeField] int idTp;
    [SerializeField] string nameInToolTip;
    bool corroutIsDone = false;
    GameObject blackScreen;
    InformationTP inftp;

    float timer = 0;

    public string NameInToolTip { get => nameInToolTip; }

    private void Awake()
    {
        inftp.id = id;
        inftp.idTogo = idTp;
        inftp.nameSceneHere = sceneHere;
        inftp.nameSceneTogo = zoneToLoad;
        inftp.gameObjOfthisID = this.gameObject;
        ManagerCheckPoint.CheckPointList.Add(inftp);

        beginiColliderOrNot = colliderOrnot;
    }

    public void LoadNextlvl()
    {
        if (!corroutIsDone 
            && !tpInLvl
            && PlayerManager.main.FocusedCharacter.CurrentFight == null
            && idTp != 0)
        {
            corroutIsDone = true;
            StartCoroutine(Transition.main.LoadNewLevel(zoneToLoad, inftp));
        }
        else if (!corroutIsDone 
                && tpInLvl 
                && PlayerManager.main.FocusedCharacter.CurrentFight == null)
        {
            corroutIsDone = true;
            StartCoroutine(Transition.main.Teleport(inftp));

        }

    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 30)
        {
            corroutIsDone = false;
            colliderOrnot = beginiColliderOrNot;
            timer = 0;
        }
    }


    public void LoadPreviouslvl()
    {
        //StartCoroutine(Transition.main.LoadNewLevel(previousZone, inftp));
    }
    private void OnTriggerEnter(Collider other)
    {            
        if (PlayerManager.main.ItsPlayer(other.name) && colliderOrnot )
        {
            LoadNextlvl();
            colliderOrnot = false;
        }
    }


}
