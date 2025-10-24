using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float stopDistance = 0f;

    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    private Animator animator;

    private Vector2 lastNonZeroDir = Vector2.down;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;

        Vector2 toPlayer = (Vector2)(target.position - transform.position);
        float dist = toPlayer.magnitude;

        bool shouldMove = stopDistance <= 0f || dist > stopDistance;

        if (shouldMove)
        {
            moveDirection = toPlayer.normalized;

            
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", moveDirection.x);
            animator.SetFloat("InputY", moveDirection.y);

            
            if (moveDirection.sqrMagnitude > 0.0001f)
                lastNonZeroDir = moveDirection;
        }
        else
        {
            
            moveDirection = Vector2.zero;
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastNonZeroDir.x);
            animator.SetFloat("LastInputY", lastNonZeroDir.y);
        }
     

    }

    private void FixedUpdate()
    {
        if (!target) return;

        // Apply velocity from Update’s computed moveDirection
        rb.velocity = moveDirection * moveSpeed;

        // If we’ve effectively stopped, make sure idle last-facing is correct
        if (rb.velocity.sqrMagnitude <= 0.0001f)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastNonZeroDir.x);
            animator.SetFloat("LastInputY", lastNonZeroDir.y);
        }

        //if (target)
        //{
        //    rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        //}
    }

    private void OnDisable()
    {
        // Clean reset when disabled/destroyed
        if (rb) rb.velocity = Vector2.zero;
        if (animator)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastNonZeroDir.x);
            animator.SetFloat("LastInputY", lastNonZeroDir.y);
        }
    }
}
