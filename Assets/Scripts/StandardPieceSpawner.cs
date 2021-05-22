using System;
using System.Text;
using UnityEngine;

public class StandardPieceSpawner : MonoBehaviour, IPieceSpawner
{
    private IPieceGetter _pieceGetter;

    private void Awake()
    {
        // Get the piece getter on awake
        _pieceGetter = GameObject.FindGameObjectWithTag("GameController").GetComponent<IPieceGetter>();
    }

    public void SpawnPiece(Type chessPiece, bool isBlack, string tileName, ChessBoard board)
    {
        // Instantiate the piece, place it, and track it
        GameObject unityFigure = Instantiate(_pieceGetter.PiecePrefab(chessPiece, isBlack));
        ChessFigure figure = unityFigure.GetComponent<ChessFigure>();
        ChessTile tile = board.transform.Find(tileName).GetComponent<ChessTile>();

        // Move the piece to the tile and doubly link them
        figure.Tile = tile;
        tile.Figure = figure;
        figure.SetPosition(tile);
        figure.HasMoved = false;  // Reset the HasMoved indicator

        // Track the piece in the active figures
        board.ActiveFigures.Add(figure);

        // Track the kings
        if (chessPiece == typeof(King))
        {
            if (isBlack) board.BlackKing = figure;
            else board.WhiteKing = figure;
        }
    }

    public void SpawnStart(ChessBoard board)
    {
        // Spawns all pieces on the starting board
        SpawnPiece(typeof(Rook), false, "a1", board);
        SpawnPiece(typeof(Knight), false, "b1", board); // Nb1
        SpawnPiece(typeof(Bishop), false, "c1", board); // Bc1
        SpawnPiece(typeof(Queen), false, "d1", board); // Qd1
        SpawnPiece(typeof(King), false, "e1", board); // Ke1
        SpawnPiece(typeof(Bishop), false, "f1", board); // Bf1
        SpawnPiece(typeof(Knight), false, "g1", board); // Ng1
        SpawnPiece(typeof(Rook), false, "h1", board); // Rh1

        for (int i = 0; i < 8; i++)
        {
            string tileFile = ((char)(i + 97)).ToString();
            SpawnPiece(typeof(Pawn), false, tileFile + "2", board);
            SpawnPiece(typeof(Pawn), true, tileFile + "7", board);
        }

        SpawnPiece(typeof(Rook), true, "a8", board); // Ra8
        SpawnPiece(typeof(Knight), true, "b8", board); // Nb8
        SpawnPiece(typeof(Bishop), true, "c8", board); // Bc8
        SpawnPiece(typeof(Queen), true, "d8", board); // Qd8
        SpawnPiece(typeof(King), true, "e8", board); // Ke8
        SpawnPiece(typeof(Bishop), true, "f8", board); // Bf8
        SpawnPiece(typeof(Knight), true,"g8" , board); // Ng8
        SpawnPiece(typeof(Rook), true, "h8", board); // Rh8
    }
}
