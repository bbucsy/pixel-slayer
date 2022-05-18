using System;
using UnityEngine;

namespace CharacterBehaviour
{

    public class CharacterMovement : MonoBehaviour, ICharacterBehaviour
    {
    
        [SerializeField] private float moveSpeed = 3.0f;
        [SerializeField] private float jumpForce = 100;
        [SerializeField] private float groundCheckExtraHeight = 0.05f;
        [SerializeField] private LayerMask platformLayerMask;

        [Header("Component references")]
        [SerializeField] private Rigidbody2D playerRb;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private Animator animator;

        public event Action OnGroundEnterEvent;
        
        private bool wasGrounded = true;
        private bool rightFacing = true;
    
        private void Reset()
        {
            if (playerRb == null) playerRb = GetComponent<Rigidbody2D>();
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (animator == null) animator = GetComponent<Animator>();
            if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
        }


        public bool IsGrounded
        {
            get
            {
                var bounds = boxCollider.bounds;
                var rayHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, groundCheckExtraHeight,
                    platformLayerMask);
                return rayHit.collider != null;
            }
        }


        public void Move(float direction, bool slowed = false)
        {
            if(!IsGrounded) return;

            var slowness = slowed ? 0.5f : 1f;
       
            var targetVelocity = new Vector2(direction * moveSpeed * slowness, playerRb.velocity.y);
            playerRb.velocity = targetVelocity;
        
            animator.SetFloat("Speed", Math.Abs(targetVelocity.x));


            if ((rightFacing && direction < 0) || (!rightFacing && direction > 0))
            {
                rightFacing = direction > 0;
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
            }

        }

        public void Jump()
        {
            if(!IsGrounded) return;
            playerRb.AddForce(new Vector2(0f,jumpForce));
        }
    
    
        private void OnGroundEnter()
        {
            OnGroundEnterEvent?.Invoke();
            animator.SetBool("isJumping", false);
        }

        private void OnGroundLeave()
        {
            animator.SetBool("isJumping",true);
        }

        private void OnDrawGizmosSelected()
        {
            var bounds = boxCollider.bounds;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center - Vector3.up * groundCheckExtraHeight,
                new Vector3(bounds.size.x, bounds.size.y, 1));
        }


        private void FixedUpdate()
        {
            // check for grounding events
            var grounded = IsGrounded;
            if(grounded && !wasGrounded) OnGroundEnter();
            if(!grounded && wasGrounded) OnGroundLeave();
            wasGrounded = grounded;
        
        }
    }
}