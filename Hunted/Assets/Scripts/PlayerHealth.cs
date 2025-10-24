using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 10;
    
    GameObject player;

    AudioManager audioManager;

    public HealthBar healthBar;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //Fill in later when damage is taken
        //if (damage)
        //{
        //    TakeDamage(1);
        //}
        //if (damage2)
        //{
        //    TakeDamage(2);
        //}
        //if (damage3)
        //{
        //    TakeDamage(3);
        //}

        if (Input.GetKeyDown(KeyCode.Backspace)){
            TakeDamage(1);
        }

        if(currentHealth == 0)
        {
            if (audioManager.PlaySFX(audioManager.death))
            {
                Destroy(player);
                SceneManager.LoadSceneAsync(7);
            }
            
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
