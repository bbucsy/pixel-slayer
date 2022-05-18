using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
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
        Gizmos.DrawWireCube(transform.position, new Vector3(detectRange,transform.localScale.y));
        Gizmos.DrawWireCube(transform.position, new Vector3(detectThreshold,transform.localScale.y));

        
    }
}
