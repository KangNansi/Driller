using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Level : MonoBehaviour
{
    private LevelCell[,] grid;

    private const int WIDTH = 9;
    private const int HEIGHT = 45;

    private Mesh currentMesh;

    private void Start()
    {
        Generate();
    }

    [Button]
    public void Generate()
    {
        grid = new LevelCell[WIDTH, HEIGHT];
        for(int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                grid[x, y] = new LevelCell(UnityEngine.Random.Range(0, 4));
            }
        }

        UpdateGrid();

        transform.position = new Vector3(-WIDTH / 2f, -HEIGHT);
    }

    public void UpdateGrid()
    {
#if UNITY_EDITOR
        if (currentMesh != null) DestroyImmediate(currentMesh);
#else
        if (currentMesh != null) Destroy(currentMesh);
#endif

        currentMesh = MakeMesh();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = currentMesh;
    }

    public void Dig(Vector2 position)
    {
        position = position - (Vector2)transform.position;

        Vector2Int basePos = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        if (!IsIn(basePos)) return;

        Queue<Vector2Int> gridCells = new Queue<Vector2Int>();
        gridCells.Enqueue(basePos);

        Queue<Vector2Int> changed =
            Parcourir(basePos,
                    pos => gridCells.Contains(pos) && Test(pos, (c) => c.type == grid[basePos.x, basePos.y].type),
                    (pos) => grid[pos.x, pos.y].type = -1);

        CheckFalling(changed);

        UpdateGrid();
    }

    private void CheckFalling(Queue<Vector2Int> toCheck)
    {
        List<Vector2Int> possiblyFalling = new List<Vector2Int>();

        foreach(var pos in toCheck)
        {
            Vector2Int possible = pos + Vector2Int.up;
            if(IsIn(possible) && !toCheck.Contains(possible) && grid[possible.x, possible.y].type >= 0)
            {
                possiblyFalling.Add(possible);
            }
        }

        List<Queue<Vector2Int>> blocks = new List<Queue<Vector2Int>>();

        foreach(var pos in possiblyFalling)
        {
            int type = grid[pos.x, pos.y].type;
            //Queue<Vector2Int> block = Parcourir(pos, pos => {

            //})
        }

        
    }

    private Queue<Vector2Int> Parcourir(Vector2Int start, Func<Vector2Int, bool> isValid, Action<Vector2Int> onCell)
    {
        Queue<Vector2Int> changed = new Queue<Vector2Int>();
        Queue<Vector2Int> gridCells = new Queue<Vector2Int>();

        while (gridCells.Count > 0)
        {
            Vector2Int pos = gridCells.Dequeue();
            changed.Enqueue(pos);
            int type = grid[pos.x, pos.y].type;
            if (type < 0) continue;

            Func<LevelCell, bool> predicate = c => c.type == type;

            // LEFT
            Vector2Int left = pos + Vector2Int.left;
            if (isValid(left))
            {
                gridCells.Enqueue(left);
            }

            // RIGHT
            Vector2Int right = pos + Vector2Int.right;
            if (isValid(right))
            {
                gridCells.Enqueue(right);
            }

            // UP
            Vector2Int up = pos + Vector2Int.up;
            if (isValid(up))
            {
                gridCells.Enqueue(up);
            }

            // DOWN
            Vector2Int down = pos + Vector2Int.down;
            if (isValid(down))
            {
                gridCells.Enqueue(down);
            }

            onCell?.Invoke(pos);
        }

        return changed;
    }

    private bool IsIn(Vector2Int pos)
    {
        return !(pos.x < 0 || pos.x >= WIDTH || pos.y < 0 || pos.y >= HEIGHT);
    }

    private bool Test(Vector2Int pos, Func<LevelCell, bool> predicate)
    {
        if(!IsIn(pos))
        {
            return false;
        }
        else
        {
            return predicate(grid[pos.x, pos.y]);
        }
    }

    public bool IsBlocking(Vector2 position)
    {
        Vector2Int pos = WorldToArray(position);

        return Test(pos, c => c.type >= 0);
        
    }

    private Vector2Int WorldToArray(Vector2 position)
    {
        position = position - (Vector2)transform.position;
        return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }

    private Mesh MakeMesh()
    {
        return MeshUtils.CreateGridMesh(WIDTH, HEIGHT,
            pos => grid[pos.x, pos.y].type < 0,
            pos => Color.HSVToRGB(grid[pos.x, pos.y].type / 4f, 1, 1));
    }
}
