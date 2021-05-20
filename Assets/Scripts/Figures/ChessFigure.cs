using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessFigure : MonoBehaviour
{
    public ChessTile Tile;
    public bool isBlack;
    public bool HasMoved = false;
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

        HasMoved = true;
    }

    public virtual bool[,] PossibleAttacks()
    {
        return PossibleMove();
    }

    public bool[,] LegalMoves()
    {

        // A piece can't move if it's not the players turn!
        if (isBlack == Tile.Board.IsBlacksTurn)
        {
            bool[,] moves = PossibleMove();

            // Filter out all of the moves that create self-check
            // It's a lot more efficient than it looks because MoveCreatesSelfCheck only executes if moves[x, y] is a possible move
            ChessBoard board = Tile.Board;
            for (int x = 0; x < 8; x++) for (int y = 0; y < 8; y++) moves[x, y] = moves[x, y] && !board.MoveCreatesSelfCheck(this, x, y, isBlack);

            return moves;
        }

        return new bool[8, 8];
    }

    // Must return a 8x8 array, with true entries where the piece can be legally moved
    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

    public bool MoveOnBoard(int x, int y) => (x >= 0 && x < 8 && y >= 0 && y < 8);

    public bool MoveDoesNotConflict(int x, int y, ref ChessTile[,] tiles) => !tiles[x, y].HasFriendlyPiece(isBlack);

    public void AddMoveIfOnboardAndNotConflicting(int x, int y, ref ChessTile[,] tiles, ref bool[,] moveArray)
    {
        if (MoveOnBoard(x, y)) moveArray[x, y] = MoveDoesNotConflict(x, y, ref tiles);
    }

    public List<ChessFigure> ProtectedBy()
    {
        // Change the piece's color to see if it's threatened by anything and then change it back
        isBlack = !isBlack;
        List<ChessFigure> piecesProtecting = Tile.ThreatenedBy(isBlack);
        isBlack = !isBlack;

        return piecesProtecting;
    }

    private void OnDestroy()
    {
        // Remove this from the active figures and the tile reference to this
        Tile.Board.ActiveFigures.Remove(this);
    }
}