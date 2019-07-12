using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketStack : MonoBehaviour
{
    [SerializeField] float _dropForce = 10f;

    public int maxAppleLost;
    public float stockDelay;
    public List<GameObject> _appleAnchors = new List<GameObject>();
    public int maxAppleCount = 8;
    public bool isFull = false;

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
        if (_apples.Count >= maxAppleCount)
            isFull = true;
        
        apple.transform.position = _appleAnchors[_apples.Count - 1].transform.position;
        apple.transform.parent = _appleAnchors[_apples.Count - 1].transform;
    }

    private void DropApple(Vector3 direction)
    {
        GameObject droppedApple = _apples.Pop();
        isFull = false;
        droppedApple.GetComponent<Collider>().isTrigger = true;
        droppedApple.GetComponent<Rigidbody>().isKinematic = false;
        droppedApple.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
        droppedApple.transform.parent = null;
        droppedApple.GetComponent<Rigidbody>().AddForce(direction * _dropForce, ForceMode.Impulse);
        StartCoroutine(DisableIsTrigger(droppedApple));
    }

    private IEnumerator DisableIsTrigger(GameObject droppedApple)
    {
        yield return new WaitForSeconds(0.5f);
        droppedApple.GetComponent<Collider>().isTrigger = false;
    }

    public void Shake()
    {
        Vector3 dropDirection = Vector3.zero;
        for (int i = 0; i < Mathf.Min(_apples.Count, maxAppleLost); i++)
        {
            
            dropDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            DropApple(dropDirection);
        }
    }

    public void OnStockCollision(StockSave stock)
    {
        if(_apples.Count > 0)
        {
            StartCoroutine("SaveApples", stock);
            _stockingApples = true;
        }
    }

    public void OnStopStockCollision()
    {
        if(_stockingApples)
        {
            StopCoroutine("SaveApples");
            _stockingApples = false;
        }
    }

    public void OnAppleCollision(GameObject apple)
    {
        apple.transform.position = gameObject.transform.position;
        apple.transform.parent = gameObject.transform;
        apple.transform.Translate(new Vector3(0, 1f+0.5f*_apples.Count, 0));
        apple.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        apple.GetComponent<Rigidbody>().isKinematic = true;
        AddApple(apple);
    }

    public void SaveApple(StockSave stock)
    {
        GameObject savedApple = _apples.Pop();
        if (_apples.Count == 0)
        {
            StopCoroutine("SaveApples");
            _stockingApples = false;
        }
        stock.AddApple(savedApple);
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