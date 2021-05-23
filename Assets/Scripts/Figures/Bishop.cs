using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMoves(BoardState state)
    {
        bool[,] moves = new bool[8, 8];

        // Crawl through each of the four diagonals 
        // Stopping on any piece but noting a possible move if the unit is of a different color

        // Crawl up the top left diagonal
        int j;
        for (int i = xCoord - 1; i >= 0; i--)
        {
            j = yCoord + (xCoord - i);
            if (j >= state.BoardHeight) break;
            if (state.TileIsEmpty(i, j)) moves[i, j] = true;
            else
            {
                moves[i, j] = state.HasEnemyPiece(i, j, isBlack);
                break;
            }
        }

        // Crawl up the top right diagonal
        for (int i = xCoord + 1; i < state.BoardWidth; i++)
        {
            j = yCoord + (i - xCoord);
            if (j >= state.BoardHeight) break;
            if (state.TileIsEmpty(i, j)) moves[i, j] = true;
            else
            {
                moves[i, j] = state.HasEnemyPiece(i, j, isBlack);
                break;
            }
        }

        // Crawl down the bottom right diagonal
        for (int i = xCoord + 1; i < state.BoardWidth; i++)
        {
            j = yCoord - (i - xCoord);
            if (j < 0) break;
            if (state.TileIsEmpty(i, j)) moves[i, j] = true;
            else
            {
                moves[i, j] = state.HasEnemyPiece(i, j, isBlack);
                break;
            }
        }

        // Crawl down the bottom left diagonal
        for (int i = xCoord - 1; i >= 0; i--)
        {
            j = yCoord - (xCoord - i);
            if (j < 0) break;
            if (state.TileIsEmpty(i, j)) moves[i, j] = true;
            else
            {
                moves[i, j] = state.HasEnemyPiece(i, j, isBlack);
                break;
            }
        }

        return moves;
    }
}

