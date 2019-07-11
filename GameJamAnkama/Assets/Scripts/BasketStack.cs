using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketStack : MonoBehaviour
{
    public const int MAX_APPLE_LOST = 3;

    private Stack<GameObject> _apples;

    // Start is called before the first frame update
    void Start()
    {
        _apples = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddApple(GameObject apple)
    {
        _apples.Push(apple);
    }

    private void DropApple()
    {
        GameObject droppedApple = _apples.Pop();
    }

    public void Shake()
    {
        for (int i = 0; i < Mathf.Max(_apples.Count, MAX_APPLE_LOST); i++)
        {
            DropApple();
        }
    }

    public void SaveApple(StockSave stock)
    {
        GameObject savedApple = _apples.Pop();
    }
}