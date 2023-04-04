using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MazeGenerator<T>
{
    private Grid<T> grid;

    public PathNode enter;
    public PathNode exit;

    private Maze maze;

    public List<PathNode> closedList = new List<PathNode>();
    public List<PathNode> openedList = new List<PathNode>();

    public MazeGenerator(Grid<T> grid)
    {
        this.grid = grid;

        maze = new Maze();
    }

    public void LoadSaveMap(string data)
    {
        enter = null;
        exit = null;

        CleanNodes(closedList);
        CleanNodes(openedList);

        closedList.Clear();
        openedList.Clear();

        maze.SetNewData(LoadMapData(data));
        GenerateWalls(maze);
    }

    public void GenerateMap()
    {
        maze.GenerateNewMaze(grid.GetWidth(), grid.GetHeight());
        GenerateLabyrinthWithPasses(maze);

    }

    public void GenerateNewMap(int seed = 0)
    {
        enter = null;
        exit = null;

        CleanNodes(closedList);
        CleanNodes(openedList);

        closedList.Clear();
        openedList.Clear();

        maze.GenerateNewMaze(grid.GetWidth(), grid.GetHeight(), seed);
        GenerateLabyrinthWithPasses(maze);
    }

    private void CleanNodes(List<PathNode> nodes)
    {
        foreach (var node in nodes)
            node.SetIsWalkable(false);
    }

    /**
     * Generate walls.
     */

    private void GenerateWalls(Maze mazeDataGenerator)
    {
        for (int x = 0; x < mazeDataGenerator.data.GetLength(0); x++)
        {
            for (int y = 0; y < mazeDataGenerator.data.GetLength(1); y++)
            {
                if (mazeDataGenerator.data[x, y] == 1)
                {
                    var node = grid.GetGridObject(x, y) as PathNode;
                    if (node != null)
                    {
                        node.isWalkable = false;
                        closedList.Add(node);
                    }
                    continue;
                }
                else if (mazeDataGenerator.data[x, y] == 0)
                {
                    var node = grid.GetGridObject(x, y) as PathNode;
                    if (node != null)
                    {
                        node.isWalkable = true;
                        openedList.Add(node);
                    }
                    continue;
                }
            }
        }
    }

    /**
     * Generate walls with passes.
     */

    private void GenerateLabyrinthWithPasses(Maze mazeDataGenerator)
    {
        for (int x = 0; x < mazeDataGenerator.data.GetLength(0); x++)
        {
            for (int y = 0; y < mazeDataGenerator.data.GetLength(1); y++)
            {
                if (mazeDataGenerator.data[x, y] == 1)
                {
                    var node = grid.GetGridObject(x, y) as PathNode;
                    if (node != null)
                    {
                        node.isWalkable = false;
                        closedList.Add(node);
                    }
                    continue;
                }
                else if (mazeDataGenerator.data[x, y] == 0)
                {
                    var node = grid.GetGridObject(x, y) as PathNode;
                    if (node != null)
                    {
                        node.isWalkable = true;
                        openedList.Add(node);
                    }
                    continue;
                }
            }
        }

        do
        {
            enter = GeneratePassEnter();
        } while (enter is null);
        enter.SetIsWalkable(true);

        do
        {
            exit = GeneratePassExit(enter);
        } while (exit is null);
        exit.SetIsWalkable(true);

        mazeDataGenerator.SetPass(enter.x, enter.y);
        mazeDataGenerator.SetPass(exit.x, exit.y);

        closedList.Remove(enter);
        closedList.Remove(exit);

        openedList.Add(enter);
        openedList.Add(exit);
    }

    public Vector2[] GetWalls()
    {
        Vector2[] walls = new Vector2[closedList.Count];
        for (int i = 0; i < closedList.Count; i++)
            walls[i] = grid.GetPosition2D(closedList[i].x, closedList[i].y);
        return walls;
    }

    public Vector2[] GetGround()
    {
        Vector2[] walls = new Vector2[openedList.Count];
        for (int i = 0; i < openedList.Count; i++)
            walls[i] = grid.GetPosition2D(openedList[i].x, openedList[i].y);
        return walls;
    }

    private PathNode GeneratePassEnter()
    {
        switch (UnityEngine.Random.Range(0, 4))
        {
            // Up
            case 0:
                for (int x = UnityEngine.Random.Range(1, grid.GetWidth()); x < grid.GetWidth(); x++)
                    if (IsPass(x, grid.GetHeight() - 1))
                        return grid.GetGridObject(x, grid.GetHeight() - 1) as PathNode;
                break;
            // Left
            case 1:
                for (int y = UnityEngine.Random.Range(1, grid.GetHeight()); y < grid.GetHeight(); y++)
                    if (IsPass(0, y))
                        return grid.GetGridObject(0, y) as PathNode;
                break;
            // Right
            case 2:
                for (int y = UnityEngine.Random.Range(1, grid.GetHeight()); y < grid.GetHeight(); y++)
                    if (IsPass(grid.GetWidth() - 1, y))
                        return grid.GetGridObject(grid.GetHeight() - 1, y) as PathNode;
                break;
            // Down
            case 3:
                for (int x = UnityEngine.Random.Range(1, grid.GetWidth()); x < grid.GetWidth(); x++)
                    if (IsPass(x, 0))
                        return grid.GetGridObject(x, 0) as PathNode;
                break;
        }
        return null;
    }

    private PathNode GeneratePassExit(PathNode enter)
    {
        switch (enter.edgeGrid)
        {
            // Up
            case PathNode.EdgeGridNodeType.Up:
                switch (UnityEngine.Random.Range(0, 3))
                {
                    // Left
                    case 0:
                        int yL = 0;
                        if (enter.x > grid.GetWidth() / 2) yL = UnityEngine.Random.Range(0, grid.GetHeight());
                        else yL = UnityEngine.Random.Range(0, grid.GetHeight() / 2);
                        for (; yL < grid.GetHeight(); yL++)
                            if (IsPass(0, yL))
                                return grid.GetGridObject(0, yL) as PathNode;
                        break;
                    // Right
                    case 1:
                        int yR = 0;
                        if (enter.x > grid.GetWidth() / 2) yR = UnityEngine.Random.Range(1, grid.GetHeight() / 2);
                        else yR = UnityEngine.Random.Range(1, grid.GetHeight());
                        for (; yR < grid.GetWidth(); yR++)
                            if (IsPass(grid.GetHeight() - 1, yR))
                                return grid.GetGridObject(grid.GetHeight() - 1, yR) as PathNode;
                        break;
                    // Down
                    case 2:
                        for (int x = UnityEngine.Random.Range(1, grid.GetWidth()); x < grid.GetWidth(); x++)
                            if (IsPass(x, 0))
                                return grid.GetGridObject(x, 0) as PathNode;
                        break;
                }
                break;
            // Left
            case PathNode.EdgeGridNodeType.Left:
                switch (UnityEngine.Random.Range(0, 3))
                {
                    // Up
                    case 0:
                        int xU = 0;
                        if (enter.y > grid.GetHeight() / 2) xU = UnityEngine.Random.Range(grid.GetWidth() / 2, grid.GetWidth());
                        else xU = UnityEngine.Random.Range(1, grid.GetWidth());
                        for (; xU < grid.GetWidth(); xU++)
                            if (IsPass(xU, grid.GetHeight() - 1))
                                return grid.GetGridObject(xU, grid.GetHeight() - 1) as PathNode;
                        break;
                    // Right
                    case 1:
                        for (int y = UnityEngine.Random.Range(1, grid.GetHeight()); y < grid.GetHeight(); y++)
                            if (IsPass(grid.GetWidth() - 1, y))
                                return grid.GetGridObject(grid.GetWidth() - 1, y) as PathNode;
                        break;
                    // Down
                    case 2:
                        int xD = 0;
                        if (enter.y < grid.GetHeight() / 2) xD = UnityEngine.Random.Range(grid.GetWidth() / 2, grid.GetWidth());
                        else xD = UnityEngine.Random.Range(1, grid.GetWidth());
                        for (; xD < grid.GetWidth(); xD++)
                            if (IsPass(xD, 0))
                                return grid.GetGridObject(xD, 0) as PathNode;
                        break;
                }
                break;
            // Right
            case PathNode.EdgeGridNodeType.Right:
                switch (UnityEngine.Random.Range(0, 3))
                {
                    // Up
                    case 0:
                        int xU = 0;
                        if (enter.y > grid.GetHeight() / 2) xU = UnityEngine.Random.Range(1, grid.GetHeight() / 2);
                        else xU = UnityEngine.Random.Range(1, grid.GetHeight());
                        for (int x = UnityEngine.Random.Range(1, grid.GetHeight()); x < grid.GetWidth(); x++)
                            if (IsPass(x, grid.GetHeight() - 1))
                                return grid.GetGridObject(x, grid.GetHeight() - 1) as PathNode;
                        break;
                    // Left
                    case 1:
                        for (int y = UnityEngine.Random.Range(0, grid.GetWidth()); y < grid.GetWidth(); y++)
                            if (IsPass(0, y))
                                return grid.GetGridObject(0, y) as PathNode;
                        break;
                    // Down
                    case 2:
                        int xD = 0;
                        if (enter.y < grid.GetHeight() / 2) xD = UnityEngine.Random.Range(1, grid.GetWidth() / 2);
                        else xD = UnityEngine.Random.Range(1, grid.GetWidth());
                        for (; xD < grid.GetWidth(); xD++)
                            if (IsPass(xD, 0))
                                return grid.GetGridObject(xD, 0) as PathNode;
                        break;
                }
                break;
            // Down
            case PathNode.EdgeGridNodeType.Down:
                switch (UnityEngine.Random.Range(0, 3))
                {
                    // Up
                    case 0:
                        for (int x = UnityEngine.Random.Range(1, grid.GetHeight()); x < grid.GetWidth(); x++)
                            if (IsPass(x, grid.GetHeight() - 1))
                                return grid.GetGridObject(x, grid.GetHeight() - 1) as PathNode;
                        break;
                    // Left
                    case 1:
                        int yL = 0;
                        if (enter.x < grid.GetWidth() / 2) yL = UnityEngine.Random.Range(grid.GetHeight() / 2, grid.GetWidth());
                        else yL = UnityEngine.Random.Range(1, grid.GetHeight());
                        for (; yL < grid.GetHeight(); yL++)
                            if (IsPass(0, yL))
                                return grid.GetGridObject(0, yL) as PathNode;
                        break;
                    // Right
                    case 2:
                        int yR = 0;
                        if (enter.x > grid.GetWidth() / 2) yR = UnityEngine.Random.Range(grid.GetHeight() / 2, grid.GetHeight());
                        else yR = UnityEngine.Random.Range(1, grid.GetHeight());
                        for (; yR < grid.GetHeight(); yR++)
                            if (IsPass(grid.GetWidth() - 1, yR))
                                return grid.GetGridObject(grid.GetHeight() - 1, yR) as PathNode;
                        break;
                }
                break;
        }
        return null;
    }

    private bool IsPass(int x, int y)
    {
        var node = grid.GetGridObject(x, y) as PathNode;
        if (node.edgeGrid == PathNode.EdgeGridNodeType.Down)
        {
            var neighbord = grid.GetGridObject(node.x, node.y + 1) as PathNode;
            if (neighbord.isWalkable) return true;
            else return false;
        }
        else if (node.edgeGrid == PathNode.EdgeGridNodeType.Up)
        {
            var neighbord = grid.GetGridObject(node.x, node.y - 1) as PathNode;
            if (neighbord.isWalkable) return true;
            else return false;
        }
        else if (node.edgeGrid == PathNode.EdgeGridNodeType.Left)
        {
            var neighbord = grid.GetGridObject(node.x + 1, node.y) as PathNode;
            if (neighbord.isWalkable) return true;
            else return false;
        }
        else if (node.edgeGrid == PathNode.EdgeGridNodeType.Right)
        {
            var neighbord = grid.GetGridObject(node.x - 1, node.y) as PathNode;
            if (neighbord.isWalkable) return true;
            else return false;
        }

        return false;
    }

    private int[,] LoadMapData(string data)
    {
        string[] rows = data.Split(' ');

        int[,] map = new int[rows.Length, rows[0].Length];

        for (int x = 0; x < rows.Length; x++)
        {
            for (int y = 0; y < rows[0].Length; y++)
            {
                int.TryParse(rows[x][y].ToString(), out map[x, y]);
            }
        }

        return map;
    }

    public int[,] GetCurrentLevel() => maze.data;

    public string GetCurrentLevelToString()
    {
        int row = maze.data.GetLength(0);
        int column = maze.data.GetLength(1);

        StringBuilder sb = new StringBuilder(maze.data.Length);
        for(int x = 0; x < row; x++)
        {
            if (x != 0) sb.Append(' ');
            for (int y = 0; y < column; y++)
            {
                sb.Append(maze.data[x, y]);
            }
        }
        return sb.ToString();
    }
}