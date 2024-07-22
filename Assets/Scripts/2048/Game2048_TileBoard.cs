using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Game2048_TileBoard : MonoBehaviour
{
    public Game2048_Tile TilePrefab;
    public Game2048_TileState[] TileStates;
    public Game2048_GameManager GameManager;

    private Game2048_TileGrid Grid;
    private List<Game2048_Tile> Tiles;

    private bool IsMoveable;

    public void ClearBoard()
    {
        foreach (Game2048_TileCell cell in Grid.Cells)
            cell.Tile = null;

        foreach (Game2048_Tile tile in Tiles)
            Destroy(tile.gameObject);
        
        Tiles.Clear();
    }

    public void CreateTile()
    {
        Game2048_Tile tile = Instantiate(TilePrefab, Grid.transform);

        if (Random.value < 0.75f)
            tile.SetState(TileStates[0], 2);
        else
            tile.SetState(TileStates[1], 4);

        tile.Spawn(Grid.GetRandomEmptyCell());
        Tiles.Add(tile);
    }

    void Awake()
    {
        Grid = GetComponentInChildren<Game2048_TileGrid>();
        Tiles = new List<Game2048_Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoveable)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                MoveTiles(Vector2Int.down, 0, 1, Grid.Height - 2, -1);
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                MoveTiles(Vector2Int.right, Grid.Width - 2, -1, 0, 1);
        }
    }

    private void OnEnable()
    {
        IsMoveable = true;
    }

    private void OnDisable()
    {
        IsMoveable = false;
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < Grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < Grid.Height; y += incrementY)
            {
                Game2048_TileCell cell = Grid.GetCell(x, y);
                if (!cell.IsEmpty)
                    changed |= MoveTile(cell.Tile, direction);
            }
        }

        if (changed)
            StartCoroutine(WaitTillMoveable());
    }

    private bool MoveTile(Game2048_Tile tile, Vector2Int direction)
    {
        Game2048_TileCell newCell = null;
        Game2048_TileCell nextCell = Grid.GetNextCell(tile.Cell, direction);

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

    private void Merge(Game2048_Tile a, Game2048_Tile b)
    {
        Tiles.Remove(a);
        a.Merge(b.Cell);

        int index = Mathf.Clamp(IndexOf(b.State) + 1, 0, TileStates.Length - 1);
        int value = b.Value * 2;

        b.SetState(TileStates[index], value);

        GameManager.Scored(value);
    }

    private bool Mergeable(Game2048_Tile a, Game2048_Tile b)
    {
        return a.Value == b.Value && !b.Locked;
    }

    private int IndexOf(Game2048_TileState state)
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

        foreach (Game2048_Tile tile in Tiles)
            tile.Unlock();

        if (Tiles.Count != Grid.Size)
            CreateTile();

        if (IsGameOver())
            GameManager.GameOver();
    }

    private bool IsGameOver()
    {
        if (Tiles.Count != Grid.Size) return false;

        foreach (Game2048_Tile tile in Tiles)
        {
            Game2048_TileCell up = Grid.GetNextCell(tile.Cell, Vector2Int.up);
            Game2048_TileCell down = Grid.GetNextCell(tile.Cell, Vector2Int.down);
            Game2048_TileCell left = Grid.GetNextCell(tile.Cell, Vector2Int.left);
            Game2048_TileCell right = Grid.GetNextCell(tile.Cell, Vector2Int.right);

            if (up != null && Mergeable(tile, up.Tile)) return false;
            if (down != null && Mergeable(tile, down.Tile)) return false;
            if (left != null && Mergeable(tile, left.Tile)) return false;
            if (right != null && Mergeable(tile, right.Tile)) return false;
        }

        return true;
    }
}
