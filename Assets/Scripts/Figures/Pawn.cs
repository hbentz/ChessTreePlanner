using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMove()
    {
        // Disclude all options to start
        bool[,] possibleMoves = new bool[8, 8];
        
        // Black pawns move towards y = 0, and white pawns to y = 7
        int offset = isBlack ? -1 : 1;
        int x = Tile.xCoord;
        int y = Tile.yCoord;
        ChessTile[,] tiles = Tile.Board.Tiles;

        // Generally shouldn't have to worry about out of bounds reference because pawns promote on the last file

        // Pawns can be blocked by units in front
        if (tiles[x, y + offset].Figure == null)
        {
            possibleMoves[x, y + offset] = true;
            // Pawns can double move on their first turn
            if ((y == 6 && isBlack) || (y == 1 && !isBlack)) possibleMoves[x, y + (offset * 2)] = tiles[x, y + (offset * 2)].Figure == null;
        }

        // Pawns can take units on the in-front diagonal if they are of opposite color
        if (x < 7) possibleMoves[x + 1, y + offset] = tiles[x + 1, y + offset].Figure != null && tiles[x + 1, y + offset].Figure.isBlack != isBlack;
        if (x > 0) possibleMoves[x - 1, y + offset] = tiles[x - 1, y + offset].Figure != null && tiles[x - 1, y + offset].Figure.isBlack != isBlack;

        // Pawns can take other pawns via EnPassant
        if (isBlack && Tile.Board.WhiteEnPassantTile != null)
        {
            possibleMoves[Tile.Board.WhiteEnPassantTile.xCoord, Tile.Board.WhiteEnPassantTile.yCoord] = true;
        }
        else if (Tile.Board.BlackEnPassantTile != null)
        {
            possibleMoves[Tile.Board.BlackEnPassantTile.xCoord, Tile.Board.BlackEnPassantTile.yCoord] = true;
        }

        return possibleMoves;
    }

    // Special for pawn due to EnPassant logic and promotoion
    public new void SetPosition(ChessTile tile)
    {
        
        // Reposition this in Unity
        this.transform.parent = tile.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Scale;

        // EnPassant Logic
        if (isBlack)
        {
            // If the pawn moved two tiles this enables EnPassant on the previous tile, otherwise it eliminates previous enpassant
            tile.Board.BlackEnPassantTile = Tile.yCoord - tile.yCoord == 2 ? tile.Board.Tiles[tile.xCoord, tile.yCoord + 1] : null;

            // If this was moving to an Enpassant tile delete the pawn on the correct tile
            if (tile.Board.WhiteEnPassantTile != null && tile.Board.WhiteEnPassantTile == tile)
            {
                Destroy(tile.Board.Tiles[tile.xCoord, tile.yCoord + 1].Figure.gameObject);
            }
        }
        else
        {
            // If the pawn moved two tiles this enables EnPassant on the previous tile, otherwise it eliminates previous enpassant
            tile.Board.WhiteEnPassantTile = tile.yCoord - Tile.yCoord == 1 ? tile.Board.Tiles[tile.xCoord, tile.yCoord - 1] : null;

            // If this was moving to an Enpassant tile delete the pawn on the correct tile
            if (tile.Board.BlackEnPassantTile != null && tile.Board.BlackEnPassantTile == tile)
            {
                Destroy(tile.Board.Tiles[tile.xCoord, tile.yCoord - 1].Figure.gameObject);
            }
        }

        // Promotion Logic
        if ((isBlack && tile.yCoord == 0) || (!isBlack && tile.yCoord == 7))
        {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<GameController>().PromotePawn(this);
        }
    }
}

