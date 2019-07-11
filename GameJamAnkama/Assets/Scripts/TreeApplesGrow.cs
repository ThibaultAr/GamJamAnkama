using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeApplesGrow : MonoBehaviour
{
    public int maxApples;
    public float averagePop;
    public float popVariance;


    private Queue<GameObject> _apples;
    private bool _coroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        _apples = new Queue<GameObject>();
        StartCoroutine("GrowApples");
        _coroutineRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DropApple()
    {
        GameObject droppedApple = _apples.Dequeue();
        if (!_coroutineRunning)
            StartCoroutine("GrowApples");
    }

    private void GrowApple()
    {
        //change when apples exist
        GameObject apple = new GameObject();
        _apples.Enqueue(apple);
        if (_apples.Count >= maxApples)
        {
            StopCoroutine("GrowApples");
            _coroutineRunning = false;
        }
    }

    private IEnumerator GrowApples()
    {
        _coroutineRunning = true;
        yield return new WaitForSeconds(averagePop + Random.Range(-1 * popVariance, popVariance));
        while (true)
        {
            GrowApple();
            yield return new WaitForSeconds(averagePop + Random.Range(-1 * popVariance, popVariance));
        }
    }
}
