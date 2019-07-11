using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockSave : MonoBehaviour
{
    private int _applesCount;

    // Start is called before the first frame update
    void Start()
    {
        _applesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddApple(GameObject apple)
    {
        _applesCount += 1;
    }
}
