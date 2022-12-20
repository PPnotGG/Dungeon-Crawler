using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 direction;

    [Header("Enemy movement settings")]
    [Range(0f, 10f)] public float speed = 1f;

    [Header("Enemy animation settings")]
    public Animator animator;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        direction.x = target.position.x - transform.position.x;
        direction.y = target.position.y - transform.position.y;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }
}
