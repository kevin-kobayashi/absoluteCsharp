using System.Collections;
using UnityEngine;
using System;

public class StageSequencer: MonoBehaviour
{
    public Transform[] points;
    public GameObject monsterPrefab;
    [SerializeField] private StageController StageCtrl = default;
    public float createTime = 2.0f;
    public int maxMonster = 10;
    public bool isGameOver = false;

    void Start()
    {
        StageCtrl = GetComponent<StageController>();
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
      
        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    
    }

    IEnumerator CreateMonster()
    {
        while(!isGameOver)
        {
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;

            if (monsterCount < maxMonster)
            {
                yield return new WaitForSeconds(createTime);

                int idx = UnityEngine.Random.Range(1, points.Length);

                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
