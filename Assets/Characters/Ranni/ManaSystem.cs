using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    // Mana
    public ManaBar manaBar; 
    public float maxMana = 100f; // Maximum mana
    private float currentMana; // Current mana
    public float manaRegenerationRate = 5f; // Mana regeneration per second
    public float manaCostPerProjectile = 10f; // Mana cost per projectile

    void Start()
    {
        SetMaxMana(maxMana); 
        StartCoroutine(RegenerateMana()); 
    }

    public void SetMaxMana(float mana)
    {
        maxMana = mana;
        currentMana = maxMana; 
        manaBar.SetMaxMana(maxMana); // Update the mana bar slider
    }

    public void SetMana(float mana)
    {
        currentMana = Mathf.Clamp(mana, 0, maxMana); // Ensure mana doesn't go below 0 or above max
        manaBar.SetMana(currentMana); // Update the mana bar slider
    }

    public float GetCurrentMana()
    {
        return currentMana; 
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            SetMana(currentMana - amount); // Deduct the mana
            return true; // Successfully used mana
        }
        return false; // Not enough mana
    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            if (currentMana < maxMana)
            {
                SetMana(currentMana + (manaRegenerationRate * Time.deltaTime)); // Regenerate mana over time
            }
            yield return null; 
        }
    }
}
