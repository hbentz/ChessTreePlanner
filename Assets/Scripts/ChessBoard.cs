using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChessBoard : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private GameObject _tilePrefab;  // what to instantiate the tiles from

    [SerializeField] private Canvas arrowCanvas;
    [SerializeField] private GameObject BoardHighlight;  // Background for the board
    [SerializeField] private Material _boardDefault;
    [SerializeField] private Material _boardHighlight;

    [SerializeField] private TMP_Text _underText;

    // Which player is allowed to make a move on this board, starting with White.
    public int Turn = 0;

    // Update the text field whenever the turn changes
    public bool IsBlacksTurn
    {
        get => _isBlacksTurn;
        set
        {
            _underText.text = (value ? "Black" : "White") + " To Move";
            _isBlacksTurn = value;
        }
    }
    private bool _isBlacksTurn = false;

    // Allows access to all the tiles
    public ChessTile[,] Tiles = new ChessTile[8, 8];
    public List<ChessFigure> ActiveFigures = new List<ChessFigure>();  //  Useful for keeping track all of the pieces on this board

    public ChessFigure BlackKing;
    public ChessFigure WhiteKing;
    
    public ChessTile WhiteEnPassantTile;
    public ChessTile BlackEnPassantTile;

    // Controls BoardHighlightState
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            BoardHighlight.GetComponent<MeshRenderer>().material = _selected ? _boardHighlight : _boardDefault;
        }
    }
    private bool _selected = false;
    private List<GameObject> _threatArrows = new List<GameObject>();

    private IPieceGetter _pieceGetter;
    void Awake()
    {
        // Get the piece getter on awake
        _pieceGetter = GameObject.FindGameObjectWithTag("GameController").GetComponent<IPieceGetter>();

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

    public void SpawnChessFigure(Type chessPiece, bool isBlack, int x, int y)
    {
        // Instantiate the piece, place it, and track it
        GameObject unityFigure = Instantiate(_pieceGetter.PiecePrefab(chessPiece, isBlack));
        ChessFigure figure = unityFigure.GetComponent<ChessFigure>();

        // Move the piece to the tile and doubly link them
        figure.Tile = Tiles[x, y];
        Tiles[x, y].Figure = figure;
        figure.SetPosition(Tiles[x, y]);
        figure.HasMoved = false;  // Reset the HasMoved indicator

        // Track the piece in the active figures
        ActiveFigures.Add(figure);

        // Track the kings
        if (chessPiece == typeof(King))
        {
            if (isBlack) BlackKing = figure;
            else WhiteKing = figure;
        }
    }

    public void SpawnAll()
    {
        // Spawns all pieces on the starting board
        SpawnChessFigure(typeof(Rook), false, 0, 0); // Ra1
        SpawnChessFigure(typeof(Knight), false, 1, 0); // Nb1
        SpawnChessFigure(typeof(Bishop), false, 2, 0); // Bc1
        SpawnChessFigure(typeof(Queen), false, 3, 0); // Qd1
        SpawnChessFigure(typeof(King), false, 4, 0); // Ke1
        SpawnChessFigure(typeof(Bishop), false, 5, 0); // Bf1
        SpawnChessFigure(typeof(Knight), false, 6, 0); // Ng1
        SpawnChessFigure(typeof(Rook), false, 7, 0); // Rh1

        for (int i = 0; i < 8; i++) SpawnChessFigure(typeof(Pawn), false, i, 1); // Pawns on 2nd rank

        SpawnChessFigure(typeof(Rook), true, 0, 7); // Ra8
        SpawnChessFigure(typeof(Knight), true, 1, 7); // Nb8
        SpawnChessFigure(typeof(Bishop), true, 2, 7); // Bc8
        SpawnChessFigure(typeof(Queen), true, 3, 7); // Qd8
        SpawnChessFigure(typeof(King), true, 4, 7); // Ke8
        SpawnChessFigure(typeof(Bishop), true, 5, 7); // Bf8
        SpawnChessFigure(typeof(Knight), true, 6, 7); // Ng8
        SpawnChessFigure(typeof(Rook), true, 7, 7); // Rh8

        for (int i = 0; i < 8; i++) SpawnChessFigure(typeof(Pawn), true, i, 6); // Pawns on 7th rank
    }

    public void DrawThreatArrowsToTile(ChessTile tile)
    {
        ClearThreatArrows(); // Clean any existing threat arrows
        List<ChessFigure> threats = tile.ThreatenedBy(IsBlacksTurn);

        int x = tile.xCoord;
        int y = tile.yCoord;
        int startX, startY;
        float fillPercent, angle;

        // Rotation of arrow is CW -z
        // Zero point of arrow is 
        foreach(ChessFigure threat in threats)
        {
            GameObject newArrow = Instantiate(_arrowPrefab, arrowCanvas.transform);
            _threatArrows.Add(newArrow);

            startX = threat.Tile.xCoord;
            startY = threat.Tile.yCoord;

            angle = Vector2.Angle(Vector2.right, new Vector2(x - startX, y - startY));
            fillPercent = Mathf.Sqrt(Mathf.Pow(startX - x, 2) + Mathf.Pow(startY - y, 2)) / 10f;

            // Rotate and position the arrow starting at the correct location
            RectTransform newArrowRect = newArrow.GetComponent<RectTransform>();
            newArrowRect.localRotation = Quaternion.Euler(new Vector3(0, 0, -angle));
            newArrowRect.pivot = new Vector2(1f - fillPercent, 0.5f);
            newArrowRect.anchoredPosition = new Vector2(startX + 1, startY + 1);
            
            // Fill it the correct 
            newArrow.GetComponent<Image>().fillAmount = fillPercent;
        }
    }

    public void ClearThreatArrows()
    {
        foreach (GameObject arrow in _threatArrows) Destroy(arrow);
        _threatArrows = new List<GameObject>();
    }

    public bool PlayerInCheck(bool playerIsBlack) => (playerIsBlack? BlackKing : WhiteKing).Tile.ThreatenedBy(playerIsBlack).Count > 0;

    public bool MoveCreatesSelfCheck(ChessFigure piece, ChessTile tile, bool playerIsBlack)
    {
        // Record the the relevant pieces before the move
        ChessFigure pieceOnToTile = tile.Figure;
        ChessTile fromTile = piece.Tile;

        // Mock the move
        piece.Tile = tile;
        tile.Figure = piece;
        fromTile.Figure = null;
        if (pieceOnToTile != null)
        {
            pieceOnToTile.Tile = null;
            ActiveFigures.Remove(pieceOnToTile);
        }

        // Look for Check
        bool inCheck = PlayerInCheck(playerIsBlack);

        // Undo the move
        piece.Tile = fromTile;
        tile.Figure = pieceOnToTile;
        fromTile.Figure = piece;
        if (pieceOnToTile != null)
        {
            pieceOnToTile.Tile = tile;
            ActiveFigures.Add(pieceOnToTile);
        }

        // Return the finding
        return inCheck;
    }

    public bool MoveCreatesSelfCheck(ChessFigure piece, int x, int y, bool playerIsBlack) => MoveCreatesSelfCheck(piece, Tiles[x, y], playerIsBlack);

}
