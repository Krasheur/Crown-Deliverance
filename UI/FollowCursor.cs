using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCursor : MonoBehaviour
{
    static public FollowCursor main;
    [SerializeField] CanvasGroup cg;

    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
    }
    void Update()
    {
        Vector3 newPos = Input.mousePosition;
        newPos.z = 0f;

        transform.position = newPos;
    }
    
    public void ShowCursor()
    {
        cg.alpha = 1;
    }

    public void HideCursor()
    {
        cg.alpha = 0;
    }
}
