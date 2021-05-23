using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChessBoard : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;

    [SerializeField] private Canvas arrowCanvas;

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
    private List<GameObject> _threatArrows = new List<GameObject>();

    private IGridSpawner _gridSpawner;
    public IHighlighter Highlighter;
    public BoardState State;

    private void Awake()
    {
        _gridSpawner = GetComponent<IGridSpawner>();
        Highlighter = GetComponentInChildren<IHighlighter>();
        Tiles = _gridSpawner.SpawnTiles(8, 8);
    }

    public void ActivateGame()
    {
        State = new BoardState(ActiveFigures, false);
    }

    public void DrawThreatArrowsToTile(ChessTile tile)
    {
        int x = tile.xCoord;
        int y = tile.yCoord;

        ClearThreatArrows(); // Clean any existing threat arrows
        List<ChessFigure> threats = IsBlacksTurn ? State.BlackPiecesAttacking(x, y) : State.WhitePiecesAttacking(x, y);

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

}
