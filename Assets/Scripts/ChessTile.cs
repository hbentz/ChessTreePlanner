using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    public ChessFigure Figure;

    // Make this get-only so that it can't accidentally get attached to a new board
    public ChessBoard Board { get => _board; }
    private ChessBoard _board;

    [SerializeField] Material _darkTile, _lightTile, _darkHighlight, _lightHighlight; // TODO: Two more highlight colors for potential moves
    public bool Highlighted
    {
        get => _highlighted;
        set
        { 
            _highlighted = value;
            if (_highlighted) GetComponent<MeshRenderer>().material = (xCoord + yCoord) % 2 == 1 ? _darkHighlight : _lightHighlight;
            else GetComponent<MeshRenderer>().material = (xCoord + yCoord) % 2 == 1 ? _darkTile : _lightTile;
        }
    }
    private bool _highlighted = false;

    public int xCoord = -1;
    public int yCoord = -1;

    private void Start()
    {
        // On initialization as GameController may reference this many times
        _board = this.GetComponentInParent<ChessBoard>();
        GetComponent<MeshRenderer>().material = (xCoord + yCoord) % 2 == 1 ? _darkTile : _lightTile;
    }

    private void OnMouseDown()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<GameController>().SelectTile(this);
    }

    private void OnMouseUp()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<GameController>().ReleaseOverTile(this);
    }

    private void OnMouseEnter()
    {
        // TODO highlight logic
    }
}
