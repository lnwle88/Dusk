using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHoverEffect : MonoBehaviour
{
    private Text textComponent;

    public Color normalColor = new Color(0.7f, 0.7f, 0.7f);  // Bright grey
    public Color hoverColor = Color.white;  // White when hovered

    void Start()
    {
        textComponent = GetComponent<Text>();
        textComponent.color = normalColor;  // Set to bright grey initially
    }

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Entered");
        textComponent.color = hoverColor;  // Change to white
    }

    public void OnMouseExit()
    {
        Debug.Log("Mouse Exited");
        textComponent.color = normalColor;  // Revert to bright grey
    }
}