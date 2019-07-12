using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeApplesGrow : MonoBehaviour
{
    public int maxApples;
    public float averagePop;
    public float popVariance;
    public GameObject applePrefab;

    
    private bool _coroutineRunning;
    private Dictionary<Vector3, GameObject> _apples;

    // Start is called before the first frame update
    void Start()
    {
        _apples = new Dictionary<Vector3, GameObject>();
        StartCoroutine("GrowApples");
        _coroutineRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropApples()
    {
        foreach (KeyValuePair<Vector3, GameObject> keyValuePair in _apples)
        {
            keyValuePair.Value.GetComponent<Rigidbody>().useGravity = true;
            _apples.Remove(keyValuePair.Key);
        }
        gameObject.GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 18, 5);
        if (!_coroutineRunning)
            StartCoroutine("GrowApples");
    }

    private void GrowApple()
    {
        GameObject apple = Instantiate(applePrefab);
        apple.transform.parent = gameObject.transform;
        apple.transform.localPosition = Vector3.zero;
        Vector3 vectorTemp;
        bool validAngle = true;
        int tries = 0;
        do
        {
            tries += 1;
            vectorTemp = new Vector3(Random.Range(-0.5f, 0.5f), 7 + Random.Range(-4, 4), Random.Range(-0.5f, 0.5f));
            foreach (KeyValuePair<Vector3, GameObject> keyValuePair in _apples)
            {
                if (Vector3.Angle(vectorTemp, keyValuePair.Key) < 20)
                {
                    validAngle = false;
                    break;
                }
            }
        } while (!validAngle && tries<10);
        _apples[vectorTemp] = apple;
        apple.transform.Translate(vectorTemp);
        apple.GetComponent<Rigidbody>().useGravity = false;
        
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
