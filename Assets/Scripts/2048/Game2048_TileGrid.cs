using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Game2048_TileGrid : MonoBehaviour
{
    public Game2048_TileRow[] Rows { get; private set; }
    public Game2048_TileCell[] Cells { get; private set; }

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size / Height;

    public Game2048_TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return Rows[y].Cells[x];
        return null;
    }

    public Game2048_TileCell GetCell(Vector2Int coordinate) => GetCell(coordinate.x, coordinate.y);

    public Game2048_TileCell GetNextCell(Game2048_TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.Coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public Game2048_TileCell GetRandomEmptyCell()
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
        Rows = GetComponentsInChildren<Game2048_TileRow>();
        Cells = GetComponentsInChildren<Game2048_TileCell>();
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
}
