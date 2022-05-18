using UnityEngine;

namespace CharacterBehaviour
{
        public class CharacterHealth : MonoBehaviour, ICharacterBehaviour
        {

                [SerializeField] private int health = 100;
                [SerializeField] private int maxHealth = 100;
                [SerializeField] private GameObject hpBar;
        

                [Header("Component references")]
                [SerializeField] private Animator animator;

                void Reset()
                {
                        if (animator == null) animator = GetComponent<Animator>();
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
                                var hpv = (float) health / (float)maxHealth;
                                hpv = hpv < 0 ? 0 : hpv;
                                hpBar.transform.localScale = new Vector3(hpv, 1, 1);
                        }

                
                        if (health <= 0)
                        {
                                Die();
                                return;
                        }
                        // show hit animation       
                        animator.SetTrigger("Hit");


                }


                private void Die()
                {
                        animator.SetTrigger("Die");
                        var controllers = gameObject.GetComponents<BaseController>();

                        foreach (var controller in controllers)
                        {
                                controller.OnCharacterDeath();
                        }

                }
        }
}
