using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject player;
    private Vector2 direction;
    private Vector2 lastDir;
    //Новый рывок не оправдал ожиданий
    //private Vector2 DashDir;
    //Параметры старого рывка
    private bool isDashButtonDown = false;
    private RaycastHit2D raycastHit2D;
    private Vector2 dashPosition;

    //Состояния
    public enum State
    {
        Normal, 
        Attacking,
        //Dodging
    }
    //Хранение текущего состояния
    public State state;

    public UIScript UI;

    [Header("Player movement settings")]
    [SerializeField] public float speed = 1f;
    [SerializeField] private float dashDistance = 20f;
    [SerializeField] private LayerMask dashLayerMask;
    //[SerializeField] private float dashSpeed;
    //private float actualDashSpeed;

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
        state = State.Normal;
        //actualDashSpeed = dashSpeed;
    }

    //Старый рывок в виде телепортации
    private void Dash()
    {
        dashPosition = rb.position + lastDir * dashDistance;
        raycastHit2D = Physics2D.Raycast(transform.position, lastDir, dashDistance, dashLayerMask);
        if (raycastHit2D.collider != null)
            dashPosition = raycastHit2D.point;
        rb.MovePosition(dashPosition);
        UI.energyInt -= 20;
        UI.refrRecTime = Time.time + 3f;
        UI.refreshFlag = false;
        isDashButtonDown = false;
    }

    //Новый рывок
    //private void NewDash()
    //{
    //    actualDashSpeed = dashSpeed;
    //    state = State.Dodging;
    //    DashDir = lastDir;
    //    UI.energyInt -= 20;
    //    UI.refrRecTime = Time.time + 3f;
    //    UI.refreshFlag = false;
    //}

    //Проверка на нажатие кнопок атаки
    private void CheckForAttack()
    {
        //Проверка на откат атаки (чтобы не спамить ими)
        if (Time.time >= nextAttackTime)
        {
            //Проверки на направление атаки и энергию
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
        state = State.Attacking;  //Состояние атаки
        animator.SetTrigger(trigger);  //Триггер для одной из четырех анимаций атак
        nextAttackTime = Time.time + attackTime;  //Установка времени отката
        UI.energyInt -= 10;  
        UI.refrRecTime = Time.time + 3f;  //Откат восстановления энергии (Лучше перенести в UIScript)
        UI.refreshFlag = false;
        //Проверка на то, ударил ли игрок кого-то
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(damage);
        }
    }

    //Скрипт для удобства отображения области атаки
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Смерть
    public void Die()
    {
        player.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            case State.Normal:
                CheckForAttack();
                //Считывание нажатий игрока для управления моделькой
                direction = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1")).normalized;
                //Получение последнего направления движения
                if (direction.y != 0 || direction.x != 0)
                    lastDir = direction;
                //Анимации
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                animator.SetFloat("Speed", direction.sqrMagnitude);
                //Старый рывок
                if (Input.GetKeyDown(KeyCode.Space) && UI.energyInt >= 20)
                    isDashButtonDown = true;
                //Проверка на новый рывок
                //if (Input.GetKeyDown(KeyCode.Space) && UI.energyInt >= 20)
                //    NewDash();
                break;

            case State.Attacking:
                if (Time.time >= nextAttackTime)
                    state = State.Normal;  //Переход в нормальное состояние
                break;

            //case State.Dodging:
            //    //Значение убывания скорости при рывке
            //    float dashSpeedDropMult = 15f;
            //    //Самр убывание скорости
            //    actualDashSpeed -= actualDashSpeed * dashSpeedDropMult * Time.deltaTime;
            //    if (actualDashSpeed <= 6f)
            //        state = State.Normal;  //Переход в нормальное состояние
            //    break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                //Перемещение модельки
                rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
                if (isDashButtonDown)
                    Dash();
                break;

            case State.Attacking:
                //Стоять на месте
                rb.velocity = Vector2.zero;
                break;

            //case State.Dodging:
            //    //Рывок(новый)
            //    rb.velocity = DashDir * actualDashSpeed;
            //    break;
        }
    }
}
