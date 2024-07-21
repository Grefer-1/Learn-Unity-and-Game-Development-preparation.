using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int Coordinates { get; set; }
    public Tile Tile { get; set; }
    public bool IsEmpty => Tile == null ? true : false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
