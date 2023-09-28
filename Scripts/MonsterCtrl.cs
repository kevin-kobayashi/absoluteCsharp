using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    private JellyCtrl PlayerCtrl = default;
    public enum MonsterState { idle, trace, attack, die };
    public MonsterState monsterState = MonsterState.idle;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator _animator;

    private bool isDie = false;
  
    public GameObject bloodEffect;
    public GameObject bloodDecal;

    private int hp = 150;

    [SerializeField] private AudioClip shoutSfx;
    AudioSource audioSouse;
    public float shoutSfxVolumn = 1.0f;

    void Start()
    {
        PlayerCtrl = StageController.Instance.playerObj;
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        nvAgent.isStopped = false;
        _animator = this.gameObject.GetComponentInChildren<Animator>();
        audioSouse = gameObject.AddComponent<AudioSource>();

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }

  
    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(monsterState)
            {
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    _animator.SetBool("IsTrace", false);
                    break;

                case MonsterState.trace:
                    nvAgent.isStopped = false;
                    _animator.SetBool("IsAttack", false);
                    nvAgent.destination = playerTr.position;
                    _animator.SetBool("IsTrace", true);
                    _animator.SetTrigger("Shout");
                    PanPlay();
                    break;

                case MonsterState.attack:
                    nvAgent.isStopped = true;
                    _animator.SetBool("IsAttack", true);
                    break;

            }
            yield return null;
        }
    }

    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.gothit"))
        {
            _animator.SetBool("IsHit", false);
        }
       
        if(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.fall"))
        {
            _animator.SetBool("IsPlayerDie", false);
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.die"))
        {
            _animator.SetBool("IsDie", false);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "BULLET")
        {
            StartCoroutine(this.CreateBloodEffect(coll.transform.position));
            Destroy(coll.gameObject);
            _animator.SetBool("IsHit", true);

            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            if(hp <= 0)
            {
                MonsterDie();
            }
        }
    }

    void MonsterDie()
    {
        gameObject.tag = "Untagged";

        StopAllCoroutines();

        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        _animator.SetBool("IsDie", true);

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach(Collider coll in gameObject.GetComponentsInChildren<CapsuleCollider>())
        {
            coll.enabled = false;
        }
    }

    IEnumerator CreateBloodEffect(Vector3 pos)
    {
        GameObject _blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(_blood1, 2.0f);
        
        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.1f);
        Quaternion decalRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

        GameObject _blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);
        float _scale = Random.Range(1.5f, 3.5f);
        _blood2.transform.localScale = new Vector3(_scale, 1, _scale);

        Destroy(_blood2, 5.0f);
        yield return null;
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgent.isStopped = true;
        _animator.SetBool("IsPlayerDie", true);
    }

    void PanPlay()
    {
        if(transform.position.x > PlayerCtrl.transform.position.x)
        {
            audioSouse.panStereo = -0.5f;
        }
        else if(transform.position.x > PlayerCtrl.transform.position.x)
        {
            audioSouse.panStereo = -0.5f;
        }
        else
        {
            audioSouse.panStereo = 0;
        }

        audioSouse.PlayOneShot(shoutSfx, 0.02f);
    }


}
