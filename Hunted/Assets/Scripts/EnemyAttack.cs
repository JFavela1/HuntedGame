using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private string playerObjectName = "Player";
    [SerializeField] private float attackRange = 1.0f;

    [Header("Attack")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.8f;

    private Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;
    private float lastAttackTime;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        var pObj = GameObject.Find(playerObjectName);
        if (pObj)
        {
            player = pObj.transform;
            playerHealth = pObj.GetComponent<PlayerHealth>(); // uses your existing script
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!player || !playerHealth) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            playerHealth.currentHealth = Mathf.Max(0, playerHealth.currentHealth - damage);
            if (playerHealth.healthBar) playerHealth.healthBar.SetHealth(playerHealth.currentHealth);

            audioManager.PlaySFX(audioManager.hit);
        }
    }

}
