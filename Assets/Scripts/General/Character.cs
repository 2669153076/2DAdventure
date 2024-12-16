using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character : MonoBehaviour, ISaveable
{
    public float curHp;
    public float maxHp;

    public float invulnerableDuration; //无敌时间
    public float invulnerableCounter;  //计时器
    public bool isInvulnerable;    //是否无敌

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;

    public UnityEvent<Character> OnHealthChange;

    [Header("滑铲相关")]
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed; //能量回复速度

    public VoidEvent_SO newGameEvent;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }


    private void NewGame()
    {
        curHp = maxHp;
        OnHealthChange?.Invoke(this);
        currentPower = maxPower;
    }

    private void Update()
    {
        if(isInvulnerable)
        {
            invulnerableCounter-=Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                isInvulnerable = false;
            }
        }
        if (currentPower < maxPower)
        {
            currentPower += powerRecoverSpeed*Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if(curHp > 0)
            {
                curHp = 0;
                OnHealthChange?.Invoke(this);
                OnDead?.Invoke();
            }
        }
    }

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="atk"></param>
    public void TakeDamage(Attack atk)
    {
        if (isInvulnerable)
        {
            return;
        }

        if(curHp-atk.damage>0){
            curHp -= atk.damage;
            //受伤后触发无敌
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(atk.transform);
        }
        else
        {
            //死亡
            curHp = 0;
            OnDead?.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }

    /// <summary>
    /// 触发无敌
    /// </summary>
    private void TriggerInvulnerable()
    {
        if(!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public void OnSlide(float cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();  
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDic.ContainsKey(GetDataID().id))
        {
            data.characterPosDic[GetDataID().id] = new SerializeVector3( transform.position);
            data.floatSaveData[GetDataID().id + "curHp"] = this.curHp;
            data.floatSaveData[GetDataID().id + "currentPower"] = this.currentPower;
        }
        else
        {
            data.characterPosDic.Add(GetDataID().id, new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().id+ "curHp", this.curHp); 
            data.floatSaveData.Add(GetDataID().id+ "currentPower", this.currentPower); 
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDic.ContainsKey(GetDataID().id))
        {
            transform.position = data.characterPosDic[GetDataID().id].ToVector3() ;

            this.curHp = data.floatSaveData[GetDataID().id + "curHp"];
            this.currentPower = data.floatSaveData[GetDataID().id + "currentPower"];

            OnHealthChange?.Invoke(this);
        }

    }
}
