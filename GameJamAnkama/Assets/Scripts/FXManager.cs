using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{

    public GameObject fxSmoke;
    public GameObject fxMeteo;
    public GameObject fxHit;

    public void instanciateFxSmoke(Vector3 pos)
    {
        StartCoroutine(coroutine(fxSmoke, pos));
    }


    public void instanciateMeteorite(Vector3 pos)
    {
        StartCoroutine(coroutine(fxMeteo, pos));
    }

    public void instanciateHit(Vector3 pos)
    {
        StartCoroutine(coroutine(fxHit, pos));
    }

    private IEnumerator coroutine(GameObject fx, Vector3 pos)
    {
        GameObject go = Instantiate(fx, pos, Quaternion.identity);
        yield return new WaitForSeconds(fx.GetComponent<ParticleSystem>().main.duration + 0.5f);
        DestroyImmediate(go);
    }
}
