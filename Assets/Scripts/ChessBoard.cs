using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> _chessFigurePrefabs;  // Pieces in alphabetical order Black Bishop -> White Rook
    [SerializeField] private GameObject BoardHighlight;  // Shows if the board is active
    [SerializeField] private GameObject _tilePrefab;  // what to instantiate the tiles from


    // Which player is allowed to make a move on this board, starting with White.
    public int Turn = 0;
    public bool IsBlacksTurn = false;
    public bool InCheck = false;

    // Allows access to all the tiles
    public ChessTile[,] Tiles = new ChessTile[8, 8];
    private List<ChessFigure> _activeFigures = new List<ChessFigure>();  //  Useful for keeping track all of the pieces on this board

    // Controls BoardHighlightState
    public bool Selected
    {
        get => _selected;
        set { BoardHighlight.SetActive(value); _selected = value; }
    }
    private bool _selected = false;


    void Awake()
    {
        // Needs to be run in Awake because Start is delayed until after the spawning instance function finishes
        // Create all the tiles from a1 to h8
        for (int y = 0; y < 8; y++)
        {
            string tileRank = (y + 1).ToString();

            for (int x = 0; x < 8; x++)
            {
                // Spawn the tile
                GameObject newTile = Instantiate(_tilePrefab, this.transform);

                // Initialize the values and hold the tile in the array
                ChessTile tile = newTile.GetComponent<ChessTile>();
                tile.xCoord = x; tile.yCoord = y;
                Tiles[x, y] = tile;

                // Position the tile and name it
                newTile.transform.localPosition = new Vector3(x, 0, y);
                string tileFile = Convert.ToChar(x + 97).ToString();
                newTile.name = tileFile + tileRank;
            }
        }
    }

    public void SpawnChessFigure(int figureIndex, int x, int y)
    {
        // Instantiate the piece, place it, and track it
        GameObject unityFigure = Instantiate(_chessFigurePrefabs[figureIndex]);
        ChessFigure figure = unityFigure.GetComponent<ChessFigure>();
        figure.SetPosition(Tiles[x, y]);
        _activeFigures.Add(figure);
    }

    public void SpawnAll()
    {
        // Spawns all pieces on the starting board
        SpawnChessFigure(11, 0, 0); // Ra1
        SpawnChessFigure(8, 1, 0); // Nb1
        SpawnChessFigure(6, 2, 0); // Bc1
        SpawnChessFigure(10, 3, 0); // Qd1
        SpawnChessFigure(7, 4, 0); // Ke1
        SpawnChessFigure(6, 5, 0); // Bf1
        SpawnChessFigure(8, 6, 0); // Ng1
        SpawnChessFigure(11, 7, 0); // Rh1

        for (int i = 0; i < 8; i++) SpawnChessFigure(9, i, 1); // Pawns on 2nd rank

        SpawnChessFigure(5, 0, 7); // Ra8
        SpawnChessFigure(2, 1, 7); // Nb8
        SpawnChessFigure(0, 2, 7); // Bc8
        SpawnChessFigure(4, 3, 7); // Qd8
        SpawnChessFigure(1, 4, 7); // Kd8
        SpawnChessFigure(0, 5, 7); // Be8
        SpawnChessFigure(2, 6, 7); // Nf8
        SpawnChessFigure(5, 7, 7); // R88

        for (int i = 0; i < 8; i++) SpawnChessFigure(3, i, 6); // Pawns on 7th rank
    }
}
