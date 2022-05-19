using System;
using System.Collections;
using System.Collections.Generic;
using CharacterBehaviour;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : BaseController
{
    public float attackRange = 0.5f;
    public float detectRange = 5;
    public float detectThreshold = 0.46f;
    public PlayerController player;

    [Header("Component references")] 
    public CharacterMovement movementScript;
    public CharacterAttack attackScript;
    
    private void Reset()
    {
        if (movementScript == null) movementScript = GetComponent<CharacterMovement>();
        if (attackScript == null) attackScript = GetComponent<CharacterAttack>();
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        // detect player distance
        var dx = player.gameObject.transform.position.x - gameObject.transform.position.x;
        var adx = math.abs(dx);

        if (adx < attackRange)
        {
                movementScript.Move(0);
               if(!attackScript.IsAttacking) attackScript.Attack();
        }
        else if ( adx < detectRange && adx > detectThreshold)
        {
            movementScript.Move(dx / adx);
        }
        else
        {
            movementScript.Move(0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position,attackRange);
        
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(transform.position, detectThreshold);

        
    }

    public override void OnCharacterDeath()
    {
        var score = GameObject.FindObjectOfType<PointController>();
        if (score != null)
        {
            score.Points++;
        }
        base.OnCharacterDeath();
    }
}
