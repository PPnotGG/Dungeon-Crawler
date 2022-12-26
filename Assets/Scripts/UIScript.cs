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
    public int energyInt = 100;
    public int healthInt = 2;
    public Text energyText;
    public Text healthText;
    public bool refreshFlag = false;
    public float refrRecTime = 0;
    public PlayerScript playerScript;
    public GameObject DeathWarning;

    void Update()
    {
        if (healthInt <= 0)
        {
            playerScript.Die();
            DeathWarning.SetActive(true);
        }
        if (refreshFlag)
        {
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
        energyStr = energyInt.ToString();
        energyText.text = energyStr;
        healthStr = healthInt.ToString();
        healthText.text = healthStr;
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
