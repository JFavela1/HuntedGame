using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float deathDestroyDelay = 0.2f;
    [SerializeField] private int nextScene = 0;


    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    private int currentHealth;
    private Animator animator;
    AudioManager audioManager;


    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    public void ApplyDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        onDamaged?.Invoke();

        if (currentHealth == 0)
        {
            audioManager.PlaySFX(audioManager.death);
            Destroy(gameObject, deathDestroyDelay);
            SceneManager.LoadSceneAsync(nextScene);
        }
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
