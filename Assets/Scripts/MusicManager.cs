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
    private bool isBattleMusic;
    private bool isBattleMusicChanging;

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
        isBattleMusic = false;
        isBattleMusicChanging = false;
    }

    private void Start()
    {
        musicSaveData = SceneInfo.Instance.GetMusicSaveData();
        LoadMusic();

        if (SceneInfo.Instance.GetSceneIndex() != 0)
        {
            BattleManager.Instance.OnIsBattleChange += BattleManager_OnIsBattleChange;
        }
    }

    private void BattleManager_OnIsBattleChange(object sender, System.EventArgs e)
    {
        bool isBattle = BattleManager.Instance.IsBattle();
        if(isBattle)
        {
            List<MusicSO> battleMusicList = Resources.Load<MusicListSO>("BattleMusicList").list;
            int random = Random.Range(0, battleMusicList.Count);
            musicIndex = random;

            musicSource.clip = battleMusicList[random].music;
            musicSource.time = 0;
            musicSource.Play();
            isBattleMusic = true;
        }
        else
        {
            isBattleMusicChanging = true;
        }
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

        if(isBattleMusicChanging)
        {
            musicSource.volume = Mathf.Clamp(musicSource.volume - Time.deltaTime / 3, 0, 1);
            if(musicSource.volume == 0)
            {
                isBattleMusicChanging = false;
                isBattleMusic = false;
                musicSource.volume = volume;

                musicIndex = Random.Range(0, musicList.list.Count);
                musicSource.clip = musicList.list[musicIndex].music;
                prevMusic = musicList.list[musicSaveData.index];
            }
        }
    }

    private void NextMusic()
    {
        if (isBattleMusic)
        {
            List<MusicSO> battleMusicList = Resources.Load<MusicListSO>("BattleMusicList").list;
            musicIndex = Random.Range(0, battleMusicList.Count);
            while (battleMusicList[musicIndex].infoIndex == prevMusic?.infoIndex)
            {
                musicIndex = Random.Range(0, battleMusicList.Count);
            }

            musicSource.clip = battleMusicList[musicIndex].music;
            prevMusic = battleMusicList[musicSaveData.index];
        }
        else
        {
            musicIndex = Random.Range(0, musicList.list.Count);
            while (musicList.list[musicIndex].infoIndex == prevMusic?.infoIndex)
            {
                musicIndex = Random.Range(0, musicList.list.Count);
            }

            musicSource.clip = musicList.list[musicIndex].music;
            prevMusic = musicList.list[musicSaveData.index];
        }

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
