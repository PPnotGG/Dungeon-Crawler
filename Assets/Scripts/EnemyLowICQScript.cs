using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLowICQScript : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    private float distance;
    public EnemyScript enemyScript;
    public float stunTime = 0f;

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        if (distance < distanceBetween && !enemyScript.stunned)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if (enemyScript.stunned)
        {
            if (Time.time >= stunTime)
                enemyScript.stunned = false;
        }
    }
}
