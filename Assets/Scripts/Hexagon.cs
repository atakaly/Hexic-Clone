using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{

    public Vector2 Coordinate { get; private set; }
    public Vector2 offset;

    public Color color { get; private set; }
    public int ColorIndex { get; set; }

    private List<Vector2> cornerPositions = new List<Vector2>();

    public List<Corner> Corners { get; private set; } = new List<Corner>() { Corner.BottomLeft, Corner.Left, Corner.TopLeft, Corner.TopRight, Corner.Right, Corner.TopRight };

    private void Start()
    {
        InitializeCornerPositions();
    }

    public void SetCoordinate(Vector2 _coordinate)
    {
        Coordinate = _coordinate;
    }

    private void InitializeCornerPositions()
    {
        cornerPositions.Add(new Vector2(-0.5f, -Mathf.Sqrt(3)) * 0.1f); // left bottom
        cornerPositions.Add(new Vector2(-1, 0) * 0.1f); // left
        cornerPositions.Add(new Vector2(-0.5f, 2 * Mathf.Sqrt(3)) * 0.1f); // left-top
        cornerPositions.Add(new Vector2(0.5f, 2 * Mathf.Sqrt(3)) * 0.1f); // right up
        cornerPositions.Add(new Vector2(1, 0) * 0.1f); // right
        cornerPositions.Add(new Vector2(0.5f, -Mathf.Sqrt(3)) * 0.1f); //right bottom
    }

    public Corner GetSelectableCorner(Corner corner)
    {

        if (Coordinate.x == 0)
        {
            if (Coordinate.y == 0)
            {
                if (corner == Corner.TopLeft) corner = Corner.TopRight;
                else if (corner == Corner.Left) corner = Corner.TopRight;
                else if (corner == Corner.BottomLeft) corner = Corner.TopRight;
                else if (corner == Corner.BottomRight) corner = Corner.TopRight;
                else if (corner == Corner.Right) corner = Corner.TopRight;
            }
            else if (Coordinate.y == Grid.instance.columns - 1)
            {
                if (corner == Corner.BottomLeft) corner = Corner.BottomRight;
                else if (corner == Corner.Left) corner = Corner.Right;
                else if (corner == Corner.TopLeft) corner = Corner.BottomRight;
                else if (corner == Corner.TopRight) corner = Corner.Right;
            }
            else
            {
                if (corner == Corner.Left) corner = Corner.Right;
                else if (corner == Corner.BottomLeft) corner = Corner.BottomRight;
                else if (corner == Corner.TopLeft) corner = Corner.TopRight;
            }
        }

        else if (Coordinate.x % 2 == 1)

        {
            if (Coordinate.x == Grid.instance.rows - 1)
            {
                if (Coordinate.y == 0)
                {
                    if (corner == Corner.Right) corner = Corner.TopLeft;
                    else if (corner == Corner.BottomRight) corner = Corner.TopLeft;
                    else if (corner == Corner.BottomLeft) corner = Corner.TopLeft;
                    else if (corner == Corner.TopLeft) corner = Corner.TopLeft;
                }
                else if (Coordinate.y == Grid.instance.columns - 1)
                {
                    if (corner == Corner.BottomRight) corner = Corner.BottomLeft;
                    else if (corner == Corner.Right) corner = Corner.BottomLeft;
                    else if (corner == Corner.TopRight) corner = Corner.BottomLeft;
                    else if (corner == Corner.TopLeft) corner = Corner.BottomLeft;
                    else if (corner == Corner.Left) corner = Corner.BottomLeft;
                }
                else
                {
                    if (corner == Corner.Right) corner = Corner.Left;
                    else if (corner == Corner.BottomRight) corner = Corner.BottomLeft;
                    else if (corner == Corner.TopRight) corner = Corner.TopLeft;
                }
            }
            else
            {
                if (Coordinate.y == 0)
                {
                    if (corner == Corner.BottomRight) corner = Corner.Right;
                    else if (corner == Corner.BottomLeft) corner = Corner.Left;
                }
                else if (Coordinate.y == Grid.instance.columns - 1)
                {
                    if (corner == Corner.Left) corner = Corner.BottomLeft;
                    else if (corner == Corner.TopLeft) corner = Corner.BottomLeft;
                    else if (corner == Corner.TopRight) corner = Corner.BottomRight;
                    else if (corner == Corner.Right) corner = Corner.BottomRight;
                }
            }
        }
        else if (Coordinate.x % 2 == 0)
        {
            if (Coordinate.x == Grid.instance.rows - 1)
            {
                if (Coordinate.y == 0)
                {
                    if (corner != Corner.TopLeft) corner = Corner.TopLeft;
                }
                else if (Coordinate.y == Grid.instance.columns - 1)
                {
                    if (corner == Corner.TopRight) corner = Corner.Left;
                    else if (corner == Corner.TopLeft) corner = Corner.Left;
                    else if (corner == Corner.Right) corner = Corner.BottomLeft;
                    else if (corner == Corner.BottomRight) corner = Corner.BottomLeft;
                }
                else
                {
                    if (corner == Corner.TopRight) corner = Corner.TopLeft;
                    else if (corner == Corner.Right) corner = Corner.Left;
                    else if (corner == Corner.BottomRight) corner = Corner.BottomLeft;
                }
            }
            else
            {
                if (Coordinate.y == 0)
                {
                    if (corner == Corner.Left) corner = Corner.TopLeft;
                    else if (corner == Corner.BottomLeft) corner = Corner.TopLeft;
                    else if (corner == Corner.BottomRight) corner = Corner.TopRight;
                    else if (corner == Corner.Right) corner = Corner.TopRight;
                }
                else if (Coordinate.y == Grid.instance.columns - 1)
                {
                    if (corner == Corner.TopLeft) corner = Corner.Left;
                    else if (corner == Corner.TopRight) corner = Corner.Right;

                }
            }
        }

        return corner;

    }

    public Corner GetCorner(Vector2 position)
    {
        List<float> differences = new List<float>();
        foreach (var cornerPosition in cornerPositions)
        {
            var difference = Vector2.Distance(position - (Vector2)transform.position, cornerPosition);
            differences.Add(difference);
        }
        Corner corner = (Corner)differences.IndexOf(differences.Min());

        return GetSelectableCorner(corner);
    }

    public void SetColor(Color color)
    {
        this.color = color;
        GetComponent<Image>().color = this.color;
    }

    public Corner GetRawCorner(Corner corner)
    {
        return Corners.Find(x => x == corner);
    }

}


public enum Corner
{
    BottomLeft,
    Left,
    TopLeft,
    TopRight,
    Right,
    BottomRight
}