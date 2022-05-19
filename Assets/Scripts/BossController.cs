using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterBehaviour;
using Unity.Mathematics;
using UnityEngine;

public class BossController : BaseController
{
    [Serializable]
    public class PhaseOptions
    {
        public bool canCombo = false;
        public float attackDelay = 3;
        public bool useAttackEffect = false;
        public float moveSpeed = 3;
        public int damage = 15;
    }

    public enum Phase
    {
        Initial,
        Phase1,
        Phase2
    }

    public Phase phase = Phase.Initial;
    public float awakeRadius = 5;
    public float attackRadius = 3;
    public float phase1SafeRadius = 2;
    public PlayerController player;
    public LayerMask awakeLayers;
    public GameObject winSign;
    
    [Header("Phase options")] 
    public PhaseOptions initialStats;
    public PhaseOptions phase1Stats;
    public PhaseOptions phase2Stats;

    [Header("Component references")] public CharacterMovement movement;
    public CharacterAttack attack;
    public BossHit hit;
    public CharacterHealth hp;
    public AudioSource audioSource;

    [Header("Sounds")] 
    public AudioClip bossMusic;

    public AudioClip laughter;

    private void Start()
    {
        SwitchPhase(Phase.Initial);
    }

    private void Reset()
    {
        if (movement == null) movement = GetComponent<CharacterMovement>();
        if (attack == null) attack = GetComponent<CharacterAttack>();
        if (hit == null) hit = GetComponent<BossHit>();
        if (hp == null) hp = GetComponent<CharacterHealth>();
        if (audioSource == null) GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (phase == Phase.Initial)
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, awakeRadius, awakeLayers);
            foreach (var c in hitColliders)
            {
                var playerScript = c.gameObject.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    player = playerScript;
                    SwitchPhase(Phase.Phase1);
                }
            }
        }

        if (phase != Phase.Initial )
        {
            var dx = player.gameObject.transform.position.x - transform.position.x;
            var adx = math.abs(dx);

            if (adx <= attackRadius)
            {
                movement.Move(dx/adx * 0.001f);
                attack.Attack();
            }

            if (adx > phase1SafeRadius)
            {
                // move towards player
                movement.Move(dx / adx);
            }
            else
            {
                movement.Move(0);
            }

            if (phase == Phase.Phase1 && hp.Health < 0.5)
            {
                SwitchPhase(Phase.Phase2);
            }
        }
    }

    private void SwitchPhase(Phase p)
    {
        phase = p;
        switch (p)
        {
            case Phase.Initial:
                hp.hpBar.SetActive(false);
                setStats(initialStats);
                break;
            case Phase.Phase1:
                var bg = GameObject.Find("GameManager")?.GetComponent<AudioSource>();
                if (bg != null)
                {
                    bg.clip = bossMusic;
                    bg.Play();
                }
                audioSource.PlayOneShot(laughter);
                setStats(phase1Stats);
                hp.hpBar.SetActive(true);
                break;
            case Phase.Phase2:
                setStats(phase2Stats);
                break;
                
        }
    }

    private void setStats(PhaseOptions option)
    {
        attack.autoCombo = option.canCombo;
        attack.attackCooldownSecs = option.attackDelay;
        hit.damage = option.damage;
        hit.useEffect = option.useAttackEffect;
        movement.moveSpeed = option.moveSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, awakeRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, phase1SafeRadius);
    }

    public override void OnCharacterDeath()
    {
        winSign.SetActive(true);
        base.OnCharacterDeath();
        
    }
}