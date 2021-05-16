using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessFigure
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

        // Crawl up the top left diagonal
        int j;
        for (int i = currX - 1; i >= 0; i--)
        {
            j = currY + (currX - i);
            if (j >= 8) break;
            if (tiles[i, j].Figure == null) moves[i, j] = true;
            else
            {
                if (isBlack != tiles[i, j].Figure.isBlack) moves[i, j] = true;
                break;
            }
        }

        // Crawl up the top right diagonal
        for (int i = currX + 1; i < 8; i++)
        {
            j = currY + (i - currX);
            if (j >= 8) break;
            if (tiles[i, j].Figure == null) moves[i, j] = true;
            else
            {
                if (isBlack != tiles[i, j].Figure.isBlack) moves[i, j] = true;
                break;
            }
        }

        // Crawl down the bottom right diagonal
        for (int i = currX + 1; i < 8; i++)
        {
            j = currY - (i - currX);
            if (j < 0) break;
            if (tiles[i, j].Figure == null) moves[i, j] = true;
            else
            {
                if (isBlack != tiles[i, j].Figure.isBlack) moves[i, j] = true;
                break;
            }
        }

        // Crawl down the bottom left diagonal
        for (int i = currX - 1; i >= 0; i--)
        {
            j = currY - (currX - i);
            if (j < 0) break;
            if (tiles[i, j].Figure == null) moves[i, j] = true;
            else
            {
                if (isBlack != tiles[i, j].Figure.isBlack) moves[i, j] = true;
                break;
            }
        }

        return moves;
    }
}

