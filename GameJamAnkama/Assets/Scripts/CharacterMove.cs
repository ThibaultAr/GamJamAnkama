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
        _basket = Instantiate(basketPrefab, gameObject.transform).GetComponent<BasketStack>();
        _basket.gameObject.transform.Translate(new Vector3(0, 5.6f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<StockSave>() != null)
            _basket.OnStockCollision(collision.gameObject.GetComponent<StockSave>());
        if (collision.gameObject.name.Contains("Apple"))
            _basket.OnAppleCollision(collision.gameObject);
        if (collision.gameObject.name.Contains("Tree"))
            collision.gameObject.GetComponent<TreeApplesGrow>().DropApples();
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<StockSave>() != null)
            _basket.OnStopStockCollision();
    }
}
