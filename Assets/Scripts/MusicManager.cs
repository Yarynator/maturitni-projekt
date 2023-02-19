using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public struct MusicSaveData
    {
        public int type;
        public int index;
        public float time;
        public int fromSceneIndex;

        public MusicSaveData(int type, int index, float time, int fromSceneIndex)
        {
            /*  0 - menu
                1 - outside
                2 - town
                3 - battle
            */
            this.type = type;

            //index in list
            this.index = index;

            //time in seconds
            this.time = time;
            this.fromSceneIndex = fromSceneIndex;
        }
    }

    public static MusicManager Instance { get; private set; }


    [SerializeField] private AudioSource musicSource;
    [SerializeField] private MusicListSO musicList;

    private float volume;

    private MusicSaveData musicSaveData;
    private MusicSO prevMusic;
    private int musicIndex;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("MusicManager already exists!");
            return;
        }
        Instance = this;

        if (PlayerPrefs.HasKey("Music"))
        {
            volume = PlayerPrefs.GetFloat("Music");
        } else
        {
            volume = .5f;
            PlayerPrefs.SetFloat("Music", .5f);
        }

        musicSource.volume = volume;
        prevMusic = null;
    }

    private void Start()
    {
        musicSaveData = SceneInfo.Instance.GetMusicSaveData();
        LoadMusic();
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            NextMusic();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextMusic();
        }
    }

    private void NextMusic()
    {
        musicIndex = Random.Range(0, musicList.list.Count);
        while(musicList.list[musicIndex].infoIndex == prevMusic?.infoIndex)
        {
            musicIndex = Random.Range(0, musicList.list.Count);
        }

        musicSource.clip = musicList.list[musicIndex].music;
        prevMusic = musicList.list[musicSaveData.index];
        musicSource.time = 0;
        musicSource.Play();
    }

    private void LoadMusic()
    {
        if(musicList.musicType == musicSaveData.type)
        {
            musicSource.clip = musicList.list[musicSaveData.index].music;
            prevMusic = musicList.list[musicSaveData.index];
            if (SceneInfo.Instance.ComeFromSceneIndex() != -1)
            {
                musicSource.time = musicSaveData.time;
            }
            musicSource.Play();
        } 
        else 
        {
            NextMusic();
        }
    }

    public MusicSaveData GetMusicSaveData()
    {
        return new MusicSaveData(musicList.musicType, musicIndex, musicSource.time, SceneInfo.Instance.GetSceneIndex());
    }

    public void SetMusicVolume(float volume)
    {
        this.volume = volume;
        musicSource.volume = volume;
    }

}
