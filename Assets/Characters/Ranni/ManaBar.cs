using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    
    public Slider manaSlider;

    // Set the maximum mana value and update the slider
    public void SetMaxMana(float maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = maxMana; 
    }

    // Update the current mana value in the slider
    public void SetMana(float currentMana)
    {
        manaSlider.value = currentMana; // Update the slider value
    }
}
