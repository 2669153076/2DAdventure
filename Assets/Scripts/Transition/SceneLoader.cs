using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public SceneLoadEvent_SO sceneLoadEvent;
    public GameScene_SO firstLoadScene;


    private GameScene_SO sceneToLoad;
    private Vector3 posToGo;
    private bool isFade;

    private GameScene_SO currentScene;
    [SerializeField]private float fadeDuraction = 0.5f;

    public Transform player;

    public VoidEvent_SO afterSceneLoadEvent;
    private bool isLoading;

    public Vector3 firstPos;

    public FadeEvent_SO fadeEvent;

    public Vector3 menuPos;
    public GameScene_SO menuScene;

    public VoidEvent_SO newGameEvent;

    public SceneLoadEvent_SO sceneUnLoadEvent;

    public VoidEvent_SO backToMenuEvent;


    private void Start()
    {
        sceneLoadEvent.RaiseLoadRequestEvent(menuScene, menuPos, true);
        //NewGame();
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.RegisterSaveData();

    }
    private void OnDisable()
    {

        sceneLoadEvent.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnLoadRequestEvent(GameScene_SO arg0, Vector3 arg1, bool arg2)
    {
        if(isLoading)
        {
            return;
        }

        isLoading = true;

        sceneToLoad = arg0;
        posToGo = arg1;
        isFade = arg2;

        if(currentScene != null){
            StartCoroutine(UnLoadScene());
        }
        else
        {
            LoadScene();
        }
    }

    private IEnumerator UnLoadScene()
    {
        if(isFade)
        {
            fadeEvent.FadeIn(fadeDuraction);
        }

        yield return new WaitForSeconds(fadeDuraction);

        //隐藏血条
        sceneUnLoadEvent.RaiseLoadRequestEvent(sceneToLoad,posToGo,true);

        if (currentScene != null)
        {
            yield return currentScene.assetReference.UnLoadScene();
        }

        player.gameObject.SetActive(false);

        LoadScene();
    }

    private void LoadScene()
    {
        var loadOperation = sceneToLoad.assetReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadOperation.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentScene = sceneToLoad;

        player.gameObject.SetActive(true);
        player.position = posToGo;

        if(isFade)
        {
            fadeEvent.FadeOut(fadeDuraction);
        }
        isLoading = false;

        afterSceneLoadEvent.RaiseEvent();
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        OnLoadRequestEvent(sceneToLoad, firstPos, false);
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentScene);
    }

    public void LoadData(Data data)
    {
        var playerId = player.GetComponent<DataDefination>().id;
        if(data.characterPosDic.ContainsKey(playerId))
        {
            posToGo = data.characterPosDic[playerId].ToVector3();
            sceneToLoad = data.LoadGameScene();

            OnLoadRequestEvent(sceneToLoad,posToGo,true);
        }
    }


    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        sceneLoadEvent.RaiseLoadRequestEvent(sceneToLoad, menuPos, true);
    }
}
