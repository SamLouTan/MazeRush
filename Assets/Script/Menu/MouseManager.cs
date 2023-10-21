using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorArrow;
    [SerializeField] private Texture2D _cursorOnclick;
    [SerializeField] private Texture2D _cursorOnHover;
    private void Start()
    {
        Cursor.SetCursor(_cursorArrow, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerDown()
    {
        Cursor.SetCursor(_cursorOnHover, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerUp()
    {
        Cursor.SetCursor(_cursorArrow, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerEnter()
    {
        Cursor.SetCursor(_cursorOnclick, Vector2.zero, CursorMode.Auto);
    }
    public void OnPointerExit()
    {
        Cursor.SetCursor(_cursorArrow, Vector2.zero, CursorMode.Auto);
    }
}
