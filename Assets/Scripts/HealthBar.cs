using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public PlayerMovement player;
    public int maxHealth = 3;

    void Update()
    {
        maxHealth = player.maxHealth;
        //Debug.Log(player.GetHealth());
        float percent = (float)player.GetHealth() / maxHealth;
        fillImage.fillAmount = Mathf.Clamp01(percent);
    }
}
