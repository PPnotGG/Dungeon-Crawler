using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    public AIPath AIPath;
    private Rigidbody2D rb;
    public Transform target;

    public bool stunned = false;
    public float stunTime = 0;

    [Header("Enemy settings")]
    [SerializeField] public int maxHealth;
    [SerializeField] public GameObject model;
    [SerializeField] public Animator animator;
    [SerializeField] public float speed;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        AIPath.maxSpeed = speed; 
    }

    //Получение урона
    public void TakeDamage(int damage)  
    {
        stunned = true;
        stunTime = Time.time + 0.6f;
        AIPath.maxSpeed = 0;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
            Die();
    }

    //Смерть
    private void Die()
    {
        model.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (stunned)
        {
            if (Time.time >= stunTime)
            {
                stunned = false;
                AIPath.maxSpeed = speed;
            }
        }
    }
}
