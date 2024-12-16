using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public GameScene_SO sceneToGo;
    public Vector3 positionToGo;
    public SceneLoadEvent_SO sceneLoadEvent;

    public void TriggerAction()
    {
        sceneLoadEvent.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
}
