using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHit : MonoBehaviour
{

    public LayerMask enemyLayers;
    public Transform hitPoint;
    public float hitRadius;
    public int damage = 10;
    public bool enableGizmo = false;


    public void HitEnemies()
    {
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
