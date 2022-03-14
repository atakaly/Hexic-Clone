using System;
using UnityEngine;

[Serializable]
public class GridItem
{
    public bool isOccupied;

    public Hexagon hexagon;

    public Vector2 Coordinate { get; set; }
    public Vector2 Position { get; set; }


    public GridItem(Vector2 coordinate, Vector2 position, Hexagon hexagon)
    {
        this.Coordinate = coordinate;
        this.Position = position;
        this.hexagon = hexagon;
    }

}
