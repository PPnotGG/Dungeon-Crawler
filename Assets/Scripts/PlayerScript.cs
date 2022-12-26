using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject player;
    private Vector2 direction;
    private bool isDashButtonDown = false;
    private Vector2 lastDir;
    private RaycastHit2D raycastHit2D;
    private Vector2 dashPosition;

    public bool movementPermission = true;
    public UIScript UI;

    [Header("Player movement settings")]
    [Range(0f, 10f)] public float speed = 1f;
    [SerializeField] public float dashDistance = 20f;
    [SerializeField] private LayerMask dashLayerMask;

    [Header("Player animation settings")]
    public Animator animator;

    [Header("Attack Parameters")]
    [SerializeField] public int damage = 2;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public LayerMask enemyLayers;
    [SerializeField] public float attackRange = 0.5f;

    [Header("Attack time Parameters")]
    [SerializeField] public float attackTime;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Dash()
    {
        dashPosition = rb.position + lastDir * dashDistance;
        raycastHit2D = Physics2D.Raycast(transform.position, lastDir, dashDistance, dashLayerMask);
        if (raycastHit2D.collider != null)
            dashPosition = raycastHit2D.point;
        rb.MovePosition(dashPosition);
        UI.energyInt -= 25;
        UI.refrRecTime = Time.time + 3f;
        UI.refreshFlag = false;
        isDashButtonDown = false;
    }

    private void CheckForAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            movementPermission = true;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && UI.energyInt >= 10)
            {
                attackPoint = transform.Find("AttackPointLeft");
                Attack("Attack-left");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && UI.energyInt >= 10)
            {
                attackPoint = transform.Find("AttackPointRight");
                Attack("Attack-right");
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && UI.energyInt >= 10)
            {
                attackPoint = transform.Find("AttackPointUp");
                Attack("Attack-up");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && UI.energyInt >= 10)
            {
                attackPoint = transform.Find("AttackPointDown");
                Attack("Attack-down");
            }
        }
    }

    private void Attack(string trigger)
    {
        movementPermission = false;
        animator.SetTrigger(trigger);
        nextAttackTime = Time.time + attackTime;
        UI.energyInt -= 10;
        UI.refrRecTime = Time.time + 3f;
        UI.refreshFlag = false;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Die()
    {
        player.SetActive(false);
    }

    void Update()
    {
        CheckForAttack();
        direction = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1")).normalized;
        if (direction.y != 0 || direction.x != 0)
            lastDir = direction;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        if (Input.GetKeyDown(KeyCode.Space) && UI.energyInt >= 25 && movementPermission)
            isDashButtonDown = true;
    }

    private void FixedUpdate()
    {
        if (movementPermission)
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

        if (isDashButtonDown)
            Dash();
    }
}
