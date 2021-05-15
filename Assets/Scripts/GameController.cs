using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _chessBoardPrefab;

    private ChessBoard _activeChessBoard;
    private ChessTile _activeTile;
    private ChessFigure _activeFigure;

    // Track all the chessboards with _chessBoards[turn][permutation]
    private List<List<ChessBoard>> _chessBoards = new List<List<ChessBoard>>();

    private void Start()
    {
        // Createa a chessboard on staratup and spawn all the pieces
        GameObject firstBoard = Instantiate(_chessBoardPrefab);
        _activeChessBoard = firstBoard.GetComponent<ChessBoard>();
        _activeChessBoard.Selected = true;
        _activeChessBoard.SpawnAll();

        // Add this as this as the 0th turn
        _chessBoards.Add(new List<ChessBoard>());
        _chessBoards[0].Add(_activeChessBoard);
    }

    public void SelectBoard(ChessBoard board)
    {
        if (_activeChessBoard != board)
        {
            _activeChessBoard.Selected = false;
            _activeChessBoard = board;
            board.Selected = true;
        }
    }
    public void SelectTile(ChessTile tile)
    {
        // Make the tile this belongs to the active board
        SelectBoard(tile.Board);

        // If there is no active tile just select it
        if (_activeTile == null)
        {
            _activeTile = tile;
            tile.Selected = true;
        }

        // Otherwise if this a legal move of a piece
        else if (_activeFigure != null && _activeFigure.PossibleMove()[tile.xCoord, tile.yCoord])
        {
            Debug.Log("Moving Piece");
            // TODO: Create a new board
            // TODO: Do move logic on that board
            return;
        }

        // Otherwise deselect the old tile and select the new one
        else
        {
            _activeTile.Selected = false;
            _activeTile = tile;
            _activeTile.Selected = true;
        }

        // If there is a piece on this tile select it
        if (_activeTile.Figure != null) _activeFigure = _activeTile.Figure;
        else _activeFigure = null;

        // TODO: Show all pieces that could move to that tile with arrows
    }

    public void FigureSelected(ChessFigure figure)
    {
        SelectTile(figure.Tile);
    }
}
