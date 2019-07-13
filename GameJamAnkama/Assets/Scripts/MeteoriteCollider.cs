using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteCollider : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        GetComponentInParent<MeteoriteExplode>().Explode();
        FindObjectOfType<FXManager>().instanciateMeteorite(other.contacts[0].point);
    }
}
