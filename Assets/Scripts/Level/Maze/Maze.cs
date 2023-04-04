using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    int width;
    int height;

    int[,] maze;

    public int[,] GenerateMaze(int width, int height, int seed = 0)
    {
        this.width = width;
        this.height = height;

        maze = new int[this.width, this.height];

        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                maze[x, y] = 1;
            }
        }

        int startX = this.width / 2;
        int startY = this.height / 2;

        GenerateMazeRecursive(startX, startY, seed);

        return maze;
    }

    private void GenerateMazeRecursive(int x, int y, int seed = 0)
    {
        maze[x, y] = 0;

        List<int[]> neighbors = new List<int[]>();
        if (x > 1 && maze[x - 2, y] == 1)
        {
            neighbors.Add(new int[] { x - 2, y });
        }
        if (x < width - 2 && maze[x + 2, y] == 1)
        {
            neighbors.Add(new int[] { x + 2, y });
        }
        if (y > 1 && maze[x, y - 2] == 1)
        {
            neighbors.Add(new int[] { x, y - 2 });
        }
        if (y < height - 2 && maze[x, y + 2] == 1)
        {
            neighbors.Add(new int[] { x, y + 2 });
        }

        neighbors = Shuffle(neighbors, seed == 0 ? 0 : seed);

        foreach (int[] neighbor in neighbors)
        {
            int neighborX = neighbor[0];
            int neighborY = neighbor[1];
            if (maze[neighborX, neighborY] == 1)
            {
                maze[neighborX, neighborY] = 0;
                if (neighborX > x)
                {
                    maze[x + 1, y] = 0;
                }
                else if (neighborX < x)
                {
                    maze[x - 1, y] = 0;
                }
                else if (neighborY > y)
                {
                    maze[x, y + 1] = 0;
                }
                else if (neighborY < y)
                {
                    maze[x, y - 1] = 0;
                }
                GenerateMazeRecursive(neighborX, neighborY);
            }
        }
    }

    public List<int[]> Shuffle(List<int[]> list, int seed = 0)
    {
        if (seed != 0) Random.InitState(seed);

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int[] value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}

public class Maze
{
    private MazeDataGenerator dataGenerator;

    public Maze()
    {
        dataGenerator = new MazeDataGenerator();
    }

    public int[,] data
    {
        get; private set;
    }

    public void SetPass(int x, int y)
    {
        data[x, y] = 0;
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        data = dataGenerator.GenerateMaze(sizeRows, sizeCols);
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols, int seed)
    {
        data = dataGenerator.GenerateMaze(sizeRows, sizeCols, seed);
    }

    public void SetNewData(int[,] data)
    {
        this.data = data;
    }

    public int[,] GetNewMaze(int sizeRows, int sizeCols)
    {
        data = dataGenerator.GenerateMaze(sizeRows, sizeCols);
        return data;
    }
}
