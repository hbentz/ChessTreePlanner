using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMoves(BoardState state)
    {
        bool[,] moves = new bool[8, 8];

        // Crawl through each of the four directions and then diagonals 
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

        // Crawl up the top left diagonal
        int k;
        for (int i = xCoord - 1; i >= 0; i--)
        {
            k = yCoord + (xCoord - i);
            if (k >= state.BoardHeight) break;
            if (state.TileIsEmpty(i, k)) moves[i, k] = true;
            else
            {
                moves[i, k] = state.HasEnemyPiece(i, k, isBlack);
                break;
            }
        }

        // Crawl up the top right diagonal
        for (int i = xCoord + 1; i < state.BoardWidth; i++)
        {
            k = yCoord + (i - xCoord);
            if (k >= state.BoardHeight) break;
            if (state.TileIsEmpty(i, k)) moves[i, k] = true;
            else
            {
                moves[i, k] = state.HasEnemyPiece(i, k, isBlack);
                break;
            }
        }

        // Crawl down the bottom right diagonal
        for (int i = xCoord + 1; i < state.BoardWidth; i++)
        {
            k = yCoord - (i - xCoord);
            if (k < 0) break;
            if (state.TileIsEmpty(i, k)) moves[i, k] = true;
            else
            {
                moves[i, k] = state.HasEnemyPiece(i, k, isBlack);
                break;
            }
        }

        // Crawl down the bottom left diagonal
        for (int i = xCoord - 1; i >= 0; i--)
        {
            k = yCoord - (xCoord - i);
            if (k < 0) break;
            if (state.TileIsEmpty(i, k)) moves[i, k] = true;
            else
            {
                moves[i, k] = state.HasEnemyPiece(i, k, isBlack);
                break;
            }
        }

        return moves;
    }
}
