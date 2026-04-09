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
}