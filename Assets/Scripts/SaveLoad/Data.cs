using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{
    public Dictionary<string, SerializeVector3> characterPosDic = new Dictionary<string, SerializeVector3>();   //角色坐标
    public Dictionary<string,float> floatSaveData = new Dictionary<string,float>();//保存所有float类型的值
    public string sceneToSave;

    public void SaveGameScene(GameScene_SO gameScene_SO)
    {
        sceneToSave = JsonUtility.ToJson(gameScene_SO);
    }
    public GameScene_SO LoadGameScene()
    {
       var newScene = ScriptableObject.CreateInstance<GameScene_SO>();
         JsonUtility.FromJsonOverwrite(sceneToSave, newScene);   

        return newScene;

    }

}

public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
