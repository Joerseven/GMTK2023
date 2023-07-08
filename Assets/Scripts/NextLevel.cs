using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameLoop game;
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        game.BeginLevel();
    }
}
