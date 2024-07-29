using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tetris_Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public Tetris_Piece CurrentPiece { get; private set; }
    public Tetris_PieceData[] PieceDatas;
    public Vector3Int SpawnPosition;
    public Vector2Int BoardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-BoardSize.x / 2, -BoardSize.y / 2);
            return new RectInt(position, BoardSize);
        }
    }

    public void Set(Tetris_Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, piece.Data.Tile);
        }
    }

    public void Clear(Tetris_Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }

    public void SpawnPiece()
    {
        Tetris_PieceData data = PieceDatas[Random.Range(0, PieceDatas.Length)];
        CurrentPiece.Initialize(this, data, SpawnPosition);

        if (IsPositionValid(CurrentPiece, SpawnPosition))
            Set(CurrentPiece);
        else
            Gameover();
    }

    public bool IsPositionValid(Tetris_Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + position;
            if (!bounds.Contains((Vector2Int)tilePosition)) return false;
            if (Tilemap.HasTile(tilePosition)) return false;
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
                LineClear(row);
            else
                row++;
        }
    }

    void Awake()
    {
        Tilemap = GetComponentInChildren<Tilemap>();
        CurrentPiece = GetComponentInChildren<Tetris_Piece>();

        for (int i = 0; i < PieceDatas.Length; i++)
        {
            Tilemap = GetComponentInChildren<Tilemap>();
            PieceDatas[i].Initialize();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            if (!Tilemap.HasTile(new Vector3Int(col, row, 0)))
                return false;
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
            Tilemap.SetTile(new Vector3Int(col, row, 0), null);

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                TileBase above = Tilemap.GetTile(new Vector3Int(col, row + 1, 0));
                Tilemap.SetTile(new Vector3Int(col, row, 0), above);
            }
            row++;
        }
    }

    private void Gameover()
    {
        Tilemap.ClearAllTiles();
    }
}
