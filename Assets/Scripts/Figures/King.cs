using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessFigure
{
    // Start is called before the first frame update

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        int currX = Tile.xCoord;
        int currY = Tile.yCoord;
        ChessTile[,] tiles = Tile.Board.Tiles;

        // Castle legality check
        if (!HasMoved)  // If the king hasn't moved
        {
            int castleY = isBlack ? 7 : 0; // Figure out which rank the castle happens on

            // If the queenside rook also hasn't moved, the spaces are empty, and the king is not under threat while travelling
            bool canQueensideCastle = tiles[0, castleY].HasFriendlyPiece(isBlack) && !tiles[0, castleY].Figure.HasMoved;
            for (int i = 1; i < 4; i++) canQueensideCastle = canQueensideCastle && !tiles[i, castleY].HasPiece();
            for (int i = 2; i <= 4; i++) canQueensideCastle = canQueensideCastle && tiles[i, castleY].ThreatenedBy(isBlack).Count == 0;
            moves[2, castleY] = canQueensideCastle;

            // If the kingside rook also hasn't moved, the spaces are empty, and the king is not under threat while travelling
            bool canKingsideCastle = tiles[7, castleY].HasFriendlyPiece(isBlack) && !tiles[7, castleY].Figure.HasMoved;
            for (int i = 5; i < 7; i++) canKingsideCastle = canKingsideCastle && !tiles[i, castleY].HasPiece();
            for (int i = 4; i < 7; i++) canKingsideCastle = canKingsideCastle && tiles[i, castleY].ThreatenedBy(isBlack).Count == 0;
            moves[6, castleY] = canKingsideCastle;
        }

        AddMoveIfLegal(currX + 1, currY, ref tiles, ref moves);
        AddMoveIfLegal(currX - 1, currY, ref tiles, ref moves);
        AddMoveIfLegal(currX + 1, currY + 1, ref tiles, ref moves);
        AddMoveIfLegal(currX + 1, currY - 1, ref tiles, ref moves);
        AddMoveIfLegal(currX - 1, currY + 1, ref tiles, ref moves);
        AddMoveIfLegal(currX - 1, currY - 1, ref tiles, ref moves);
        AddMoveIfLegal(currX, currY + 1, ref tiles, ref moves);
        AddMoveIfLegal(currX, currY - 1, ref tiles, ref moves);

        return moves;
    }

    public void AddMoveIfLegal(int x, int y, ref ChessTile[,] tiles, ref bool[,] moveArray)
    {
        if (MoveOnBoard(x, y))
        {
            ChessTile toTile = tiles[x, y];
            bool noThreatAndEmpty = toTile.ThreatenedBy(isBlack).Count == 0 && !toTile.HasPiece();
            bool canTake = toTile.HasEnemyPiece(isBlack) && toTile.Figure.ProtectedBy().Count == 0;
            moveArray[x, y] = noThreatAndEmpty || canTake;
        }
    }

    public override bool[,] PossibleAttacks()
    {
        bool[,] moves = new bool[8, 8];
        int currX = Tile.xCoord;
        int currY = Tile.yCoord;
        ChessTile[,] tiles = Tile.Board.Tiles;

        AddMoveIfOnboardAndNotConflicting(currX + 1, currY, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 1, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 1, currY - 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY - 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX, currY - 1, ref tiles, ref moves);

        return moves;
    }

    public override void SetPosition(ChessTile tile)
    {
        if (!HasMoved) // If this is the first move
        {
            int castleY = isBlack ? 7 : 0;
            ChessTile[,] tiles = tile.Board.Tiles;

            // Move the Queenside rook or kingside rook if castling
            if (tile.xCoord == 2) tiles[0, castleY].Figure.SetPosition(tiles[3, castleY]);
            else if (tile.xCoord == 6) tiles[7, castleY].Figure.SetPosition(tiles[5, castleY]);
        }
        base.SetPosition(tile);
    }
}

