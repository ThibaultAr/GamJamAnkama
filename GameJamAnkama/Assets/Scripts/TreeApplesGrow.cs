using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeApplesGrow : MonoBehaviour
{
    public int maxApples;
    public float averagePop;
    public float popVariance;
    public GameObject applePrefab;


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

    public void DropApple()
    {
        GameObject droppedApple = _apples.Dequeue();
        droppedApple.GetComponent<Rigidbody>().useGravity = true;
        if (!_coroutineRunning)
            StartCoroutine("GrowApples");
    }

    private void GrowApple()
    {
        GameObject apple = Instantiate(applePrefab, gameObject.transform, false);
        apple.transform.Translate(new Vector3(0, 4, 0));
        apple.transform.parent = gameObject.transform;
        apple.GetComponent<Rigidbody>().useGravity = false;
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
