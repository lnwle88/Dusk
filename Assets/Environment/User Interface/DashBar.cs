using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    //Dash
    public Slider DSlider;

   public void SetDash(int Dash)
    {
        DSlider.value = Dash;
    }
}
