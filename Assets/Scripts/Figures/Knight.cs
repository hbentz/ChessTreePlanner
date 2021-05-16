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

        AddKnightMoveIfLegal(currX + 2, currY + 1, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX + 2, currY - 1, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX + 1, currY + 2, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX - 1, currY + 2, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX - 2, currY + 1, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX - 2, currY - 1, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX + 1, currY - 2, ref tiles, ref moves);
        AddKnightMoveIfLegal(currX - 1, currY - 2, ref tiles, ref moves);

        return moves;
    }

    private void AddKnightMoveIfLegal(int x, int y, ref ChessTile[,] tiles, ref bool[,] moveArray)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8) moveArray[x, y] = tiles[x, y].Figure == null || (tiles[x, y].Figure.isBlack != isBlack);
    }
}
