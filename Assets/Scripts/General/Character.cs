using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public int curHp;
    public int maxHp;

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

    private void Start()
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
}
