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
        if (y % 7 != 0 && tiles[x, y + offset].Figure == null)
        {
            possibleMoves[x, y + offset] = true;
            // Pawns can also double move on their first turn
            if (!HasMoved) possibleMoves[x, y + (offset * 2)] = tiles[x, y + (offset * 2)].Figure == null;
        }

        // Pawns can take units on the in-front diagonal if they are of opposite color
        if (y % 7 != 0 && x < 7) possibleMoves[x + 1, y + offset] = tiles[x + 1, y + offset].Figure != null && tiles[x + 1, y + offset].Figure.isBlack != isBlack;
        if (y % 7 != 0 && x > 0) possibleMoves[x - 1, y + offset] = tiles[x - 1, y + offset].Figure != null && tiles[x - 1, y + offset].Figure.isBlack != isBlack;

        // Pawns can take other pawns via EnPassant
        if (isBlack && Tile.Board.WhiteEnPassantTile != null)
        {
            bool isBeside = y == 3 && Mathf.Abs(Tile.Board.WhiteEnPassantTile.xCoord - Tile.xCoord) == 1;
            possibleMoves[Tile.Board.WhiteEnPassantTile.xCoord, Tile.Board.WhiteEnPassantTile.yCoord] = isBeside;
        }
        else if (Tile.Board.BlackEnPassantTile != null)
        {
            bool isBeside = y == 4 && Mathf.Abs(Tile.Board.BlackEnPassantTile.xCoord - Tile.xCoord) == 1;
            possibleMoves[Tile.Board.BlackEnPassantTile.xCoord, Tile.Board.BlackEnPassantTile.yCoord] = isBeside;
        }

        return possibleMoves;
    }

    public override bool[,] PossibleAttacks()
    {
        // Disclude all options to start
        bool[,] possibleMoves = new bool[8, 8];

        // Black pawns move towards y = 0, and white pawns to y = 7
        int offset = isBlack ? -1 : 1;
        int x = Tile.xCoord;
        int y = Tile.yCoord;
        ChessTile[,] tiles = Tile.Board.Tiles;

        if (x < 7 && y % 7 != 0) possibleMoves[x + 1, y + offset] = true;
        if (x > 0 && y % 7 != 0) possibleMoves[x - 1, y + offset] = true;

        // Pawns can take other pawns via EnPassant
        if (isBlack && Tile.Board.WhiteEnPassantTile != null)
        {
            bool isBeside = y == 3 && Mathf.Abs(Tile.Board.WhiteEnPassantTile.xCoord - Tile.xCoord) == 1;
            possibleMoves[Tile.Board.WhiteEnPassantTile.xCoord, Tile.Board.WhiteEnPassantTile.yCoord] = isBeside;
        }
        else if (Tile.Board.BlackEnPassantTile != null)
        {
            bool isBeside = y == 4 && Mathf.Abs(Tile.Board.BlackEnPassantTile.xCoord - Tile.xCoord) == 1;
            possibleMoves[Tile.Board.BlackEnPassantTile.xCoord, Tile.Board.BlackEnPassantTile.yCoord] = isBeside;
        }

        return possibleMoves;
    }

    // Special for pawn due to EnPassant logic and promotoion
    public override void SetPosition(ChessTile tile)
    {
        base.SetPosition(tile);

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
            tile.Board.WhiteEnPassantTile = tile.yCoord - Tile.yCoord == 2 ? tile.Board.Tiles[tile.xCoord, tile.yCoord - 1] : null;

            // If this was moving to an Enpassant tile delete the pawn on the correct tile
            if (tile.Board.BlackEnPassantTile != null && tile.Board.BlackEnPassantTile == tile)
            {
                Destroy(tile.Board.Tiles[tile.xCoord, tile.yCoord - 1].Figure.gameObject);
            }
        }

        // If the pawn has moved to the last rank open up the promotion window
        if (tile.yCoord % 7 == 0)
        {
            // This is ran before the two-way figure-tile coupling happens in the MovePiece() logic in GameController
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<GameController>().PromotePawn(this);
        }
    }
}

