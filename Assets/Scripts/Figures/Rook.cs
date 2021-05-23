using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMoves(BoardState state)
    {
        bool[,] moves = new bool[8, 8];

        // Crawl through each of the four directions 
        // Stopping on any piece but noting a possible move if the unit is of a different color

        // Crawl left across the board
        for (int i = xCoord - 1; i >= 0; i--)
        {
            if (state.TileIsEmpty(i, yCoord)) moves[i, yCoord] = true;
            else
            {
                moves[i, yCoord] = state.HasEnemyPiece(i, yCoord, isBlack);
                break;
            }
        }

        // Crawl right across the board
        for (int i = xCoord + 1; i < 8; i++)
        {
            if (state.TileIsEmpty(i, yCoord)) moves[i, yCoord] = true;
            else
            {
                moves[i, yCoord] = state.HasEnemyPiece(i, yCoord, isBlack);
                break;
            }
        }

        // Crawl down the board
        for (int j = yCoord - 1; j >= 0; j--)
        {

            if (state.TileIsEmpty(xCoord, j)) moves[xCoord, j] = true;
            else
            {
                moves[xCoord, j] = state.HasEnemyPiece(xCoord, j, isBlack);
                break;
            }
        }

        // Crawl up the board
        for (int j = yCoord - 1; j >= 0; j--)
        {

            if (state.TileIsEmpty(xCoord, j)) moves[xCoord, j] = true;
            else
            {
                moves[xCoord, j] = state.HasEnemyPiece(xCoord, j, isBlack);
                break;
            }
        }

        return moves;
    }
}

