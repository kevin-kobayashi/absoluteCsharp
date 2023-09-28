using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class JellyCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    //private ObjectPoo bulletPool;
    //private float shootInterval = 0;
    
    private Transform tr;
    public float moveSpeed = 3.0f;
    public float rotSpeed = 100.0f;

    public Animator _animator;

    public bool isPlayerDie = false;

    /*BULLETMove
    public float speed;
    [SerializeField]PoolConte poolcontent;
    */
    public const int maxhp = 100;
    public int currentHp = maxhp;
    
    public Text HP;
    /*
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    */

    private StageSequencer _gameMgr;
    private StageController StageCtrl;
    void Start()
    {
        tr = GetComponent<Transform>();
        _animator = GetComponentInChildren<Animator>();
        StageCtrl = GetComponent<StageController>();
        _gameMgr = GameObject.Find("StageSequencer").GetComponent<StageSequencer>();
        /*
        bulletPool = StageController.Instance.playerBulletPool;
        poolcontent = transform.GetComponent<PoolConte>();
        */
        
    }

    void Update()
    {
        //if (!StageCtrl.isPlaying) return;
        
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        /*
        Debug.Log("H = " + h.ToString());
        Debug.Log("V = " + v.ToString());
        */
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir * moveSpeed  * Time.deltaTime, Space.Self);

        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));  
        //shootInterval -= Time.deltaTime;

        if (v >= 0.01)
        {
            _animator.SetBool("RunF", true);
        }
        else 
        {
            _animator.SetBool("RunF", false);
        }

        if (v <= -0.01)
        {
            _animator.SetBool("RunB", true);
        }
        else
        {
            _animator.SetBool("RunB", false);
        }

        if (h >= 0.01)
        {
            _animator.SetBool("RunL", true);
        }
        else
        {
            _animator.SetBool("RunL", false);
        }

        if (h <= -0.01)
        {
            _animator.SetBool("RunR", true);
        }
        else
        {
            _animator.SetBool("RunR", false);
        }
    }

    /*
    public void Shot()
    {


        if (shootInterval <= 0)
        {
            var obj = bulletPool.Launch(transform.position + Vector3.up * 1f, 0);
            if (obj != null)
            {
                //shootInterval = 0.1f;
                //BULLETMove

            }
           
        }
    }
    */

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "PUNCH")
        {
            currentHp -= 10;
            Debug.Log("Player HP = " + currentHp.ToString());
            HP.text = currentHp.ToString();
    
            if (currentHp <= 0)
            {
                currentHp = 0;
                PlayerDie();
                //OnPlayerDie();
                _gameMgr.isGameOver = true;
            }
        }
    }


    void PlayerDie()
    {
        Debug.Log("Player Die !!");
        _animator.SetBool("IsDie", true);
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach(GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
        isPlayerDie = true;
    }

}
