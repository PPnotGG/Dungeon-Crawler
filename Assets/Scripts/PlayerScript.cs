using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    public bool movementPermission = true;

    [Header("Player movement settings")]
    [Range(0f, 10f)] public float speed = 1f;

    [Header("Player animation settings")]
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (movementPermission)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }
}
