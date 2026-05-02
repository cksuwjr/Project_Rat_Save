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
        GameManager.Instance.player.GetComponent<Entity>().Init(10000, 5);

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
        List<Entity> spawnedEntities = new List<Entity>();


        List<StageData> stageData = StageDataManager.Instance.stageDatas;

        GameObject spawned;

        for (int i = 0; i < stageData.Count; i++)
        {
            Debug.Log("Stage - " + (i + 1));

            for (int j = 0; j < stageData[i].spawnDatas.Count; j++)
            {
                var spawnData = stageData[i].spawnDatas[j];

                for (int k = 0; k < spawnData.spawnCount; k++)
                {
                    spawned = EnemySpawner.GetSpawnerById(spawnData.spawnerID)?.Spawn(spawnData.spawnType, spawnData.weaponType);
                    spawned?.GetComponent<Entity>().Init(spawnData.hp, spawnData.speed);
                    spawnedEntities.Add(spawned?.GetComponent<Entity>());

                    YieldInstructionCache.WaitForSeconds(spawnData.spawnTerm);
                }
            }

            bool isAllDead = false;
            while (!isAllDead)
            {
                isAllDead = true;

                for (int j = 0; j < spawnedEntities.Count; j++)
                    if (!spawnedEntities[j].isDead)
                    {
                        isAllDead = false;
                        break;
                    }
                yield return null;
            }

            Debug.Log("Stage - " + (i + 1) + "»óÁ¡ ¿ÀÇÂ");
        }

       



        yield return null;
    }
}