using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentHUD : UI
{
    private Character localPlayer;
    private Character player2;
    private Character player3;

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
