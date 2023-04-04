using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public enum TylesType
    {
        Wall,
        Ground
    }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile ruleTileWalls;
    [SerializeField] private Tile[] tilesGround;

    public void SpawnTiles(Vector2[] coordinates, TylesType tylesType, int seed = 0)
    {
        if (seed != 0) Random.InitState(seed);
        if (tylesType == TylesType.Wall)
        {
            foreach (var coor in coordinates)
            {
                var vec = new Vector3Int((int)coor.x, (int)coor.y, 0);
                //tileBase.ru
                tilemap.SetTile(vec, ruleTileWalls);
                vec = new Vector3Int((int)coor.x - 1, (int)coor.y - 1, 0);
                tilemap.SetTile(vec, ruleTileWalls);
                vec = new Vector3Int((int)coor.x - 1, (int)coor.y, 0);
                tilemap.SetTile(vec, ruleTileWalls);
                vec = new Vector3Int((int)coor.x, (int)coor.y - 1, 0);
                tilemap.SetTile(vec, ruleTileWalls);
            }
        }
        else if (tylesType == TylesType.Ground)
        {
            foreach (var coor in coordinates)
            {
                var vec = new Vector3Int((int)coor.x, (int)coor.y, 0);
                //tileBase.ru
                tilemap.SetTile(vec, tilesGround[Random.Range(0, tilesGround.Length)]);
                vec = new Vector3Int((int)coor.x - 1, (int)coor.y - 1, 0);
                tilemap.SetTile(vec, tilesGround[Random.Range(0, tilesGround.Length)]);
                vec = new Vector3Int((int)coor.x - 1, (int)coor.y, 0);
                tilemap.SetTile(vec, tilesGround[Random.Range(0, tilesGround.Length)]);
                vec = new Vector3Int((int)coor.x, (int)coor.y - 1, 0);
                tilemap.SetTile(vec, tilesGround[Random.Range(0, tilesGround.Length)]);
            }
        }
    }

    public void ClearTiles()
    {
        tilemap.ClearAllTiles();
    }
}
