using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<Animator>().SetBool("isGameOver", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.main.PlayerFairy.IsDead && PlayerManager.main.PlayerRogue.IsDead && PlayerManager.main.PlayerTank.IsDead)
        {
            GetComponent<Animator>().SetBool("isGameOver", true);
        } 
        else if(GetComponent<CanvasGroup>().alpha == 0)
        {
            GetComponent<Animator>().SetBool("isGameOver", false);
        }
    }

    public void Reload()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        StartCoroutine(ReloadGame());

        Saver.LoadImportant();
        Saver.LoadCharacters();        
    }

    public void ReturnToMenu()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        SceneManager.UnloadSceneAsync("Permanent");
        AsyncOperation aso = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

        while (!aso.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }

    IEnumerator ReloadGame()
    {
        AsyncOperation aso = SceneManager.LoadSceneAsync(Saver.LoadScene(), LoadSceneMode.Single);        
        AsyncOperation aso1 = SceneManager.LoadSceneAsync("Permanent", LoadSceneMode.Additive);

        while (!aso.isDone && !aso1.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Saver.LoadScene()));              
    }
}
