using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteExplode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        Invoke("SelfDestroy", 0.2f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
            Debug.Log(other.name);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
