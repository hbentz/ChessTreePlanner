using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessFigure : MonoBehaviour
{
    public ChessTile Tile;
    public bool isBlack;

    private Vector3 _scale = new Vector3(8.5f, 8.5f, 8.5f);

    // Move this to a tile at coordinates x, y
    public void SetPosition(ChessBoard board, int x, int y)
    {
        SetPosition(board.Tiles[x, y]);
    }

    // Move this directly to a tile
    public void SetPosition(ChessTile tile)
    {
        // Reposition this in Unity
        this.transform.parent = tile.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = _scale;
    }

    public bool[,] LegalMoves()
    {
        // If the pieceis pinned there is no legal moves 
        if (IsPinned()) return new bool[8, 8];
        return PossibleMove();
    }

    // Must return a 8x8 array, with true entries where the piece can be legally moved
    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

    public bool IsPinned()
    {
        // TODO: Detect if moving the figure creates a check
        return false;
    }

    private void OnDestroy()
    {
        // Remove this from the active figures
        Tile.Board.ActiveFigures.Remove(this);
    }
}