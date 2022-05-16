using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldownSecs = 1.5f;
    [SerializeField] private int comboFailTreshold = 0;

    
    [Header("Component references")]
    [SerializeField] private CharacterMovement movementScript;
    [SerializeField] private Animator animator;
    
    private bool canAttack = true;
    private bool canCombo = false;
    private bool isAttacking = false;
    private bool isComboing = false;
    private int failedCombo = 0;
 
    public  bool IsAttacking => isAttacking;

    private void Reset()
    {
        if (movementScript == null) movementScript = GetComponent<CharacterMovement>();
        if (animator == null) animator = GetComponent<Animator>();
    }
    


    public void Attack()
    {
        if ( !movementScript.IsGrounded) return;
        
        if(!isAttacking && canAttack) StartAttack();
        else if (isAttacking && canCombo && !isComboing) StartCombo();
        else if (isAttacking && !isComboing && !canCombo)
        {
            failedCombo++;
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(StartAttackCooldown());
    }

    private void StartCombo()
    {
        isComboing = true;
        animator.SetTrigger("Combo");
    }

    
    private IEnumerator StartAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownSecs);
        canAttack = true;
        isAttacking = false;
        isComboing = false;
        canCombo = false;
        failedCombo =0 ;
    }
    
    void OnAttackComboWindow()
    {
        if (failedCombo < comboFailTreshold)
            canCombo = true;
    }

    void OnAttackWindowComboClose()
    {
        canCombo = false;
    }
    
    
    void OnAttackEnd()
    {
        isAttacking = isComboing;
    }

    void OnComboEnd()
    {
        isComboing = false;
        isAttacking = false;
    }
}
