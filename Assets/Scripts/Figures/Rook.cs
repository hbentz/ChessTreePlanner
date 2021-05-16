using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];

        int currX = Tile.xCoord;
        int currY = Tile.yCoord;

        ChessTile[,] tiles = Tile.Board.Tiles;

        // Crawl through each of the four diagonals 
        // Stopping on any piece but noting a possible move if the unit is of a different color

        // Crawl left across the board
        for (int i = currX - 1; i >= 0; i--)
        {
            if (tiles[i, currY].Figure == null) moves[i, currY] = true;
            else
            {
                if (isBlack != tiles[i, currY].Figure.isBlack) moves[i, currY] = true;
                break;
            }
        }

        // Crawl right across the board
        for (int i = currX + 1; i < 8; i++)
        {
            if (tiles[i, currY].Figure == null) moves[i, currY] = true;
            else
            {
                if (isBlack != tiles[i, currY].Figure.isBlack) moves[i, currY] = true;
                break;
            }
        }

        // Crawl down the board
        for (int j = currY - 1; j >= 0; j--)
        {
            if (tiles[currX, j].Figure == null) moves[currX, j] = true;
            else
            {
                if (isBlack != tiles[currX, j].Figure.isBlack) moves[currX, j] = true;
                break;
            }
        }

        // Crawl up the board
        for (int j = currY + 1; j < 8; j++)
        {
            if (tiles[currX, j].Figure == null) moves[currX, j] = true;
            else
            {
                if (isBlack != tiles[currX, j].Figure.isBlack) moves[currX, j] = true;
                break;
            }
        }

        return moves;
    }
}

