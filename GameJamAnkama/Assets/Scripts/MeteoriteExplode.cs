using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteExplode : MonoBehaviour
{

    public void Explode()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        Invoke("SelfDestroy", 0.2f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Bump(transform.position, true);
            other.GetComponent<PlayerController>().spriteGO.GetComponentInChildren<BasketStack>().Shake();
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
