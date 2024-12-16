using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SceneLoadEvent_SO", menuName = "Event/SceneLoadEvent")]
public class SceneLoadEvent_SO : ScriptableObject
{
    public UnityAction<GameScene_SO, Vector3, bool> LoadRequestEvent;

    public void RaiseLoadRequestEvent(GameScene_SO gameScene,Vector3 pos,bool fadeScene)
    {
        LoadRequestEvent?.Invoke(gameScene, pos, fadeScene);
    }
}
