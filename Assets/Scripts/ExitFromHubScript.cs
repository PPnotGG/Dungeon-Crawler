using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFromHubScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверка на игрока
        if (collision.name == "Player")
        {
            SceneManager.LoadScene("Game");
        }
    }
}
