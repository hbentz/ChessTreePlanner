using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        int currX = Tile.xCoord;
        int currY = Tile.yCoord;
        ChessTile[,] tiles = Tile.Board.Tiles;

        AddMoveIfOnboardAndNotConflicting(currX + 2, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 2, currY - 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 1, currY + 2, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY + 2, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 2, currY + 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 2, currY - 1, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX + 1, currY - 2, ref tiles, ref moves);
        AddMoveIfOnboardAndNotConflicting(currX - 1, currY - 2, ref tiles, ref moves);

        return moves;
    }

}
