using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //private DataManager dataManager;
    private UIManager uiManager;
    private SoundManager soundManager;
    private PoolManager poolManager;

    private CameraManager cameraManager;
    //private TitleSceneManager titleSceneManager;
    //private ScenarioManager scenarioManager;

    public bool isNextStageReady;
    public GameObject clearNpc;
    public int chatCount = 0;
    const int TUTORIAL_COUNT = 3;

    [SerializeField] private GameObject player;


    private int money;


    public GameObject Player
    {
        get
        {
            if (player == null)
                player = GameObject.Find("Player");
            return player;
        }
    }

    public int Money { get { return money; } set { money = value; OnChangeMoney?.Invoke(money); } }
    public Action<int> OnChangeMoney;

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

        if (GameObject.Find("CameraManager"))
            GameObject.Find("CameraManager")?.TryGetComponent<CameraManager>(out cameraManager);
        else
            cameraManager = null;

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
        cameraManager?.Init();
        //titleSceneManager?.Init();
        //scenarioManager?.Init();
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        GameManager.Instance.player.GetComponent<Entity>().Init(1200, 5);
        Money = 10;

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

        // GetBool  isTutorial End ?
        int i = PlayerPrefs.GetInt("StageData", 0);

        for (; i < stageData.Count; i++)
        {
            Debug.Log("Stage - " + (i));

            for (int j = 0; j < stageData[i].spawnDatas.Count; j++)
            {
                var spawnData = stageData[i].spawnDatas[j];

                for (int k = 0; k < spawnData.spawnCount; k++)
                {
                    spawned = EnemySpawner.GetSpawnerById(spawnData.spawnerID)?.Spawn(spawnData.spawnType, spawnData.weaponType);
                    spawned?.GetComponent<Entity>().Init(spawnData.hp, spawnData.speed);
                    spawnedEntities.Add(spawned?.GetComponent<Entity>());

                    yield return YieldInstructionCache.WaitForSeconds(spawnData.spawnTerm);
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


            chatCount = 0;

            UIManager.Instance.UI_Chat_OpenClose();
            while (chatCount < stageData[i].chatDatas.Count)
            { 
                UIManager.Instance.UI_Chat_Set(stageData[i].chatDatas[chatCount]);
                yield return null;
            }
            UIManager.Instance.UI_Chat_OpenClose();

            StageClear(TUTORIAL_COUNT - 2 < i);
            UIManager.Instance.AddSlots(stageData[i].shopItemDatas);

            if (i == TUTORIAL_COUNT - 2) ReadyToStage();

            UIManager.Instance.UI_Chat_Set(stageData[i].shopChatData);

            if (i == TUTORIAL_COUNT - 1)  // Set Tutorial End
            {
                PlayerPrefs.SetInt("StageData", TUTORIAL_COUNT);
                PlayerPrefs.Save();
            }



            while (!isNextStageReady)
            {
                yield return null;
            }
        }

       



        yield return null;
    }

    public void StageClear(bool npcUse = true)
    {
        isNextStageReady = false;

        if (!npcUse) return;

        var npcPos = GameManager.Instance.player.transform.position;
        npcPos.y = 0;
        npcPos.z += 1f;
        clearNpc.transform.position = npcPos;
        clearNpc.gameObject.SetActive(true);
    }

    public void ReadyToStage()
    {
        isNextStageReady = true;

        clearNpc.gameObject.SetActive(false);

    }

    public void UpgradePlayer(ItemType itemType, float value, int cost)
    {
        if (cost > Money) return;

        Money -= cost;

        switch (itemType)
        {
            case ItemType.AttackPowerPlus:
                GameManager.Instance.Player.GetComponent<Status>().AttackPower += value;
                break;
            case ItemType.AttackSpeedPlus:
                //btn.onClick.AddListener(() => GameManager.Instance.Player.GetComponent<Status>().AttackSpeed += data.value);
                //slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text += "%";
                break;
            case ItemType.MoveSpeedPlus:
                GameManager.Instance.Player.GetComponent<Status>().MoveSpeed += value;
                break;
            case ItemType.Heal:
                GameManager.Instance.Player.GetComponent<PlayerController>().GetHeal(400);
                break;
            case ItemType.Else:
                break;
        }
    }
}