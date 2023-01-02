using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsScript : MonoBehaviour
{
    [SerializeField] private UIScript ui;
    [SerializeField] private PlayerScript playerScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверка на игрока
        if (collision.name == "Player")
        {
            ui.healthInt -= 1;
        }
    }
}
