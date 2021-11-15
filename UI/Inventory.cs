using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    protected Character localPlayer;
    protected Character player2;
    protected Character player3;

    void setLocalPlayer(Character character)
    {
        localPlayer = character;
    }

    void setPlayer2(Character character)
    {
        player2 = character;
    }

    void setPlayer3(Character character)
    {
        player3 = character;
    }
}
