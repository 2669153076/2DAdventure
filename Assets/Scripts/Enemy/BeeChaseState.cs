using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    private Attack attack;
    private bool isAttack;
    private float attackRateCounter;

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(E_NPCState.Patrol);
        }

        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);

        //攻击
        if(Mathf.Abs(target.x-currentEnemy.transform.position.x)<=attack.attackRange&& Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            if(!currentEnemy.isHurt)
            { 
                currentEnemy.rb.velocity = Vector2.zero; 
            }
            isAttack = true;

            attackRateCounter -=Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                attackRateCounter = attack.attackRate;
                currentEnemy.animator.SetTrigger("Attack");
            }
        }
        else
        {
            isAttack = false;
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.normalSpeed = currentEnemy.chaseSpeed;
        attack = currentEnemy.GetComponent<Attack>();
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;

        currentEnemy.animator.SetBool("IsChest", true);
    }

    public override void OnExit()
    {

        currentEnemy.animator.SetBool("IsChest", false);
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead&&!isAttack)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
    }

}
