using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterAttack))]
public class PlayerController : MonoBehaviour
{
    public CharacterMovement movementScript;
    public CharacterAttack attackScript;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool attack = false;

    private void Reset()
    {
        if (movementScript == null) movementScript = GetComponent<CharacterMovement>();
        if (attackScript == null) attackScript = GetComponent<CharacterAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            attack = true;
        }
    }

    private void FixedUpdate()
    {
        movementScript.Move(horizontalMove, attackScript.IsAttacking);

        if (attack && movementScript.IsGrounded)
        {
            attackScript.Attack();
        }

        if (jump && !attackScript.IsAttacking)
        {
            movementScript.Jump();
        }

        jump = false;
        attack = false;
    }
}