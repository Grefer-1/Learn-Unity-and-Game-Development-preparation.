using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2048_TileRow : MonoBehaviour
{
    public Game2048_TileCell[] Cells { get; private set; }

    void Awake()
    {
        Cells = GetComponentsInChildren<Game2048_TileCell>();
    }
}
