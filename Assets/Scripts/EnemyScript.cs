using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Animator animator;
    public GameObject model;
    private Vector3 startingPos;
    public bool stunned = false;
    public EnemyLowICQScript enemyLowICQ;
        
    [Header("Enemy settings")]
    [SerializeField] public int maxHealth = 1;
    private int currentHealth;

    private void Start()
    {
        startingPos = transform.position;
        currentHealth = maxHealth;
    }

    //private Vector3 GetRoamingPosition()
    //{
    //    return startingPos + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * Random.Range(1f, 3f);
    //}

    public void TakeDamage(int damage)  
    {
        stunned = true;
        enemyLowICQ.stunTime = Time.time + 1f;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Enemy is dead");
        model.SetActive(false);
    }
}
