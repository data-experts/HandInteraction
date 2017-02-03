using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : MonoBehaviour, HandSelectable
{

    private bool _selected = false;
    private Renderer _myRenderer;
    private HandInteractionMaster _curHandInteraction;
    public Color HighlightClr = Color.yellow;
    public Color SelectClr = Color.red;
    private Color _defaultClr;

    // Use this for initialization
    void Start()
    {
        _myRenderer = GetComponent<Renderer>();
        _defaultClr = _myRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_curHandInteraction != null && _curHandInteraction.CurFocusedObj != gameObject)
        {
            _curHandInteraction = null;
        }
        UpdateColor();
    }

    public void Select()
    {
        _selected = !_selected;
    }

    void UpdateColor()
    {
        if (_curHandInteraction != null && _curHandInteraction.CurFocusedObj == gameObject)
        {
            _myRenderer.material.color = HighlightClr;
        }
        else if (_selected)
        {
            _myRenderer.material.color = SelectClr;
        }
        else
        {
            _myRenderer.material.color = _defaultClr;
        }
    }

    void Highlight(HandInteractionMaster interaction)
    {
        _curHandInteraction = interaction;
    }
}
