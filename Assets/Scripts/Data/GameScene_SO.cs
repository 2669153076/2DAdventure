using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


[CreateAssetMenu(fileName = "GameScene_SO", menuName = "GameScene/GameScene")]
public class GameScene_SO : ScriptableObject
{
    public AssetReference assetReference;
    public E_SceneType sceneType;
}
