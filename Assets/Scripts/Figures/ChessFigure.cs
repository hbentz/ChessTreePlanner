using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessFigure : MonoBehaviour
{
    public ChessTile Tile;
    public bool isBlack;
    public bool HasMoved = false;
    public Vector3 Scale = new Vector3(8.5f, 8.5f, 8.5f);

    public int xCoord = -1;
    public int yCoord = -1;

    //public abstract string PGN_Note { get; }

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

        xCoord = tile.xCoord;
        yCoord = tile.yCoord;

        HasMoved = true;
    }

    public virtual bool[,] PossibleAttacks(BoardState state)
    {
        return PossibleMoves(state);
    }

    public bool[,] LegalMoves(BoardState state)
    {

        // A piece can't move if it's not the players turn!
        if (isBlack == Tile.Board.IsBlacksTurn)
        {
            bool[,] moves = PossibleMoves(state);

            // Filter out all of the moves that create self-check
            // It's a lot more efficient than it looks because MoveCreatesSelfCheck only executes if moves[x, y] is a possible move
            ChessBoard board = Tile.Board;
            for (int x = 0; x < 8; x++) for (int y = 0; y < 8; y++) moves[x, y] = moves[x, y] && state.MoveCreatesCheck(xCoord, yCoord, x, y);

            return moves;
        }

        return new bool[8, 8];
    }

    // Must return a 8x8 array, with true entries where the piece can be legally moved
    public virtual bool[,] PossibleMoves(BoardState state)
    {
        return new bool[8, 8];
    }

    public void AddMoveIfOnboardAndNoCollision(int x, int y, BoardState state, ref bool[,] moves)
    {
        if (state.TileOnBoard(x, y)) moves[x, y] = !state.HasFriendlyPiece(x, y, isBlack);
    }

    private void OnDestroy()
    {
        // Remove this from the active figures and the tile reference to this
        Tile.Board.ActiveFigures.Remove(this);
    }
}