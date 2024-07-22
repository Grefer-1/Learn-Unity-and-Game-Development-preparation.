using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2048_TileCell : MonoBehaviour
{
    public Vector2Int Coordinates { get; set; }
    public Game2048_Tile Tile { get; set; }
    public bool IsEmpty => Tile == null ? true : false;
}
