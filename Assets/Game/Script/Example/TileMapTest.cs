using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTest : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;
    public RuleTile  ruleTile;

    private void Start()
    {
        
        tilemap.SetTile(Vector3Int.right,ruleTile);
        tilemap.SetTile(Vector3Int.zero,ruleTile);
    }
}
