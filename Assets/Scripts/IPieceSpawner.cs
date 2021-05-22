using System;

public interface IPieceSpawner
{
    void SpawnPiece(Type chessPiece, bool isBlack, string tileName, ChessBoard board);
    void SpawnStart(ChessBoard board);
}
