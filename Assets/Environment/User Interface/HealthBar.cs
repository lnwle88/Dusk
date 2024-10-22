using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //Health
    public Slider HSlider;

    public void SetMaxHealth(int Health)
    {
        HSlider.maxValue = Health;
        HSlider.value = Health;
    }
    public void SetHealth(int Health)
    {
        HSlider.value = Health;
    }
}
