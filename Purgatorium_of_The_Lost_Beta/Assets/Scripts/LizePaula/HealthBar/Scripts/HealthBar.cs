using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public Image healthBar;

    [SerializeField]
    public TMP_Text healthBarText;

    private void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        Player_controller pc = Player_controller.instance;
        if (pc == null || pc.currentPlayerStats == null) return;
        if (healthBar == null || healthBarText == null) return;

        float current = pc.currentPlayerStats.currentHealth;
        float max     = pc.currentPlayerStats.maxHealth;

        healthBar.fillAmount = max > 0 ? current / max : 0f;
        healthBarText.text   = current + "/" + max;
    }
}