using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _chessBoardPrefab;
    [SerializeField] private GameObject _pawnPromotionUIPrefabWhite;
    [SerializeField] private GameObject _pawnPromotionUIPrefabBlack;

    private ChessBoard _activeChessBoard;
    private ChessTile _activeTile;
    private ChessFigure _activeFigure;

    // Track all the chessboards with _chessBoards[turn][permutation]
    private List<List<ChessBoard>> _chessBoards = new List<List<ChessBoard>>();
    private List<ChessTile> _potentialMoves = new List<ChessTile>();

    private IPieceSpawner _pieceSpawner;

    private void Awake()
    {
        _pieceSpawner = GetComponent<IPieceSpawner>();
    }

    private void Start()
    {
        // Create a chessboard on start up and spawn all the pieces
        GameObject firstBoard = Instantiate(_chessBoardPrefab);
        _activeChessBoard = firstBoard.GetComponent<ChessBoard>();
        _activeChessBoard.Highlighter.Select();

        // Add this as this as the 0th turn
        _chessBoards.Add(new List<ChessBoard>());
        _chessBoards[0].Add(_activeChessBoard);
    }

    private void Update()
    {
        // TODO get _activeFigure to follow mouse when holding mouse down
    }

    public void SpawnStartWrapper() => _pieceSpawner.SpawnStart(_activeChessBoard);

    public void SelectBoard(ChessBoard board)
    {
        // Set the selected board to active
        if (_activeChessBoard != board)
        {
            _activeChessBoard.Highlighter.Deselect();
            _activeChessBoard = board;
            board.Highlighter.Select();
        }
    }
    public void SelectTile(ChessTile tile)
    {
        // Make the tile this belongs to the active board
        SelectBoard(tile.Board);

        // If there is an active piece and this is a legal move make the move then deactivate the piece
        if (_activeFigure != null && _potentialMoves.Contains(tile))
        {
            MovePiece(_activeFigure, tile);
            _activeFigure = null;
            return;
        }

        foreach (ChessTile moveTile in _potentialMoves) moveTile.GetComponent<IHighlighter>().Deselect();  // Unhighlights the previously highlighted moves

        // Deselect the old tile and select the new one
        if (_activeTile != null) _activeTile.GetComponent<IHighlighter>().Deselect();
        _activeTile = tile;
        _activeTile.GetComponent<IHighlighter>().Select();

        // If there is a piece on this tile select it as well, otherwise deselect the active piece
        if (_activeTile.Figure != null)
        {
            _activeFigure = _activeTile.Figure;
            bool[,] legalMoves = _activeFigure.LegalMoves();
            
            // Get the tile references for each legal move
            _potentialMoves = new List<ChessTile>();
            for (int x = 0; x < 8; x ++) for (int y = 0; y < 8; y++) if (legalMoves[x, y]) _potentialMoves.Add(_activeChessBoard.Tiles[x, y]);

            // Highlight those squares
            foreach (ChessTile moveTile in _potentialMoves) moveTile.GetComponent<IHighlighter>().Select();
        }
        else _activeFigure = null;

        _activeChessBoard.ClearThreatArrows();
        _activeChessBoard.DrawThreatArrowsToTile(_activeTile);
    }

    public void MovePiece(ChessFigure figure, ChessTile targetTile)
    {
        // Remove this piece from the old tile
        figure.Tile.Figure = null;

        // If there is a piece on the target tile destroy it
        if (targetTile.Figure != null) Destroy(targetTile.Figure.gameObject);

        // Move this piece to the new tile in Unity
        figure.SetPosition(targetTile);

        // Doubly link them:
        figure.Tile = targetTile;
        targetTile.Figure = figure;

        // Change the turn
        figure.Tile.Board.IsBlacksTurn = !figure.Tile.Board.IsBlacksTurn;
    }

    public void PromotePawn(Pawn pawn, ChessTile promotionTile)
    {
        // Spawn the pawn promotion dialouge and forget about it
        GameObject promotionUI = Instantiate(pawn.isBlack ? _pawnPromotionUIPrefabBlack : _pawnPromotionUIPrefabWhite, promotionTile.transform);
        promotionUI.transform.localPosition = new Vector3(0f, 0.05f, pawn.isBlack ? -6.25f : 6.25f);
        promotionUI.GetComponent<PawnPromotionUI>().PromotionPawn = pawn;
    }

    public void ReleaseOverTile(ChessTile tile)
    {
        //TODO: Piece Drag and drop logic
    }
}
