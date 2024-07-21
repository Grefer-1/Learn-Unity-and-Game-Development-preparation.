using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class TileGrid : MonoBehaviour
{
    public TileRow[] Rows { get; private set; }
    public TileCell[] Cells { get; private set; }

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size / Height;

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return Rows[y].Cells[x];
        return null;
    }

    public TileCell GetCell(Vector2Int coordinate) => GetCell(coordinate.x, coordinate.y);

    public TileCell GetNextCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.Coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, Cells.Length);
        int start = index;
        
        while (!Cells[index].IsEmpty)
        {
            index++;
            if (index >= Cells.Length)
                index = 0;

            if (index == start)
                return null;
        }

        return Cells[index];
    }

    void Awake()
    {
        Rows = GetComponentsInChildren<TileRow>();
        Cells = GetComponentsInChildren<TileCell>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Rows.Length; i++)
        {
            for (int j = 0; j < Rows[i].Cells.Length; j++)
                Rows[i].Cells[j].Coordinates = new Vector2Int(j, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
