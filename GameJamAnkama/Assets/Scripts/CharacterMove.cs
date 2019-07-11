using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public GameObject basketPrefab;

    private BasketStack _basket;

    // Start is called before the first frame update
    void Start()
    {
        _basket = Instantiate(basketPrefab, gameObject.transform).GetComponent<BasketStack>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<StockSave>() != null)
            _basket.OnStockCollision(collision.gameObject.GetComponent<StockSave>());
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<StockSave>() != null)
            _basket.OnStopStockCollision();
    }
}
