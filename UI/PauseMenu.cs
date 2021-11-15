using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    CanvasGroup pauseMenu = null;
    [SerializeField] Button button = null;
    [SerializeField] Button button2 = null;

    bool isSaveReady = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.interactable)
            {
                pauseMenu.alpha = 1f;
                pauseMenu.interactable = true;
                pauseMenu.blocksRaycasts = true;
            }
            else
            {
                pauseMenu.alpha = 0f;
                pauseMenu.interactable = false;
                pauseMenu.blocksRaycasts = false;
            }
        }

        if (PlayerManager.main.FocusedCharacter.CurrentFight && button.interactable)
        {
            button.interactable = false;
            button2.interactable = false;
        }
        else if (!PlayerManager.main.FocusedCharacter.CurrentFight && !button.interactable)
        {
            button.interactable = true;
            button2.interactable = true;
        }
    }

    public void Save()
    {
        if(PlayerManager.main.PlayerFairy.CurrentFight == null && PlayerManager.main.PlayerTank.CurrentFight == null && PlayerManager.main.PlayerRogue.CurrentFight == null)
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        Saver.Save();
    }

    public void Quit()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        SceneManager.UnloadSceneAsync("Permanent");


        string sceneName = Saver.LoadScene();
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation aso_2 = null;

        if (scene.name != sceneName)
        {
            SceneManager.UnloadSceneAsync(scene);
            aso_2 = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        AsyncOperation aso = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

        while (!aso.isDone && (aso_2 == null || !aso_2.isDone))
        {
            Debug.Log("truc");
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }
}
