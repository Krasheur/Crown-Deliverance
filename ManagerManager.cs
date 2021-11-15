using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerManager : MonoBehaviour
{
    public static ManagerManager main;

    [SerializeField] GameObject managerBT;
    [SerializeField] GameObject managerFight;
    [SerializeField] GameObject managerPlayer;
    [SerializeField] GameObject managerDialogue;
    [SerializeField] GameObject managerInput;
    [SerializeField] GameObject wiseObject;
    [SerializeField] GameObject hitFeedBack;
    [SerializeField] Canvas GUI_deMaupassant;
    [SerializeField] GameObject managerObj;
    Canvas currentGUI = null;

// Finaly only 2 Manager still usefull here 
    void Awake()
    {
        //Saver.LoadImportant();

        //if (main != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //main = this;
        Scene sce = SceneManager.GetSceneByName("Permanent");

        if (ObjManager.main == null)
        {
            GameObject obj = Instantiate(managerObj);
            SceneManager.MoveGameObjectToScene(obj, sce);
        }
        if (PlayerManager.main == null)
        {
            GameObject obj = Instantiate(managerPlayer);
            SceneManager.MoveGameObjectToScene(obj, sce);
        }

        //if (BTManager.main == null)
        //{
        //    Instantiate(managerBT, transform.parent);
        //}

        //if (FightManager.main == null)
        //{
        //    Instantiate(managerFight, transform.parent);
        //}

        //if (DialogueManager.Main == null)
        //{
        //    Instantiate(managerDialogue, transform.parent);
        //}

        //if (InputManager.main == null)
        //{
        //    Instantiate(managerInput, transform.parent);
        //}

        if (FeedBack.main == null)
        {
            GameObject obj = Instantiate(hitFeedBack);
            SceneManager.MoveGameObjectToScene(obj, sce);

        }


        //if (!currentGUI)
        //{
        //    currentGUI = Instantiate(GUI_deMaupassant, transform.parent);
        //}

        //Instantiate(wiseObject, transform.parent);
    }

}
