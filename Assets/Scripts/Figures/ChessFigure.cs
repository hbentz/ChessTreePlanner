using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessFigure : MonoBehaviour
{
    public ChessTile Tile;
    public bool isBlack;
    public Vector3 Scale = new Vector3(8.5f, 8.5f, 8.5f);

    // Move this to a tile at coordinates x, y
    public void SetPosition(ChessBoard board, int x, int y)
    {
        SetPosition(board.Tiles[x, y]);
    }

    // Move this directly to a tile
    public virtual void SetPosition(ChessTile tile)
    {
        // Reposition this in Unity
        this.transform.parent = tile.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Scale;

        // Any movement from a player removes their EnPassant tile
        if (isBlack) tile.Board.BlackEnPassantTile = null;
        else tile.Board.WhiteEnPassantTile = null;
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
        // Remove this from the active figures and the tile reference to this
        Tile.Board.ActiveFigures.Remove(this);
    }
    public void AddMoveIfOnboardAndNotConflicting(int x, int y, ref ChessTile[,] tiles, ref bool[,] moveArray)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8) moveArray[x, y] = tiles[x, y].Figure == null || (tiles[x, y].Figure.isBlack != isBlack);
    }
}