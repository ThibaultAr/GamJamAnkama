using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketStack : MonoBehaviour
{
    public int maxAppleLost;
    public float stockDelay;

    private Stack<GameObject> _apples;
    private bool _stockingApples;

    // Start is called before the first frame update
    void Start()
    {
        _apples = new Stack<GameObject>();
        _stockingApples = false;
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
        for (int i = 0; i < Mathf.Max(_apples.Count, maxAppleLost); i++)
        {
            DropApple();
        }
    }

    public void OnStockCollision(StockSave stock)
    {
        StartCoroutine("SaveApples", stock);
        _stockingApples = true;
    }

    public void OnStopStockCollision()
    {
        StopCoroutine("SaveApples");
        _stockingApples = false;
    }

    public void SaveApple(StockSave stock)
    {
        GameObject savedApple = _apples.Pop();
        if (_apples.Count == 0)
        {
            StopCoroutine("SaveApples");
            _stockingApples = false;
        }
        savedApple.transform.position = Vector3.MoveTowards(savedApple.transform.position, stock.transform.position, 0.02f);
    }

    private IEnumerator SaveApples(StockSave stock)
    {
        while(true)
        {
            SaveApple(stock);
            yield return new WaitForSeconds(stockDelay);
        }
    }
}