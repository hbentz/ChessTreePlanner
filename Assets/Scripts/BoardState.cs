using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    // Tracks and provides game releated information about piece inter-dependencies
    // Moving the Pieces inside the state (REFACTOR?)

    public ChessFigure[,] PieceLocations;
    public int BoardHeight, BoardWidth;
    public bool BlacksTurn;
    public int TurnNumber = 1;

    public (int, int) EnPassantTile = (-1, -1);

    public string PGN = "";

    private List<ChessFigure> _blacksPieces;
    private List<ChessFigure> _whitesPieces;

    private ChessFigure _blackKing;
    private ChessFigure _whiteking;

    public BoardState(IEnumerable<ChessFigure> figures, bool blacksTurn, int boardWidth = 8, int boardHeight = 8)
    {
        PieceLocations = new ChessFigure[boardHeight, boardWidth];
        BoardHeight = boardHeight;
        BoardWidth = boardWidth;

        _blacksPieces = new List<ChessFigure>();
        _whitesPieces = new List<ChessFigure>();

        BlacksTurn = blacksTurn;

        _blackKing = null;
        _whiteking = null;

        foreach (ChessFigure figure in figures)
        {
            PieceLocations[figure.xCoord, figure.yCoord] = figure;
            (figure.isBlack ? _blacksPieces : _whitesPieces).Add(figure);

            if (figure.GetType() == typeof(King))
            {
                if (figure.isBlack) _blackKing = figure;
                else _whiteking = figure;
            }
        }
    }

    public ChessFigure ExecuteMove(int fromX, int fromY, int toX, int toY)
    {
        // Record the captured Chessfigure if there is any
        ChessFigure capturedPiece = AnyPieceOn(toX, toY) ? PieceLocations[toX, toY] : null;
        ChessFigure movePiece = PieceLocations[fromX, fromY];

        // TODO: PGN Logic

        // Actually do the move
        MoveFigureTo(fromX, fromY, toX, toY);

        // EnPassant + Promotion Logic
        if (movePiece.GetType() == typeof(Pawn))
        {
            // Figure out the y direction the pawn should be moving
            int pawnYDirection = BlacksTurn ? -1 : 1;

            // If the move was an EnPassant capture the piece
            if ((toX, toY) == EnPassantTile)
            {
                capturedPiece = PieceLocations[toX, toY - pawnYDirection];
                PieceLocations[toX, toY - pawnYDirection] = null;
            }

            // Record a possible EnPassant tile
            EnPassantTile = (toY - fromY == 2 * pawnYDirection) ? (fromX, fromY + pawnYDirection) : (-1, -1);
        }
        else EnPassantTile = (-1, -1);

        // Castle Logic
        if (movePiece.GetType() == typeof(King))
        {
            if (fromX - toX == 2) MoveFigureTo(0, fromY, toX + 1, toY); // QueenSide
            else if (toX - fromX == 2) MoveFigureTo(8, fromY, toX - 1, toY); // Kingside
        }

        // Switch the turn and increment the turn counter
        BlacksTurn = !BlacksTurn;
        if (!BlacksTurn) TurnNumber++;

        return capturedPiece;
    }

    public bool TileOnBoard(int x, int y) => (x > 0) && (y > 0) & (PieceLocations.GetLength(0) > x) && (PieceLocations.GetLength(0) > y);
    public bool AnyPieceOn(int x, int y) => PieceLocations[x, y] != null;
    public bool TileIsEmpty(int x, int y) => PieceLocations[x, y] == null;
    public bool BlackPieceOn(int x, int y) => AnyPieceOn(x, y) && PieceLocations[x, y].isBlack;
    public bool WhitePieceOn(int x, int y) => AnyPieceOn(x, y) && !PieceLocations[x, y].isBlack;
    public bool HasFriendlyPiece(int x, int y, bool blackIsAlly) => (blackIsAlly ? BlackPieceOn(x, y) : WhitePieceOn(x, y));
    public bool HasEnemyPiece(int x, int y, bool blackIsAlly) => (!blackIsAlly ? BlackPieceOn(x, y) : WhitePieceOn(x, y));
    public bool Threatened(int x, int y, bool blackIsAlly) => (blackIsAlly ? WhitePiecesAttacking(x, y) : BlackPiecesAttacking(x, y)).Count == 0;
    public bool BlackInCheck() => WhitePiecesAttacking(_blackKing.xCoord, _blackKing.yCoord).Count > 0;
    public bool WhiteInCheck() => BlackPiecesAttacking(_whiteking.xCoord, _whiteking.yCoord).Count > 0;

    public List<ChessFigure> DefendingPieces(int x, int y)
    {
        // A list of pieces that could recapture
        List<ChessFigure> defendingPieces = new List<ChessFigure>();

        if (AnyPieceOn(x, y))
        {
            // Go through the pieces that are friendly to the unit
            foreach (ChessFigure figure in (PieceLocations[x, y].isBlack ? _blacksPieces : _whitesPieces))
            {
                // Reverse the piece's color and see how it could be captures
                figure.isBlack = !figure.isBlack;
                if (figure.PossibleAttacks(this)[x, y]) defendingPieces.Add(figure);
                figure.isBlack = !figure.isBlack;
            }
        }

        return defendingPieces;
    }
    public List<ChessFigure> BlackPiecesAttacking(int x, int y, bool includePins = true, bool reverseColor = false)
    {
        List <ChessFigure> attackingPieces = new List<ChessFigure>();

        // Go through the appropriate list of pieces
        foreach (ChessFigure figure in (reverseColor ? _whitesPieces : _blacksPieces))
        {
            // If a figure in that list can attack the tile (checking for pins if specified), add that piece to the return list
            if (figure.PossibleAttacks(this)[x, y] && (includePins || figure.LegalMoves(this)[x, y]))
            {
                attackingPieces.Add(figure);
            }
        }

        return attackingPieces;
    }
    public List<ChessFigure> WhitePiecesAttacking(int x, int y, bool includePins = true) => BlackPiecesAttacking(x, y, includePins, true);


    public bool MoveCreatesCheck(int fromX, int fromY, int toX, int toY)
    {
        // Record the piece before moving it
        ChessFigure toFigure = null;
        if (AnyPieceOn(toX, toY))
        {
            toFigure = PieceLocations[toX, toY];
            (BlacksTurn ? _blacksPieces : _whitesPieces).Remove(toFigure);
        }

        // Move the from piece and check for threats to the king
        ChessFigure fromFigure = PieceLocations[fromX, fromY];
        MoveFigureTo(fromFigure, toX, toY);

        bool inCheck = (BlacksTurn ? BlackInCheck() : WhiteInCheck());

        // Move it back and potentially captured pieces back to the original place
        MoveFigureTo(fromFigure, fromX, fromY);
        if (toFigure != null)
        {
            PieceLocations[toX, toY] = toFigure;
            (BlacksTurn ? _blacksPieces : _whitesPieces).Add(toFigure);
        }

        return inCheck;
    }

    private void MoveFigureTo(int fromX, int fromY, int toX, int toY) => MoveFigureTo(PieceLocations[fromX, fromY], toX, toY);
    private void MoveFigureTo(ChessFigure figure, int x, int y)
    {
        PieceLocations[figure.xCoord, figure.yCoord] = null;

        figure.xCoord = x;
        figure.yCoord = y;

        PieceLocations[x, y] = figure;
    }
}
