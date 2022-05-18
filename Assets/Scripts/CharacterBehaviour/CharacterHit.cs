using UnityEngine;

namespace CharacterBehaviour
{
    public class CharacterHit : MonoBehaviour, ICharacterBehaviour
    {

        public LayerMask enemyLayers;
        public Transform hitPoint;
        public float hitRadius;
        public int damage = 10;
        public bool enableGizmo = false;


        public void HitEnemies()
        {
            if(!this.enabled) return;
            var hits = Physics2D.OverlapCircleAll(hitPoint.position, hitRadius, enemyLayers);

            foreach (var enemy in hits)
            {
                var healthScript = enemy.gameObject.GetComponent<CharacterHealth>();
                if (healthScript != null)
                {
                    healthScript.Damage(damage);
                }
            }
        }


        private void OnDrawGizmos()
        {
            if(!enableGizmo || hitPoint == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(hitPoint.transform.position, hitRadius);
        }
    }
}
