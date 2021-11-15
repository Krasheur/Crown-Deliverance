using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] MusicsManager.MusicKind musicName;
    
    private bool isPlaying = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isPlaying)
        {
            if (SceneManager.GetActiveScene().buildIndex > 2 && other.gameObject.GetComponent<Character>() == PlayerManager.main.FocusedCharacter)
            {
                if (MusicsManager.instance.musicState.Count != 0)
                {
                    if (other.gameObject.GetComponent<Character>().State != CHARACTER_STATE.FIGHT && MusicsManager.instance.musicState[MusicsManager.instance.musicState.Count - 1] != musicName)
                    {
                        //Start Music
                        MusicsManager.instance.musicState.Add(musicName);
                        isPlaying = true;
                        //
                    }
                    else
                    {
                        MusicsManager.instance.nextMusicState = musicName;
                        isPlaying = true;
                    }
                }
                else
                {

                    if (other.gameObject.GetComponent<Character>().State != CHARACTER_STATE.FIGHT)
                    {
                        //Start Music
                        MusicsManager.instance.musicState.Add(musicName);
                        isPlaying = true;
                        //
                    }
                    else
                    {
                        MusicsManager.instance.nextMusicState = musicName;
                        isPlaying = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex > 2 && other.gameObject.GetComponent<Character>() == PlayerManager.main.FocusedCharacter)
        {
            //End Music
            MusicsManager.instance.musicState.Remove(musicName);
            isPlaying = false;
            //

            if(MusicsManager.instance.nextMusicState == musicName)
            {
                MusicsManager.instance.nextMusicState = MusicsManager.MusicKind.MusicKindCount;
            }
        }
    }

    private void OnDestroy()
    {
        //End Music
        MusicsManager.instance.musicState.Remove(musicName);
        //

        if (MusicsManager.instance.nextMusicState == musicName)
        {
            MusicsManager.instance.nextMusicState = MusicsManager.MusicKind.MusicKindCount;
        }
    }
}
