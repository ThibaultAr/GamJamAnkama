using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockSave : MonoBehaviour
{
    private List<GameObject> _apples;

    // Start is called before the first frame update
    void Start()
    {
        _apples = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddApple(GameObject apple)
    {
        _apples.Add(apple);
    }
}
