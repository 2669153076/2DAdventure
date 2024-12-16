using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public string id;
    public E_PersistentType persistentType;

    private void OnValidate()
    {
        if (persistentType == E_PersistentType.ReadWrite)
        {
            if (id == string.Empty)
            {
                id = System.Guid.NewGuid().ToString();
            }
        }
        else
        {
            id = string.Empty;
        }
    }
}
