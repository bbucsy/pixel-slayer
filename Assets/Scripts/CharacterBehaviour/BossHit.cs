using Unity.Mathematics;
using UnityEngine;

namespace CharacterBehaviour
{
    public class BossHit : CharacterHit
    {


        public bool useEffect = true;
        public float effectForce = 300;

        public GameObject player;

        protected override void DamageEnemies(CharacterHealth healthScript)
        {
            base.DamageEnemies(healthScript);
            var rb = healthScript.gameObject.GetComponent<CharacterMovement>();

            if (rb == null || player == null || !useEffect) return;
            
            var dx = player.transform.position.x - gameObject.transform.position.x;
            var adx = math.abs(dx);
            rb.ThrowCharacter(new Vector2(dx/adx * effectForce,1*effectForce), 1);
        }
    }
}
