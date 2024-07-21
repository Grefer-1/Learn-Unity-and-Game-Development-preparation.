using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TileBoard : MonoBehaviour
{
    public Tile TilePrefab;
    public TileState[] TileStates;
    public Game2048_GameManager GameManager;

    private TileGrid Grid;
    private List<Tile> Tiles;

    private bool IsMoveable;

    public void ClearBoard()
    {
        foreach (TileCell cell in Grid.Cells)
            cell.Tile = null;

        foreach (Tile tile in Tiles)
            Destroy(tile.gameObject);
        
        Tiles.Clear();
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(TilePrefab, Grid.transform);

        if (Random.value < 0.75f)
            tile.SetState(TileStates[0], 2);
        else
            tile.SetState(TileStates[1], 4);

        tile.Spawn(Grid.GetRandomEmptyCell());
        Tiles.Add(tile);
    }

    void Awake()
    {
        Grid = GetComponentInChildren<TileGrid>();
        Tiles = new List<Tile>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoveable)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, Grid.Height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, Grid.Width - 2, -1, 0, 1);
            }
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < Grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < Grid.Height; y += incrementY)
            {
                TileCell cell = Grid.GetCell(x, y);
                if (cell.IsEmpty)
                    changed |= MoveTile(cell.Tile, direction);
            }
        }

        if (changed)
            StartCoroutine(WaitTillMoveable());
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell nextCell = Grid.GetNextCell(tile.Cell, direction);

        while (nextCell != null)
        {
            if (!nextCell.IsEmpty)
            {
                if (Mergeable(tile, nextCell.Tile))
                {
                    Merge(tile, nextCell.Tile);
                    return true;
                }
                break;
            }

            newCell = nextCell;
            nextCell = Grid.GetNextCell(nextCell, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }

    private void Merge(Tile a, Tile b)
    {
        Tiles.Remove(a);
        a.Merge(b.Cell);

        int index = Mathf.Clamp(IndexOf(b.State) + 1, 0, TileStates.Length - 1);
        int value = b.Value * 2;

        b.SetState(TileStates[index], value);

        GameManager.Scored(value);
    }

    private bool Mergeable(Tile a, Tile b)
    {
        return a.Value == b.Value && !b.Locked;
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < TileStates.Length; i++)
            if (state == TileStates[i]) return i;
        return -1;
    }

    private IEnumerator WaitTillMoveable()
    {
        IsMoveable = false;
        yield return new WaitForSeconds(0.1f);
        IsMoveable = true;

        foreach (Tile tile in Tiles)
            tile.Unlock();

        if (Tiles.Count != Grid.Size)
            CreateTile();

        if (IsGameOver())
            GameManager.GameOver();
    }

    private bool IsGameOver()
    {
        if (Tiles.Count != Grid.Size) return false;

        foreach (Tile tile in Tiles)
        {
            TileCell up = Grid.GetNextCell(tile.Cell, Vector2Int.up);
            TileCell down = Grid.GetNextCell(tile.Cell, Vector2Int.down);
            TileCell left = Grid.GetNextCell(tile.Cell, Vector2Int.left);
            TileCell right = Grid.GetNextCell(tile.Cell, Vector2Int.right);

            if (up != null && Mergeable(tile, up.Tile)) return false;
            if (down != null && Mergeable(tile, down.Tile)) return false;
            if (left != null && Mergeable(tile, left.Tile)) return false;
            if (right != null && Mergeable(tile, right.Tile)) return false;
        }

        return true;
    }
}
