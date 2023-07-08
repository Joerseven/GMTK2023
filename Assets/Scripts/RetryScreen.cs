using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryScreen : MonoBehaviour
{
    public GameLoop game;
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        game.BeginLevel();
    }
}
