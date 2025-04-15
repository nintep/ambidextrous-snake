using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    private Color basePickUpColor;

    [SerializeField]
    private Color leftPickUpColor;

    [SerializeField]
    private Color rightPickUpColor;

    public PickUpType Type;

    private void Start()
    {
        UpdateColor();
    }

    public void SetType(PickUpType type)
    {
        Type = type;
        UpdateColor();        
    }

    private void UpdateColor()
    {
        SpriteRenderer rd = GetComponent<SpriteRenderer>();
        rd.color = Type == PickUpType.Food ? basePickUpColor 
                   : (Type == PickUpType.Food_left ? leftPickUpColor
                   : rightPickUpColor);
    }
}

public enum PickUpType
{
    Food,
    Food_left,
    Food_right
}
