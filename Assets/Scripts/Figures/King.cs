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

        AddMoveIfOnboardAndNotConflicting(currX + 1, currY, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 1, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 1, currY -1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY - 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX, currY - 1, ref tiles, ref moves);

        // TODO: Check for moves that put the king in danger
        // TODO: Castling

        return moves;
    }
}

