using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance = null;
    static public UIManager Instance { get => instance; }

    private Inventory inventory;
    private PermanentHUD permanentHUD;
    private FightHUD fightHUD;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    void EnableCanvasGroup(CanvasGroup _canvasGroup)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.enabled = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    void DisableCanvasGroup(CanvasGroup _canvasGroup)
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.enabled = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}
