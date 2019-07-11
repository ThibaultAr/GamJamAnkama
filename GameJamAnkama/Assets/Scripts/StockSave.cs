using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockSave : MonoBehaviour
{
    public List<GameObject> apples;

    // Start is called before the first frame update
    void Start()
    {
        apples = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddApple(GameObject apple)
    {
        apples.Add(apple);
    }
}
