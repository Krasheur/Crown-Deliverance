using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Controller : MonoBehaviour
{
    [SerializeField] protected Character character = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setCharacter(Character _character)
    {
        character = _character;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        switch (character.State)
        {
            case CHARACTER_STATE.FREE:
                HandleFreeInput();
                break;

            case CHARACTER_STATE.FIGHT:
                HandleFightInput();
                break;

            case CHARACTER_STATE.LOCKED:
                HandleLockInput();
                break;
        }
    }

    abstract protected void HandleFreeInput();
    abstract protected void HandleFightInput();
    abstract protected void HandleLockInput();
}
