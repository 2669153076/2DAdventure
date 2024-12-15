using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSkillState : BaseState
{
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(E_NPCState.Patrol);
        }

        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.GetComponent<Character>().invulnerableDuration;
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("IsWalk", false);
        currentEnemy.animator.SetBool("IsHide", true);
        currentEnemy.animator.SetTrigger("Skill");

        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().isInvulnerable = true;
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.GetComponent<Character>().invulnerableDuration;

    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("IsHide", false);
        currentEnemy.GetComponent<Character>().isInvulnerable = false;
    }

    public override void PhysicsUpdate()
    {

    }

}
