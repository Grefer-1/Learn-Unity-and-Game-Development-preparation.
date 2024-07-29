using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tetris_Ghost : MonoBehaviour
{
    public Tile Tile;
    public Tetris_Board Board;
    public Tetris_Piece TrackingPiece;

    public Tilemap Tilemap { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public Vector3Int Position { get; private set; }

    void Awake()
    {
        Tilemap = GetComponentInChildren<Tilemap>();
        Cells = new Vector3Int[4];
    }

    void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Vector3Int tilePosition = Cells[i] + Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < Cells.Length; i++)
            Cells[i] = TrackingPiece.Cells[i];
    }

    private void Drop()
    {
        Vector3Int position = TrackingPiece.Position;
        int current = position.y;
        int bottom = -Board.BoardSize.y / 2 - 1;

        Board.Clear(TrackingPiece);

        for (int row = current; row >= bottom; row--)
        {
            position.y = row;
            if (Board.IsPositionValid(TrackingPiece, position))
                Position = position;
            else
                break;
        }

        Board.Set(TrackingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Vector3Int tilePosition = Cells[i] + Position;
            Tilemap.SetTile(tilePosition, Tile);
        }
    }
}
