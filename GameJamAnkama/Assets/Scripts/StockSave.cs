using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockSave : MonoBehaviour
{
    public int applesCount;

    // Start is called before the first frame update
    void Start()
    {
        applesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddApple(GameObject apple)
    {
        applesCount+=1;
        Destroy(apple);
    }
}
