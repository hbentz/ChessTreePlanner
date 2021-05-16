using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    public ChessFigure Figure;

    // Make this get-only so that it can't accidentally get attached to a new board
    public ChessBoard Board { get => _board; }
    private ChessBoard _board;

    [SerializeField] Material _darkTile, _lightTile, _darkHighlight, _lightHighlight; // TODO: Two more highlight colors for potential moves
    public bool Highlighted
    {
        get => _highlighted;
        set
        { 
            _highlighted = value;
            if (_highlighted) GetComponent<MeshRenderer>().material = (xCoord + yCoord) % 2 == 1 ? _darkHighlight : _lightHighlight;
            else GetComponent<MeshRenderer>().material = (xCoord + yCoord) % 2 == 1 ? _darkTile : _lightTile;
        }
    }
    private bool _highlighted = false;

    public int xCoord = -1;
    public int yCoord = -1;

    private void Awake()
    {
        // On initialization as GameController may reference this many times
        _board = this.GetComponentInParent<ChessBoard>();
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().material = (xCoord + yCoord) % 2 == 1 ? _darkTile : _lightTile;
    }

    private void OnMouseDown()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<GameController>().SelectTile(this);
    }

    private void OnMouseUp()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<GameController>().ReleaseOverTile(this);
    }

    private void OnMouseEnter()
    {
        // TODO highlight logic
    }

    public List<ChessFigure> ThreatenedBy()
    {
        List<ChessFigure> threats = new List<ChessFigure>();

        bool blacksTurn = _board.IsBlacksTurn;

        // Go through each of the active figures on the board
        foreach(ChessFigure figure in _board.ActiveFigures)
        {
            // If the figure is the opponent's and it can move here, it is considered a threat
            if (figure.isBlack != blacksTurn && figure.PossibleAttacks()[xCoord, yCoord]) threats.Add(figure);
        }

        return threats;
    }

    public bool HasFriendlyPiece(bool playerIsBlack) => HasPiece() && (Figure.isBlack == playerIsBlack);

    public bool HasEnemyPiece(bool playerIsBlack) => HasPiece() && (Figure.isBlack != playerIsBlack);

    public bool HasPiece() => Figure != null;
}
