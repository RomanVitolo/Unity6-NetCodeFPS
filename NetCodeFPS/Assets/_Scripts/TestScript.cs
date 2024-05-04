using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    private Controls _controls;

    private void Start()
    {
        _inputReader.MoveEvent += HandleMove;
    }

    private void HandleMove(Vector2 obj)
    {
        Debug.Log(obj);
    }

    private void OnDestroy()
    {
        _inputReader.MoveEvent -= HandleMove;
    }
}
