using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{

    //[SerializeField] public ObjectPoo playerBulletPool = default;
    [SerializeField] public JellyCtrl playerObj = default;
    //[SerializeField] public FireCtrl fireObj = default;
    public bool IsGoal = false;
    private static StageController instance;
    public static StageController Instance { get => instance; }    

    //[SerializeField] public StageSequencer mons;

    public bool isPlaying;

    public enum PlayStopCodeDef
    {
        PlayerDead,
        StageClear,
    }
    public PlayStopCodeDef playStopCode;
    private void Awake()
    {
        instance = this.GetComponent<StageController>();
        
    }
    void Start()
    {
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObj.isPlayerDie)
        {
            playStopCode = PlayStopCodeDef.PlayerDead;
            isPlaying = false;
        }
        if(IsGoal)
        {
            playStopCode = PlayStopCodeDef.StageClear;
            isPlaying = false;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            playerObj._animator.SetTrigger("Shoot");
        }
        
    }
    /*
    void playerShot()
    {
        playerObj.Shot();
    }
    */
    public void StageStart()
    {
        isPlaying = true;
    }
    /*
    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;

        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        AudioSource _audioSource = soundObj.GetComponent<AudioSource>();
        _audioSource.clip = sfx;
        _audioSource.minDistance = 10.0f;
        _audioSource.maxDistance = 30.0f;
        _audioSource.volume = sfxVolumn;
        _audioSource.Play();

        Destroy(soundObj, sfx.length);
    }
    */
}
