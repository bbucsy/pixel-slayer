using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    public Animator animator;
    public bool canAttack = true;
    public bool canCombo = false;
    public bool isAttacking = false;
    public bool isComboing = false;
    public float attackCooldownSecs = 1.5f;
    public PlayerMovement movescript;
    
    // Update is called once per frame
    void Update()
    {
        
        if (  !Input.GetButton("Fire1") || !movescript.wasGrounded) return;
        
        if(!isAttacking && canAttack) Attack();
        else if (isAttacking && canCombo && !isComboing) Combo();
    }

    IEnumerator startAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownSecs);
        canAttack = true;
    }
    
    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(startAttackCooldown());
    }


    void Combo()
    {
        isComboing = true;
        animator.SetTrigger("Combo");
    }

    void onAttackComboWindow()
    {
        canCombo = true;
    }

    void onAttackWindowComboClose()
    {
        canCombo = false;
    }
    
    
    void onAttackEnd()
    {
        isAttacking = isComboing;
    }

    void onComboEnd()
    {
        isComboing = false;
        isAttacking = false;
    }
}
