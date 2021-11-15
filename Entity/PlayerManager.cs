using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public enum CHARACTER
    {
        FAIRY,
        ROGUE,
        TANK,
        FOCUSED
    };

    static public PlayerManager main;
    [SerializeField] Character originelPlayerFairy;
    [SerializeField] Character originelPlayerTank;
    [SerializeField] Character originelPlayerRogue;

    [SerializeField] private Character playerFairy;
    [SerializeField] private Character playerTank;
    [SerializeField] private Character playerRogue;

    public Character Fairy { get => playerFairy; set => playerFairy = value; }
    public Character Tank { get => playerTank; set => playerTank = value; }
    public Character Rogue { get => playerRogue; set => playerRogue = value; }


    [SerializeField] Character focused;

    public Character FocusedCharacter
    {
        get => focused;

        set
        {
            if (value == playerFairy || value == playerTank || value == playerRogue) focused = value;
        }
    }

    public Character PlayerFairy { get => playerFairy; set => playerFairy = value; }
    public Character PlayerTank { get => playerTank; set => playerTank = value; }
    public Character PlayerRogue { get => playerRogue; set => playerRogue = value; }

    // Start is called before the first frame update
    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;

        if (playerFairy == null || playerTank == null || playerRogue == null)
        {
            Scene sce = SceneManager.GetSceneByName("Permanent");
            playerFairy = Instantiate(originelPlayerFairy);
            SceneManager.MoveGameObjectToScene(playerFairy.gameObject, sce);
            PlayerFairy.gameObject.SetActive(false);
            playerFairy.name = "Hymelia";
            PlayerRogue = Instantiate(originelPlayerRogue);
            SceneManager.MoveGameObjectToScene(playerRogue.gameObject, sce);
            PlayerRogue.gameObject.SetActive(false);
            PlayerRogue.name = "Akayel";
            playerTank = Instantiate(originelPlayerTank);
            SceneManager.MoveGameObjectToScene(playerTank.gameObject, sce);
            PlayerTank.gameObject.SetActive(false);
            PlayerTank.name = "Ryveck";

            if (!Saver.LoadCharacters())
            {
                Vector3 posSpawn = GameObject.Find("ZoneCheckPointSpawn").transform.position;
                Quaternion rotate = GameObject.Find("ZoneCheckPointSpawn").transform.rotation;

                playerFairy.transform.position = posSpawn;
                playerFairy.transform.rotation = rotate;
                playerRogue.transform.position = playerFairy.transform.position + playerFairy.transform.right * 2f;
                playerTank.transform.position = playerFairy.transform.position + -playerFairy.transform.right * 2f;
                playerTank.transform.rotation = rotate;
                playerRogue.transform.rotation = rotate;

                focused = playerFairy;
            }
        }
        
        //DontDestroyOnLoad(playerFairy.gameObject);
        //DontDestroyOnLoad(playerTank.gameObject);
        //DontDestroyOnLoad(playerRogue.gameObject);
        //DontDestroyOnLoad(this.gameObject);
           
        PlayerFairy.gameObject.SetActive(true);
        PlayerRogue.gameObject.SetActive(true);
        PlayerTank.gameObject.SetActive(true);
    }
    public void Start()
    {
        // never call
        CameraBehaviour.instance.gameObject.SetActive(false);
        InputManager.main.gameObject.SetActive(false);
        InputManager.main.transform.position = PlayerManager.main.PlayerFairy.transform.position + -PlayerManager.main.PlayerFairy.transform.forward * 2f;
        InputManager.main.transform.rotation = playerFairy.transform.rotation;
        CameraBehaviour.instance.transform.position = PlayerManager.main.PlayerFairy.transform.position + -PlayerManager.main.PlayerFairy.transform.forward * 2f;
        CameraBehaviour.instance.thirdPersonCamera.m_XAxis.Value = -playerFairy.transform.rotation.eulerAngles.y;
        
        CameraBehaviour.instance.gameObject.SetActive(true);
        InputManager.main.gameObject.SetActive(true);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerFairy.gameObject.SetActive(!playerFairy.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerRogue.gameObject.SetActive(!playerRogue.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerTank.gameObject.SetActive(!playerTank.gameObject.activeSelf);
        }
    }

    public void XpForAll(int _nbXp)
    {
        PlayerFairy.GainExp(_nbXp);
        PlayerTank.GainExp(_nbXp);
        PlayerRogue.GainExp(_nbXp);
    }

    public bool ItsPlayer(string _name)
    {
        return (PlayerFairy.name == _name) || (PlayerTank.name == _name) || (PlayerRogue.name == _name);
    }

    public bool ItsPlayer(Character _character)
    {
        return (PlayerFairy == _character) || (PlayerTank == _character) || (PlayerRogue == _character);
    }
}
