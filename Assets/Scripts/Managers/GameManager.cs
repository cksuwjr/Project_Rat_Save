using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //private DataManager dataManager;
    private UIManager uiManager;
    private SoundManager soundManager;
    private PoolManager poolManager;
    //private TitleSceneManager titleSceneManager;
    //private ScenarioManager scenarioManager;


    [SerializeField] private GameObject player;

    public GameObject Player
    {
        get
        {
            if (player == null)
                player = GameObject.Find("Player");
            return player;
        }
    }

    protected override void DoAwake()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            AssignManagers();
            InitManagers();
        };
    }

    private void AssignManagers()
    {
        //GameObject.Find("DataManager")?.TryGetComponent<DataManager>(out dataManager);
        GameObject.Find("SoundManager")?.TryGetComponent<SoundManager>(out soundManager);

        if (GameObject.Find("UIManager"))
            GameObject.Find("UIManager")?.TryGetComponent<UIManager>(out uiManager);
        else
            uiManager = null;

        if (GameObject.Find("PoolManager"))
            GameObject.Find("PoolManager")?.TryGetComponent<PoolManager>(out poolManager);
        else
            poolManager = null;

        //if (GameObject.Find("TitleSceneManager"))
        //    GameObject.Find("TitleSceneManager")?.TryGetComponent<TitleSceneManager>(out titleSceneManager);
        //else
        //    titleSceneManager = null;

        //if (GameObject.Find("ScenarioManager"))
        //    GameObject.Find("ScenarioManager")?.TryGetComponent<ScenarioManager>(out scenarioManager);
        //else
        //    scenarioManager = null;
    }

    private void InitManagers()
    {
        //dataManager?.Init();
        uiManager?.Init();
        poolManager?.Init();
        soundManager?.Init();

        //titleSceneManager?.Init();
        //scenarioManager?.Init();
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        StartCoroutine("Spawn");
        



        //var cat1 = GameManager.Instance.poolManager.yellowCatPool.GetPoolObject();
        //cat1.transform.position = new Vector3(2, 0.5f, 7.68f);
        //cat1.GetComponent<Entity>().Init();


        //var cat2 = GameManager.Instance.poolManager.blackCatPool.GetPoolObject();
        //cat2.transform.position = new Vector3(-0.15f, 0.5f, 7.68f);
        //cat2.GetComponent<Entity>().Init();
    }

    IEnumerator Spawn()
    {
        int count;
        int maxCount;

        count = 0;
        maxCount = 5;
        while (count < maxCount)
        {
            GameObject spawned;

            spawned = EnemySpawner.GetSpawnerById(0).Spawn(3, WeaponType.Hand);
            spawned.GetComponent<Entity>().Init();

            spawned = EnemySpawner.GetSpawnerById(1).Spawn(3, WeaponType.Hand);
            spawned.GetComponent<Entity>().Init();

            spawned = EnemySpawner.GetSpawnerById(2).Spawn(3, WeaponType.Hand);
            spawned.GetComponent<Entity>().Init();

            yield return YieldInstructionCache.WaitForSeconds(3f);
            count++;
        }
        yield return YieldInstructionCache.WaitForSeconds(10);
        Debug.Log("다음 스테이지");
        count = 0;
        maxCount = 5;
        while (count < maxCount)
        {
            var spawned = EnemySpawner.GetSpawnerById(0).Spawn(4, WeaponType.Wood_Carving);
            spawned.GetComponent<Entity>().Init();

            spawned = EnemySpawner.GetSpawnerById(1).Spawn(4, WeaponType.Wood_Carving);
            spawned.GetComponent<Entity>().Init();

            spawned = EnemySpawner.GetSpawnerById(2).Spawn(4, WeaponType.Wood_Carving);
            spawned.GetComponent<Entity>().Init();

            yield return YieldInstructionCache.WaitForSeconds(3f);
            count++;
        }
        yield return YieldInstructionCache.WaitForSeconds(10);


        yield return null;
    }
}