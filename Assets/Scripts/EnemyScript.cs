using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyScript : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    public GameObject model;
    public bool stunned = false;
    public float stunTime = 0;


    [Header("Enemy settings")]
    [SerializeField] public int maxHealth = 1;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    //Получение урона, ага
    public void TakeDamage(int damage)  
    {
        stunned = true;
        stunTime = Time.time + 0.7f;
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
}
