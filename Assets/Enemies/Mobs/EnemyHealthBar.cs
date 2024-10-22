using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    private EnemyAI enemyAI;

    void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        healthSlider.maxValue = enemyAI.maxHealth;
        healthSlider.value = enemyAI.maxHealth;
    }

    void Update()
    {
        healthSlider.value = enemyAI.currentHealth;
    }
}
