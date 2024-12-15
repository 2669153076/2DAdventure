using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private PhysicsCheck check;
    private PlayerController playerControl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        check = GetComponent<PhysicsCheck>();
        playerControl= GetComponent<PlayerController>();
    }
    private void Update()
    {
        anim.SetFloat("VelocityX",Mathf.Abs(rb.velocity.x));
        anim.SetFloat("VelocityY",rb.velocity.y);
        anim.SetBool("IsGround", check.isGround);
        anim.SetBool("IsCrouch", playerControl.isCrouch);
        anim.SetBool("IsDead", playerControl.isDead);
        anim.SetBool("IsAttack", playerControl.isAttack);
        anim.SetBool("IsOnWall", check.isOnWall);
        anim.SetBool("IsSlide", playerControl.isSlide);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }

}
