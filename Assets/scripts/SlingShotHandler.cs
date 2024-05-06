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
    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;

    [Header("SlingShotArea script")]
    // similar to importing other files 
    [SerializeField] private SlingShotArea _slingShotArea;

    [Header("SlingShot Stats")]
    [SerializeField] private float maxLength = 3.5f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _timeBetweenNextBird = 2f;

    [Header("Bird")]
    // a gameObject that is basically a prefab (the AngeyBird.cs file is tied to the prefab -> to solve this we can make it of type AngryBird)
    [SerializeField] private AngryBird _angryBirdPrefab;
    [SerializeField] private float _angryBirdPositionOffset = 2f;

    private Vector2 _slingShotLinePosition;
    
    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;

    private AngryBird _spawnedAngryBird;
    private bool _birdOnSlingShot;

    private void Awake()
    {
        _leftLineRenderer.enabled = false;  
        _rightLineRenderer.enabled = false;

        SpawnAngryBird();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.isWithinSlingShotArea())
        {
            _clickedWithinArea = true;
        }

        if(Mouse.current.leftButton.isPressed && _clickedWithinArea && _birdOnSlingShot)
        {
            DrawSlingShot();
            PositonAndRotationBird();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _birdOnSlingShot)
        {

            // singleton pattern -> we can directly use the class variables and methods.no reference variable is needed 
            if (GameManager.instance.HasEnoughShots())
            {
                GameManager.instance.UseShot();

                _clickedWithinArea = false;
                _birdOnSlingShot = false;
                _spawnedAngryBird.LaunchBird(_direction, _shotForce)
                setLines(_centerPosition.position);

                if (GameManager.instance.HasEnoughShots())
                {
                    StartCoroutine(SpawnAngryBirdAfterTime());

                }
            }
        }
    }
    #region SlingShotMethods
    private void DrawSlingShot()
    {
        // Mouse.current.position.ReadValue() gives the mouse position in pixels values. but we want the world position 
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // ClampMagnitute basically returns a vector with a cap on magnitude(length)
        _slingShotLinePosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, maxLength);

        setLines(_slingShotLinePosition);

        // direction is bascially is calculated as destination - startpoint 
        _direction = (Vector2)_centerPosition.position - _slingShotLinePosition;
        // normalized means the magnitute of the vector has a value of 1 without changing the direction
        _directionNormalized = _direction.normalized;
    }   

    private void setLines(Vector2 touchPosition)
    {
        _leftLineRenderer.SetPosition(0, touchPosition);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, touchPosition);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);

    }
    #endregion

    #region AngryBirdMethods

    private void SpawnAngryBird()
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        setLines(_idlePosition.position);

        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        // to spawn in a game object (based on a prefab)
        // adding the normalized vector to the starting position to move the game object a bit towards the direction from user click/idle position to center position
        _spawnedAngryBird = Instantiate(_angryBirdPrefab, (Vector2)_idlePosition.position + dir * _angryBirdPositionOffset, Quaternion.identity);

        // the right(the x axis of the bird) is rotated to face the direction of dir vector
        _spawnedAngryBird.transform.right = dir;

        _birdOnSlingShot = true;
    }

    private void PositonAndRotationBird()
    {
        _spawnedAngryBird.transform.position = _slingShotLinePosition + _directionNormalized * _angryBirdPositionOffset;
        _spawnedAngryBird.transform.right = _directionNormalized;
    }

    // co-routine -> a method that can pause execution
    private IEnumerator SpawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBetweenNextBird);

        SpawnAngryBird();
    }

    #endregion
}
