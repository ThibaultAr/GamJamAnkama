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
    private bool _fullTree;
    private Dictionary<Vector3, GameObject> _apples;

    // Start is called before the first frame update
    void Start()
    {
        _fullTree = false;
        _apples = new Dictionary<Vector3, GameObject>();
        StartCoroutine("GrowApples");
        _coroutineRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        if(_apples.ContainsValue(other.gameObject))
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void DropApples()
    {
        foreach (KeyValuePair<Vector3, GameObject> keyValuePair in _apples)
        {
            DropApple(keyValuePair.Key, keyValuePair.Value);
        }
        _apples.Clear();
    }

    private void DropApple(Vector3 key, GameObject value)
    {
        value.GetComponent<Rigidbody>().useGravity = true;
        Vector3 v = key;
        v.y = 4;
        value.GetComponent<Rigidbody>().isKinematic = false;
        value.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(v) * 4, ForceMode.Impulse);
    }

    private void GrowApple()
    {
        Vector3 vectorTemp;
        bool validAngle = true;
        int tries = 0;
        do
        {
            validAngle = true;
            tries += 1;
            vectorTemp = new Vector3(Random.Range(-0.3f, 0.3f), 4f + Random.Range(-1f, 1f), Random.Range(-0.3f, 0.3f));
            Vector2 vecCalc1 = new Vector2();
            Vector2 vecCalc2 = new Vector2();
            foreach (KeyValuePair<Vector3, GameObject> keyValuePair in _apples)
            {
                if (0.9*vectorTemp.x < vectorTemp.z-0.1)
                {
                    vectorTemp.x *= -1;
                    vectorTemp.z *= -1;
                }

                vecCalc1.Set(vectorTemp.x, vectorTemp.z);
                vecCalc2.Set(keyValuePair.Key.x, keyValuePair.Key.z);

                if (Vector2.Angle(vecCalc1, vecCalc2) < 50)
                {
                    validAngle = false;
                    break;
                }
            }
        } while (!validAngle && tries<100);
        if(validAngle)
        {
            if (_apples.Count == maxApples)
            {
                Vector3[] keys = new Vector3[maxApples];
                _apples.Keys.CopyTo(keys, 0);
                DropApple(keys[maxApples-1], _apples[keys[maxApples-1]]);
                _apples.Remove(keys[maxApples-1]);
                _fullTree = true;
            }
            else
                _fullTree = false;
            GameObject apple = Instantiate(applePrefab);
            apple.transform.parent = gameObject.transform;
            apple.transform.localPosition = Vector3.zero;
            apple.transform.Translate(vectorTemp);
            apple.GetComponent<Rigidbody>().useGravity = false;
            vectorTemp.y = 0;
            apple.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(vectorTemp) * 10, ForceMode.Impulse);
            Vector3 v = apple.GetComponent<Rigidbody>().velocity;
            v.y = 0;
            apple.GetComponent<Rigidbody>().velocity = v;
            _apples[vectorTemp] = apple;
        }
    }

    private IEnumerator GrowApples()
    {
        _coroutineRunning = true;
        yield return new WaitForSeconds(_fullTree? averagePop*2 : averagePop + Random.Range(-1 * popVariance, popVariance));
        while (true)
        {
            GrowApple();
            yield return new WaitForSeconds(_fullTree ? averagePop * 2 : averagePop + Random.Range(-1 * popVariance, popVariance));
        }
    }
}
