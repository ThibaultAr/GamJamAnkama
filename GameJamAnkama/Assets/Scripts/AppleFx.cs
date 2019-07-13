using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleFx : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
            GameObject.FindObjectOfType<FXManager>().instanciateFxSmoke(collision.contacts[0].point);
    }
}
