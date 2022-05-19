using UnityEngine;

namespace CharacterBehaviour
{
        public class CharacterHealth : MonoBehaviour, ICharacterBehaviour
        {

                [SerializeField] private int health = 100;
                [SerializeField] private int maxHealth = 100;
                public HpBar hpBar;
                public AudioClip damageClip;
                public float Health => (float) health / maxHealth;


                [Header("Component references")]
                [SerializeField] private Animator animator;

                [SerializeField] private AudioSource audioSource;

                void Reset()
                {
                        if (animator == null) animator = GetComponent<Animator>();
                        if (audioSource == null) audioSource = GetComponent<AudioSource>();
                }

                private void Start()
                {
                        health = maxHealth;
                }

                public void Damage(int dmg)
                {
                        health -= dmg;
                
                        if (hpBar != null)
                        {
                               
                                hpBar.SetState(Health);
                        }

                
                        if (health <= 0)
                        {
                                Die();
                                return;
                        }
                        // show hit animation       
                        animator.SetTrigger("Hit");
                        audioSource.PlayOneShot(damageClip);

                }


                private void Die()
                {
                        animator.SetTrigger("Die");
                        animator.SetBool("isDead", true);
                        var controllers = gameObject.GetComponents<BaseController>();

                        foreach (var controller in controllers)
                        {
                                controller.OnCharacterDeath();
                        }

                }
        }
}
