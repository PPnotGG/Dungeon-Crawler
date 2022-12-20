using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    private float nextAttackTime = 0f;
    public PlayerScript PlayerScript;

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            PlayerScript.movementPermission = true;
            if (Input.GetKeyDown("g"))
            {
                Attack("Attack-left");
            }
            if (Input.GetKeyDown("j"))
            {
                Attack("Attack-right");
            }
            if (Input.GetKeyDown("y"))
            {
                Attack("Attack-up");
            }
            if (Input.GetKeyDown("h"))
            {
                Attack("Attack-down");
            }
        }
    }

    void Attack(string trigger)
    {
        PlayerScript.movementPermission = false;
        animator.SetTrigger(trigger);
        nextAttackTime = Time.time + 3f / 5f;
    }
}