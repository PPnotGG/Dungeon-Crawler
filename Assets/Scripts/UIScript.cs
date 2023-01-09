using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    private float refreshTime;
    private string energyStr = "100";
    private string healthStr = "2";
    [Header("Player stats")]
    [SerializeField] public int energyInt = 100;
    [SerializeField] public int healthInt = 2;

    [Header("UI elements")]
    [SerializeField] public Text energyText;
    [SerializeField] public Text healthText;
    [SerializeField] public GameObject DeathWarning;
    [SerializeField] public Slider slider;

    public bool refreshFlag = false;
    public float refrRecTime = 0;
    public PlayerScript playerScript;

    public void SetEnergy(int energy)
    {
        slider.value = energy;
    }

    //Выход в хаб
    public void Respawn()
    {
        SceneManager.LoadScene("Hub");
    }

    void Update()
    {
        SetEnergy(energyInt);
        //Смерть игрока
        if (healthInt <= 0)
        {
            playerScript.Die();
            DeathWarning.SetActive(true);
        }
        if (refreshFlag)
        {
            //Восстановление энергии со временем
            if (Time.time >= refreshTime)
            {
                energyInt += 5;
                refreshTime = Time.time + 0.1f;
            }
            if (energyInt >= 100)
            {
                energyInt = 100;
                refreshFlag = false;
            }
        }
        if (Time.time >= refrRecTime)
            refreshFlag = true;
        //Отображение параметров
        energyStr = energyInt.ToString();
        energyText.text = energyStr;
        healthStr = healthInt.ToString();
        healthText.text = healthStr;
        //Выход в меню без сохранений
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
