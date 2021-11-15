using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicsManager : MonoBehaviour
{
    public enum MusicKind
    {
        Menu,
        Castle,
        Dungeon,
        Elf_Tribe,
        Fairy_Tribe,
        Going_In_Town,
        Going_To_War,
        Lezard_Tribe,
        Peacefull_Exploration,
        MusicKindCount
    }

    public enum AmbKind
    {
        Dungeon,
        Catacomb,
        Forest,
        AmbKindCount
    }

    public List<MusicKind> musicState;
    public MusicKind nextMusicState = MusicKind.MusicKindCount;

    public List<AmbKind> ambState;

    public static MusicsManager instance = null;

    private MusicKind lastMusicState = MusicKind.MusicKindCount;
    private AmbKind lastAmbState = AmbKind.AmbKindCount;

    public static float volumeMain = 50f;
    public static float volumeMusic = 50f;
    public static float volumeFX = 50f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(transform.gameObject);
            return;
        }

        SetVolumeMain(50f);SetVolumeMusics(50f);SetVolumeFX(50f);
        AkSoundEngine.SetRTPCValue("LowPassFX", 0f);
        LoadBank("Menu");
        LoadBank("InGame");

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (nextMusicState != MusicKind.MusicKindCount && musicState[musicState.Count - 1] != MusicKind.Going_To_War)
        {
            if (nextMusicState != musicState[musicState.Count - 1]) musicState.Add(nextMusicState);
            nextMusicState = MusicKind.MusicKindCount;
        }

        if (nextMusicState == MusicKind.MusicKindCount && musicState.Count != 0 && lastMusicState != musicState[musicState.Count - 1])
        {
            switch (musicState[musicState.Count - 1])
            {
                case MusicKind.Menu:
                    PlayMusic("MenuMusics_Play");
                    break;

                case MusicKind.Castle:
                    PlayMusic("Castle_Play");
                    break;

                case MusicKind.Dungeon:
                    PlayMusic("Dungeon_Play");
                    break;

                case MusicKind.Elf_Tribe:
                    PlayMusic("Elf_Tribe_Play");
                    break;

                case MusicKind.Fairy_Tribe:
                    PlayMusic("Fairy_Tribe_Play");
                    break;

                case MusicKind.Going_In_Town:
                    PlayMusic("Going_In_Town_Play");
                    break;

                case MusicKind.Going_To_War:
                    PlayMusic("Going_To_War_Play");
                    break;

                case MusicKind.Lezard_Tribe:
                    PlayMusic("Lezard_Tribe_Play");
                    break;

                case MusicKind.Peacefull_Exploration:
                    PlayMusic("Peacefull_Exploration_Play");
                    break;

                default:
                    break;
            }

            lastMusicState = musicState[musicState.Count - 1];
        }
        else if(musicState.Count == 0)
        {
            AkSoundEngine.PostEvent("Musics_Stop", transform.gameObject);
        }


        if (ambState.Count != 0 && lastAmbState != ambState[ambState.Count - 1])
        {
            switch (ambState[ambState.Count - 1])
            {
                case AmbKind.Dungeon:
                    PlayAmb("DungeonAmb_Play");
                    break;

                case AmbKind.Catacomb:
                    PlayAmb("CatacombAmb_Play");
                    break;

                case AmbKind.Forest:
                    PlayAmb("ForestAmb_Play");
                    break;

                default:
                    break;
            }

            lastAmbState = ambState[ambState.Count - 1];
        }
        else if(ambState.Count == 0)
        {
            AkSoundEngine.PostEvent("Ambs_Stop", transform.gameObject);
            lastAmbState = AmbKind.AmbKindCount;
        }
    }

    public void LoadBank(string _bankName)
    {
        uint tmp;
        AkSoundEngine.LoadBank(_bankName, out tmp);
    }
    public void PlayMusic(string _musicName)
    {
        AkSoundEngine.PostEvent("Musics_Stop", transform.gameObject); 
        AkSoundEngine.PostEvent(_musicName, transform.gameObject);
    }
    public void PlayAmb(string _ambName)
    {
        AkSoundEngine.PostEvent("Ambs_Stop", transform.gameObject);
        AkSoundEngine.PostEvent(_ambName, transform.gameObject);
    }
    public void SetVolumeMain(float _volume)
    {
        float tmp = _volume > 100f ? 100f : (_volume < 0f ? 0f : _volume);
        volumeMain = tmp;
        AkSoundEngine.SetRTPCValue("VolumeMain", tmp);
    }
    public void SetVolumeMusics(float _volume)
    {
        float tmp = _volume > 100f ? 100f : (_volume < 0f ? 0f : _volume);
        volumeMusic = tmp;
        AkSoundEngine.SetRTPCValue("VolumeMusics", tmp);
    }
    public void SetVolumeFX(float _volume)
    {
        float tmp = _volume > 100f ? 100f : (_volume < 0f ? 0f : _volume);
        volumeFX = tmp;
        AkSoundEngine.SetRTPCValue("VolumeFX", tmp);
    }
}
