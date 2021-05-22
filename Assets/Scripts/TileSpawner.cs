using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour, IGridSpawner
{
    [SerializeField] private GameObject _darkTilePrefab;
    [SerializeField] private GameObject _lightTilePrefab;

    public ChessTile[,] SpawnTiles(int rows, int columns)
    {
        if (rows > 26) throw new ArgumentOutOfRangeException($"{rows} is too many rows, the max is 26");
        ChessTile[,] tiles = new ChessTile[columns, rows];

        // Create all the tiles from a1 to h8
        for (int y = 0; y < rows; y++)
        {
            string tileRank = (y + 1).ToString();

            for (int x = 0; x < columns; x++)
            {
                // Spawn the tile
                GameObject newTile = Instantiate((x + y) % 2 == 1 ? _darkTilePrefab : _lightTilePrefab, this.transform);

                // Initialize the values and hold the tile in the array
                ChessTile tile = newTile.GetComponent<ChessTile>();
                tile.xCoord = x; tile.yCoord = y;
                tiles[x, y] = tile;

                // Position the tile and name it
                newTile.transform.localPosition = new Vector3(x, 0, y);
                string tileFile = Convert.ToChar(x + 97).ToString();
                newTile.name = tileFile + tileRank;
            }
        }

        return tiles;
    }
}
