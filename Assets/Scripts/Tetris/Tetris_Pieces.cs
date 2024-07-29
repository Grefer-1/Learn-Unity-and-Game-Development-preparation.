using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetris_Pieces
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}

[Serializable]
public struct Tetris_PieceData
{
    public Tetris_Pieces Pieces;
    public Tile Tile;
    public Vector2Int[] Cells { get; private set; }
    public Vector2Int[,] WallKicks { get; private set; }

    public void Initialize()
    {
        Cells = Tetris_Data.Cells[Pieces];
        WallKicks = Tetris_Data.WallKicks[Pieces];
    }
}
