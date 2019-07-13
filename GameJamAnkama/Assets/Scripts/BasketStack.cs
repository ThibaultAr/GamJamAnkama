using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketStack : MonoBehaviour
{
    [SerializeField] float _dropForce = 10f;

    public int maxAppleLost;
    public float stockDelay;
    public List<GameObject> _appleAnchors = new List<GameObject>();

    [HideInInspector]
    public int currentAppleCount;
    public int maxAppleCount = 8;

    private Stack<GameObject> _apples;
    
    private bool _stockingApples;

    private Coroutine saveApplesCoroutine;

    public int getAppleCount()
    {
        return _apples.Count;
    }

    // Start is called before the first frame update
    void Start()
    {
        _apples = new Stack<GameObject>();
        _stockingApples = false;
    }

    public void AddApple(GameObject apple)
    {
        _apples.Push(apple);
        SOUND.Grab();
        
        apple.transform.position = _appleAnchors[_apples.Count - 1].transform.position;
        apple.transform.parent = _appleAnchors[_apples.Count - 1].transform;
    }

    public void ConsumeApple()
    {
        GameObject droppedApple = _apples.Pop();
        Destroy(droppedApple);
    }

    private void DropApple(Vector3 direction)
    {
        GameObject droppedApple = _apples.Pop();
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

    public void OnStockCollision(StockSave stock, int playerIndex)
    {
        if(_apples.Count > 0)
        {
            saveApplesCoroutine = StartCoroutine(SaveApples(stock, playerIndex));
            _stockingApples = true;
        }
    }

    public void OnStopStockCollision()
    {
        if(_stockingApples)
        {
            if(saveApplesCoroutine != null)
                StopCoroutine(saveApplesCoroutine);
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

    public void SaveApple(StockSave stock, int playerIndex)
    {
        GameObject savedApple = _apples.Pop();
        if (_apples.Count == 0)
        {
            if (saveApplesCoroutine != null)
                StopCoroutine(saveApplesCoroutine);
            _stockingApples = false;
        }
        stock.AddApple(savedApple, playerIndex);
    }

    private IEnumerator SaveApples(StockSave stock, int playerIndex)
    {
        while(true)
        {
            SaveApple(stock, playerIndex);
            yield return new WaitForSeconds(stockDelay);
        }
    }

    public GameObject GetProjectile()
    {
        if (_apples.Count > 0)
        {
            GameObject apple = _apples.Pop();
            apple.transform.parent = null;
            return apple;
        }
        else
            return null;
    }

    public bool IsEmpty()
    {
        return _apples.Count == 0;
    }

    public bool IsFull()
    {
        return _apples.Count >= maxAppleCount;
    }
}