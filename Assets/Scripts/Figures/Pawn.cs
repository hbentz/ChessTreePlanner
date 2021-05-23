using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMoves(BoardState state)
    {

        // Disclude all options to start
        bool[,] possibleMoves = new bool[8, 8];
        
        // Black pawns move towards y = 0, and white pawns to y = 7
        int moveDir = isBlack ? -1 : 1;

        // Generally shouldn't have to worry about out of bounds reference because pawns promote on the last file

        // If the pawn is not on the ends of the board and the tile infront is clear
        if (yCoord % 7 != 0 && state.TileIsEmpty(xCoord, yCoord + moveDir))
        {
            possibleMoves[xCoord, yCoord + moveDir] = true;

            // Pawns can also double move on their first turn
            if (!HasMoved) possibleMoves[xCoord, yCoord + (moveDir * 2)] = state.TileIsEmpty(xCoord, yCoord + (moveDir * 2));
        }

        // Pawns can take units on the in-front diagonal if they are of opposite color, they can also take the EnPassant Tile
        if (yCoord % 7 != 0 && xCoord < 7) possibleMoves[xCoord + 1, yCoord + moveDir] = state.HasEnemyPiece(xCoord + 1, yCoord + moveDir, isBlack) || state.EnPassantTile == (xCoord + 1, yCoord + moveDir);
        if (yCoord % 7 != 0 && xCoord > 0) possibleMoves[xCoord - 1, yCoord + moveDir] = state.HasEnemyPiece(xCoord - 1, yCoord + moveDir, isBlack) || state.EnPassantTile == (xCoord + 1, yCoord + moveDir);

        return possibleMoves;
    }

    public override bool[,] PossibleAttacks(BoardState state)
    {
        // Disclude all options to start
        bool[,] attacks = new bool[8, 8];

        // Black pawns move towards y = 0, and white pawns to y = 7
        int offset = isBlack ? -1 : 1;

        if (xCoord < 7 && yCoord % 7 != 0) attacks[xCoord + 1, yCoord + offset] = true;
        if (xCoord > 0 && yCoord % 7 != 0) attacks[xCoord - 1, yCoord + offset] = true;

        return attacks;
    }

    // Special for pawn due to EnPassant logic and promotoion
    public override void SetPosition(ChessTile tile)
    {
        base.SetPosition(tile);

        // If the pawn has moved to the last rank open up the promotion window
        if (tile.yCoord % 7 == 0)
        {
            // This is ran before the two-way figure-tile coupling happens in the MovePiece() logic in GameController
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<GameController>().PromotePawn(this, tile);
        }
    }
}

