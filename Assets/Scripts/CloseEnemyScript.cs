using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target;

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
        animator.SetFloat("Horizontal", target.position.x - transform.position.x);
        animator.SetFloat("Vertical", target.position.y - transform.position.y);
    }
}
