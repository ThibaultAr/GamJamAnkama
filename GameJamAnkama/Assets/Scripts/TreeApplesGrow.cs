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

    public void OnTriggerExit(Collider other)
    {
        if(other.name.Contains("apple"))
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void DropApples()
    {
        Vector3 v;
        foreach (KeyValuePair<Vector3, GameObject> keyValuePair in _apples)
        {
            keyValuePair.Value.GetComponent<Rigidbody>().useGravity = true;
            v = keyValuePair.Key;
            v.y = 8;
            keyValuePair.Value.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(v)*5, ForceMode.Impulse);
        }
        _apples.Clear();
        if (!_coroutineRunning)
            StartCoroutine("GrowApples");
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
            vectorTemp = new Vector3(Random.Range(-0.3f, 0.3f), 12.5f + Random.Range(-1f, 1f), Random.Range(-0.3f, 0.3f));
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
            GameObject apple = Instantiate(applePrefab);
            apple.transform.parent = gameObject.transform;
            apple.transform.localPosition = Vector3.zero;
            apple.transform.Translate(vectorTemp);
            apple.GetComponent<Rigidbody>().useGravity = false;
            vectorTemp.y = 0;
            Debug.DrawRay(apple.transform.position, Vector3.Normalize(vectorTemp) * 10, Color.red, 5);
            apple.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(vectorTemp) * 10, ForceMode.Impulse);
            Vector3 v = apple.GetComponent<Rigidbody>().velocity;
            v.y = 0;
            apple.GetComponent<Rigidbody>().velocity = v;
            _apples[vectorTemp] = apple;

            if (_apples.Count >= maxApples)
            {
                StopCoroutine("GrowApples");
                _coroutineRunning = false;
            }
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
