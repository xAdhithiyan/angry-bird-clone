using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// input system is a package downloaded from the package manager in unity engine (a newer input system compared to the legacy input)
using UnityEngine.InputSystem; 

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Line Renderers")]
    // serializefield is to expose the unity vairable in the engine so it can be assigned to the a particular gameObject
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [Header("Transforms")]
    // to get the static value of one end of the slingshot 
    [SerializeField]private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;

    [Header("SlingShotArea script")]
    // similar to importing other files 
    [SerializeField] private SlingShotArea _slingShotArea;

    [SerializeField] private float maxLength = 3.5f; 
    private Vector2 _slingShotLinePosition;

    private bool _clickedWithinArea;

    private void Awake()
    {
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.isWithinSlingShotArea())
        {
            _clickedWithinArea = true;
        }

        if(Mouse.current.leftButton.isPressed && _clickedWithinArea)
        {
            DrawSlingShot();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _clickedWithinArea = false;
        }
    }
    
    private void DrawSlingShot()
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {   
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }


        // Mouse.current.position.ReadValue() gives the mouse position in pixels values. but we want the world position 
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // ClampMagnitute basically returns a vector with a cap on magnitude(length)
        _slingShotLinePosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, maxLength);

        setLines(_slingShotLinePosition);
    }

    private void setLines(Vector2 touchPosition)
    {
        _leftLineRenderer.SetPosition(0, touchPosition);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, touchPosition);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);

    }
}
