using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexagonGroup : MonoBehaviour
{
    public Hexagon hex1 { get; set; }
    public Hexagon hex2 { get; set; }
    public Hexagon hex3 { get; set; }

    public Sprite selectionSprite;

    public bool IsMatch { get { return hex1.ColorIndex == hex2.ColorIndex && hex2.ColorIndex == hex3.ColorIndex; } }


    public void SetHexagonGroup(Hexagon hex1, Hexagon hex2, Hexagon hex3)
    {
        this.hex1 = hex1;
        this.hex2 = hex2;
        this.hex3 = hex3;

        hex1.transform.SetParent(transform);
        hex2.transform.SetParent(transform);
        hex3.transform.SetParent(transform);

        var tempColor1 = hex1.color;
        tempColor1.a = .5f;
        hex1.SetColor(tempColor1);

        var tempColor2 = hex2.color;
        tempColor2.a = .5f;
        hex2.SetColor(tempColor2);

        var tempColor3 = hex3.color;
        tempColor3.a = .5f;
        hex3.SetColor(tempColor3);
    }


    public Vector2 GetMiddlePosition()
    {
        var middlePosition = (hex1.transform.position + hex2.transform.position + hex3.transform.position) / 3f;
        return middlePosition;
    }



    public void ClearSelection()
    {

        var tempColor1 = hex1.color;
        tempColor1.a = 1f;
        hex1.SetColor(tempColor1);

        var tempColor2 = hex2.color;
        tempColor2.a = 1f;
        hex2.SetColor(tempColor2);

        var tempColor3 = hex3.color;
        tempColor3.a = 1f;
        hex3.SetColor(tempColor3);
    }

    public void RotateGroup()
    {
        hex1.transform.position = hex2.transform.position;
        hex2.transform.position = hex3.transform.position;
        hex3.transform.position = hex1.transform.position;

        Grid.instance.selectedHexagonGroup.hex1 = hex2;
        Grid.instance.selectedHexagonGroup.hex2 = hex3;
        Grid.instance.selectedHexagonGroup.hex3 = hex1;
    }
} 