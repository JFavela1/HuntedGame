using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("Timing")]
    [SerializeField] private float attackCooldown = 0.35f;
    [SerializeField] private float windup = 0.08f;
    [SerializeField] private float recovery = 0.15f;

    [Header("Hitbox")]
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float hitRadius = 0.85f;
    [SerializeField] private LayerMask enemyLayers; 

    [Header("Animator")]
    [SerializeField] private string attackTriggerName = "Attack";
    [SerializeField] private string isAttackingBoolName = "isAttacking";
    [SerializeField] private bool useAnimationEvent = false;

    private Animator animator;
    private bool isAttacking;
    private bool hasDealtDamageThisSwing;
    private float nextAllowedTime;
    AudioManager audioManager;

    void Awake()
    {
        animator = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
        if (!hitPoint)
        {
            var hp = new GameObject("HitPoint");
            hp.transform.SetParent(transform);
            hp.transform.localPosition = new Vector3(0.8f, 0.8f, 0f);
            hitPoint = hp.transform;
        }
    }

    
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        TryStartAttack();
    }

    private void TryStartAttack()
    {
        if (isAttacking) return;
        if (Time.time < nextAllowedTime) return;

        nextAllowedTime = Time.time + attackCooldown;
        hasDealtDamageThisSwing = false;
        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (animator && !string.IsNullOrEmpty(isAttackingBoolName))
            animator.SetBool(isAttackingBoolName, true);
        if (animator && !string.IsNullOrEmpty(attackTriggerName))
            animator.SetTrigger(attackTriggerName);

        if (!useAnimationEvent)
        {
            
            yield return new WaitForSeconds(windup);
            DoHit();                 
            yield return new WaitForSeconds(recovery);
        }
        else
        {
            float guard = Mathf.Max(0.01f, attackCooldown * 0.6f);
            yield return new WaitForSeconds(guard);
        }

        isAttacking = false;
        if (animator && !string.IsNullOrEmpty(isAttackingBoolName))
            animator.SetBool(isAttackingBoolName, false);
    }

    public void OnAttackHit()
    {
        if (!isAttacking) return;
        if (hasDealtDamageThisSwing) return; 
        DoHit();
    }

    private void DoHit()
    {
        hasDealtDamageThisSwing = true;
        if (!hitPoint) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint.position, hitRadius, enemyLayers);
        foreach (var col in hits)
        {
            audioManager.PlaySFX(audioManager.hit);
            var eh = col.GetComponent<EnemyHealth>();
            if (eh) { eh.ApplyDamage(damage); continue; }

            
            var mb = col.GetComponent<MonoBehaviour>();
            if (mb != null)
            {
                var mi = mb.GetType().GetMethod("Damage");
                if (mi != null) mi.Invoke(mb, new object[] { damage });
            }
        }
    }

}
