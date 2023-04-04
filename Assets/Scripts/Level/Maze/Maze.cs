using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeDataNewGenerator
{
    int width;
    int height;

    int[,] maze;

    public int[,] GenerateMaze(int width, int height, int seed = 0)
    {
        this.width = width;
        this.height = height;

        // инициализируем массив лабиринта
        maze = new int[width, height];

        // заполняем массив стенами
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        int startX = width / 2;
        int startY = height / 2;

        GenerateMazeRecursive(startX, startY, seed);

        return maze;
    }

    // рекурсивная функция для генерации лабиринта
    private void GenerateMazeRecursive(int x, int y, int seed = 0)
    {
        // помечаем текущую клетку как посещенную
        maze[x, y] = 0;

        // создаем список соседних клеток
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

        // перемешиваем список соседних клеток
        neighbors = Shuffle(neighbors, seed == 0 ? 0 : seed);

        // рекурсивно вызываем функцию для каждой соседней клетки
        foreach (int[] neighbor in neighbors)
        {
            int neighborX = neighbor[0];
            int neighborY = neighbor[1];
            if (maze[neighborX, neighborY] == 1)
            {
                // создаем проход между текущей и соседней клетками
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

    public List<T> Shuffle<T>(List<T> list, int seed = 0)
    {
        if (seed != 0) Random.InitState(seed);

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}

public class MazeDataGenerator
{
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols, int seed = 0)
    {
        if (seed != 0)
            Random.InitState(seed);
        int[,] maze = new int[sizeRows, sizeCols];
        // stub to fill in

        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                //1
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }

                //2
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        //3
                        maze[i, j] = 1;

                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }

        return maze;
    }

} 

public class Maze
{
    private MazeDataNewGenerator dataGenerator;

    public Maze()
    {
        dataGenerator = new MazeDataNewGenerator();
    }

    public int[,] data
    {
        get; private set;
    }

    /**
     * Pass for Maze.
     */

    public void SetPass(int x, int y)
    {
        data[x, y] = 0;
    }

    /**
     * Ganarate new random maze.
     */

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        data = dataGenerator.GenerateMaze(sizeRows, sizeCols);
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols, int seed)
    {
        data = dataGenerator.GenerateMaze(sizeRows, sizeCols, seed);
    }

    /**
     * Set new maze data.
     */

    public void SetNewData(int[,] data)
    {
        this.data = data;
    }

    /**
     * Ganarate new random maze and Get it.
     */

    public int[,] GetNewMaze(int sizeRows, int sizeCols)
    {
        data = dataGenerator.GenerateMaze(sizeRows, sizeCols);
        return data;
    }
}
