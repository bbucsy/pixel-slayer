using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldownSecs = 1.5f;
    [SerializeField] private int comboFailTreshold = 0;
    [SerializeField] private bool autoCombo = false;
    [SerializeField] private bool disableCombo = false;
    
    [Header("Component references")]
    [SerializeField] private CharacterMovement movementScript;
    [SerializeField] private Animator animator;
    
    private bool isNotCooldown = true;
    private bool canCombo = false;
    private bool isAttacking = false;
    private bool isComboActive = false;
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
        
        if(!isAttacking && isNotCooldown) StartAttack();
        else if (isAttacking && canCombo && !isComboActive) StartCombo();
        else if (isAttacking && !isComboActive && !canCombo)
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
        if(disableCombo) return;
        isComboActive = true;
        animator.SetTrigger("Combo");
    }

    
    private IEnumerator StartAttackCooldown()
    {
        isNotCooldown = false;
        yield return new WaitForSeconds(attackCooldownSecs);
        isNotCooldown = true;
        isAttacking = false;
        isComboActive = false;
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
        if(autoCombo) this.StartCombo();
        
        isAttacking = isComboActive;
    }

    void OnComboEnd()
    {
        isComboActive = false;
        isAttacking = false;
    }
}
