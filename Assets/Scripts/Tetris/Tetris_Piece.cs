using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tetris_Piece : MonoBehaviour
{
    public Tetris_Board Board {  get; private set; }
    public Tetris_PieceData Data { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public Vector3Int Position { get; private set; }
    public int RotationIndex { get; private set; }

    public float StepDelay = 1f;
    public float LockDelay = 0.5f;

    private float StepTime;
    private float LockTime;

    public void Initialize(Tetris_Board board, Tetris_PieceData data, Vector3Int position)
    {
        Board = board;
        Data = data;
        Position = position;
        RotationIndex = Random.Range(0, 4);
        StepTime = Time.time + StepDelay;
        LockTime = 0f;

        if (Cells == null)
            Cells = new Vector3Int[data.Cells.Length];

        for (int i = 0; i < data.Cells.Length; i++)
        {
            Cells[i] = (Vector3Int)data.Cells[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Board.Clear(this);

        LockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
            Rotate(-1);
        else if (Input.GetKeyDown(KeyCode.E))
            Rotate(1);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector2Int.right);

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            Move(Vector2Int.down);

        if (Input.GetKeyDown(KeyCode.Space))
            HardDrop();

        if (Time.time > StepTime)
            Step();

        Board.Set(this);
    }

    private bool Move(Vector2Int direction)
    {
        Vector3Int newPosition = Position;
        newPosition.x += direction.x;
        newPosition.y += direction.y;

        bool isValid = Board.IsPositionValid(this, newPosition);

        if (isValid)
        {
            Position = newPosition;
            LockTime = 0f;
        }

        return isValid;
    }

    private void Step()
    {
        StepTime = Time.time + StepDelay;
        Move(Vector2Int.down);
        if (LockTime >= LockDelay)
            Lock();
    }

    private void HardDrop()
    {
        while(Move(Vector2Int.down))
            continue;

        Lock();
    }

    private void Lock()
    {
        Board.Set(this);
        Board.ClearLines();
        Board.SpawnPiece();
    }

    private void Rotate(int direction)
    {
        int originalRotation = RotationIndex;
        RotationIndex = Wrap(RotationIndex + direction, 0, 4);
        
        ApplyRotation(direction);

        if (!TestWallKicks(RotationIndex, direction))
        {
            RotationIndex = originalRotation;
            ApplyRotation(-direction);
        }
    }

    private void ApplyRotation(int direction)
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            int x, y;
            Vector3 cell = Cells[i];
            switch (Data.Pieces)
            {
                case Tetris_Pieces.I:
                case Tetris_Pieces.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Tetris_Data.RotationMatrix[0] * direction) + (cell.y * Tetris_Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Tetris_Data.RotationMatrix[2] * direction) + (cell.y * Tetris_Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Tetris_Data.RotationMatrix[0] * direction) + (cell.y * Tetris_Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Tetris_Data.RotationMatrix[2] * direction) + (cell.y * Tetris_Data.RotationMatrix[3] * direction));
                    break;
            }
            Cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < Data.WallKicks.GetLength(1); i++)
        {
            if (Move(Data.WallKicks[wallKickIndex, i]))
                return true;
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;
        if (rotationIndex < 0)
            wallKickIndex--;

        return Wrap(wallKickIndex, 0, Data.WallKicks.GetLength(0));
    }

    private int Wrap(int number, int min, int max)
    {
        if (number < min)
            return max - (min - number) % (max - min);
        else
            return min + (number - min) % (max - min);
    }
}
