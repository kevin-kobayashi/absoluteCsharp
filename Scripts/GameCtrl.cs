using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCtrl : MonoBehaviour
{
    [SerializeField] private GameObject titleUISet = default;
    [SerializeField] private GameObject gameOverUISet = default;
    [SerializeField] private GameObject gameClearUISet = default;
    //[SerializeField] private GameObject gameMainUISet = default;
    [SerializeField] private  AudioClip BGM;
    AudioSource audioSource;
    public float bgmVolumn = 0.1f;
    public float sfxVolumn = 1.0f;
    //public bool isSfxMute = false;
    public enum GameState
    {
        TITLE,
        GAMEMAIN,
        CLEAR,
        GAMEOVER
    };
    private GameState gameState = GameState.TITLE;

    delegate void gameProc();
    Dictionary<GameState, gameProc> gameProcList;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.loop = true;
        gameProcList = new Dictionary<GameState, gameProc>
        {
            {GameState.TITLE, Title},
            {GameState.GAMEMAIN, GameMain},
            { GameState.CLEAR, Clear},
            { GameState.GAMEOVER, GameOver },
        };
        gameState = GameState.TITLE;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = bgmVolumn;
        gameProcList[gameState]();
    }

    private void Title()
    {
        if(Input.anyKeyDown)
        {
            gameState = GameState.GAMEMAIN;
            StageController.Instance.StageStart();
            titleUISet.SetActive(false);
            //gameMainUISet.SetActive(true);
            audioSource.Play();
        }
    }
    private void GameMain()
    {
        
        if(!StageController.Instance.isPlaying)
        {
            if(StageController.Instance.playStopCode ==
                StageController.PlayStopCodeDef.PlayerDead)
            {
                gameOverUISet.SetActive(true);
                audioSource.Stop();
                gameState = GameState.GAMEOVER;
            }
            else
            {
                gameClearUISet.SetActive(true);
                audioSource.Stop();
                gameState = GameState.CLEAR;
            }
        }
    }
    private void Clear()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void GameOver()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }
    }

    
    /*
    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;

        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        AudioSource _audioSource = soundObj.GetComponents<AudioSource>()[1];
        _audioSource.clip = sfx;
        _audioSource.minDistance = 10.0f;
        _audioSource.maxDistance = 30.0f;
        _audioSource.volume = sfxVolumn;
        _audioSource.Play();

        Destroy(soundObj, sfx.length);
    }
    */
}
