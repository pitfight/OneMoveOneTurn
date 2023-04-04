using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelMenager : MonoBehaviour
{
    private const string LEVEL_SAVE = "LEVEL_SAVE"; 
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 10;
    private const float cellSize = 2f;
    [SerializeField] private LevelGenerator levelGenerator;

    [Header("Parent Text")]
    [SerializeField] private Transform parentText;

    private Pathfinding pathFinder;

    private Grid<PathNode> grid;
    private MazeGenerator<PathNode> mazeGenerator;

    private void Awake()
    {
        if (width % 2 == 0) width += 1;
        if (height % 2 == 0) height += 1;

        grid = new Grid<PathNode>(parentText, width, height, cellSize, Vector2.zero, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y));

        pathFinder = new Pathfinding(grid, Pathfinding.PathfindingType.Route4);

        mazeGenerator = new MazeGenerator<PathNode>(grid);
    }

    private void Start()
    {
        GanarateMap();
    }

    public void GanarateMap(string loadData = null, int seed = 0)
    {
        if (loadData == null)
        {
            Debug.Log("Ganarate new Map");

            mazeGenerator.GenerateNewMap(seed);

            pathFinder.SetRoutsList(mazeGenerator.openedList, mazeGenerator.closedList);
            Debug.Log("Enter: " + mazeGenerator.enter.ToString());
            Debug.Log("Exit: " + mazeGenerator.exit.ToString());

            levelGenerator.SpawnTiles(mazeGenerator.GetWalls(), LevelGenerator.TylesType.Wall);
            levelGenerator.SpawnTiles(mazeGenerator.GetGround(), LevelGenerator.TylesType.Ground);

            Debug.Log(mazeGenerator.GetCurrentLevelToString());
        }
        else
        {
            Debug.Log("Load Map");
            mazeGenerator.LoadSaveMap(loadData);

            levelGenerator.SpawnTiles(mazeGenerator.GetWalls(), LevelGenerator.TylesType.Wall);
            levelGenerator.SpawnTiles(mazeGenerator.GetGround(), LevelGenerator.TylesType.Ground);
        }
    }

    //void Update()
    //{
    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    Vector3 pos = UtilsClass.GetMouseWorldPosition();
    //    //    var obj = grid.GetGridObject(pos);
    //    //    if (obj != null && !obj.isOnClosedList)
    //    //        Debug.Log($"pos x: {obj.xPos}, pos y: {obj.yPos}");
    //    //}
    //}

    public string GetCurrentLevel() => mazeGenerator.GetCurrentLevelToString();
}
