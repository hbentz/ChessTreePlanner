using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    public ChessFigure Figure;

    // Make this get-only so that it can't accidentally get attached to a new board
    public ChessBoard Board { get => _board; }
    private ChessBoard _board;

    public int xCoord = -1;
    public int yCoord = -1;

    private void Awake()
    {
        // On initialization as GameController may reference this many times
        _board = this.GetComponentInParent<ChessBoard>();
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

    public List<ChessFigure> ThreatenedBy(bool friendlyPlayerIsBlack)
    {
        List<ChessFigure> threats = new List<ChessFigure>();

        // Go through each of the active figures on the board
        foreach(ChessFigure figure in _board.ActiveFigures)
        {
            // If the figure is the opponent's and it can move here, it is considered a threat
            if ((figure.isBlack != friendlyPlayerIsBlack) && figure.PossibleAttacks()[xCoord, yCoord]) threats.Add(figure);
        }

        return threats;
    }

    public bool HasFriendlyPiece(bool playerIsBlack) => HasPiece() && (Figure.isBlack == playerIsBlack);

    public bool HasEnemyPiece(bool playerIsBlack) => HasPiece() && (Figure.isBlack != playerIsBlack);

    public bool HasPiece() => Figure != null;
}
