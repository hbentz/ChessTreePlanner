using System;
using UnityEngine;

public class MaterialChangeHighlighter : MonoBehaviour, IHighlighter
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    private MeshRenderer _renderer;
    
    public bool Selected { get; }
    private bool _selected;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = _defaultMaterial;
    }

    public void Select()
    {
        if (!_selected)
        {
            _selected = true;
            _renderer.material = _highlightMaterial;
        }
    }

    public void Deselect()
    {
        if (_selected)
        {
            _selected = false;
            _renderer.material = _defaultMaterial;
        }
    }
}
