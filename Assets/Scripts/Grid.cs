using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Grid : MonoSingleton<Grid>
{


    [SerializeField] public int rows;
    [SerializeField] public int columns;

    [SerializeField] public Sprite hexagonSelectionSprite;

    [SerializeField] private Color[] colors;

    public List<GridItem> grid = new List<GridItem>();

    public List<HexagonGroup> hexagonGroups = new List<HexagonGroup>();

    public HexagonGroup selectedHexagonGroup;

    [Header("Prefabs")]
    [SerializeField] private Hexagon hexagon;
    [SerializeField] private MiddlePoint midddlePointPrefab;



    

    void CreateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            var xPos = i % 2 == 0 ? 0 : 0.5f;
            for (int j = 0; j < rows; j++)
            {
                var position = new Vector2(i * this.hexagon.offset.x, (j + xPos) * this.hexagon.offset.y);
                var hexagon = Instantiate(this.hexagon, position, Quaternion.identity, transform);
                grid.Add(new GridItem(new Vector2(i, j), position, hexagon));
                hexagon.gameObject.name = "Hex: " + new Vector2(i, j);
                SetColorsRandomly(hexagon);
            }
        }


    }

    public void SelectHexagonGroup(PointerEventData eventData)
    {
        var selectedHexagon =  GetGridElement(eventData);

        var selectedCorner = selectedHexagon.GetCorner(Camera.main.ScreenToWorldPoint(eventData.position));

        selectedHexagonGroup = SetHexagonGroupAccordingToCorner(selectedHexagon, selectedCorner);

        selectedHexagonGroup.transform.SetParent(this.transform);

        if (selectedHexagonGroup != null) selectedHexagonGroup.ClearSelection();

    }


    public void FillEmptyGridItem()
    {
        foreach (var item in grid)
        {
            if (!item.isOccupied) {
                var position = item.Position;
                var hexagon = Instantiate(this.hexagon, position, Quaternion.identity, transform);
                SetColorsRandomly(hexagon);
                item.hexagon = hexagon;
                item.hexagon.SetCoordinate(item.hexagon.Coordinate);
                item.isOccupied = true;
            }
        }
    }


    public void RotateHexagonGroup(SwipeDirection swipeDirection)
    {
        var middlePosition = selectedHexagonGroup.GetMiddlePosition();

        selectedHexagonGroup.RotateGroup();
    }

    public Hexagon GetGridElement(PointerEventData eventData)
    {
        return eventData.pointerCurrentRaycast.gameObject.GetComponent<Hexagon>();
    }

    public Hexagon GetGridElementAtCoordinate(Vector2 selectedHexagonCoordinate)
    {
        var hexagon = grid.Find(a => a.Coordinate == selectedHexagonCoordinate).hexagon;
        return hexagon;
    }

    public void SetColorsRandomly(Hexagon hexagon)
    {
        int index = Random.Range(0, colors.Length);
        hexagon.ColorIndex = index;
        hexagon.SetColor(colors[index]);
    }

    public HexagonGroup SetHexagonGroupAccordingToCorner(Hexagon hexagon, Corner? corner)
    {
        GameObject hexagonGroupGameobject = new GameObject();
        var hexagonGroup = hexagonGroupGameobject.AddComponent(typeof(HexagonGroup)) as HexagonGroup;



        if (hexagon.Coordinate.x % 2 == 1)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, -1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, 0)));
                    break;
                case Corner.Left:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, 0)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, 1)));
                    break;
                case Corner.TopLeft:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, 1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, 1)));
                    break;
                case Corner.TopRight:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, 1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, 1)));
                    break;
                case Corner.Right:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, 1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, 0)));
                    break;
                case Corner.BottomRight:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, 0)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, -1)));
                    break;
                default:
                    break;
            }
        }
        else if (hexagon.Coordinate.x % 2 == 0)
        {
            switch (corner)
            {
                case Corner.BottomLeft:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, -1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, -1)));
                    break;
                case Corner.Left:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, 0)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, -1)));
                    break;
                case Corner.TopLeft:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(-1, 0)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, 1)));
                    break;
                case Corner.TopRight:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, 1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, 0)));
                    break;
                case Corner.Right:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, 0)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(1, -1)));
                    break;
                case Corner.BottomRight:
                    hexagonGroup.SetHexagonGroup(hexagon, GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(+1, -1)), GetGridElementAtCoordinate(hexagon.Coordinate + new Vector2(0, -1)));
                    break;
                default:
                    break;
            }
        }
        hexagonGroup.transform.localPosition = hexagonGroup.GetMiddlePosition();


        return hexagonGroup;
    }


    public void GetMatch(HexagonGroup hexagonGroup)
    {
        if (hexagonGroup.IsMatch) {
            Destroy(hexagonGroup.hex1);
            Destroy(hexagonGroup.hex2);
            Destroy(hexagonGroup.hex3);
        }
    }
    

    private void CreateHexagonGroups()
    {
        for (int i = 0; i < rows; i++)
        {

        }
    }


}
