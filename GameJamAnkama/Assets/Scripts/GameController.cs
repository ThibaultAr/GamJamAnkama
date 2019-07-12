using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject meteorite;
    public GameObject meteoriteImpact;

    public int delayMeteorite;
    public int varTimeMeteorite;

    private PlayerController[] _players;
    private int _nextMeteorite;
    private int _seconds;
    // Start is called before the first frame update
    void Start()
    {
        _players = FindObjectsOfType<PlayerController>();
        _seconds = 0;
        InvokeRepeating("AddSecond", 1f, 1f);
        _nextMeteorite = delayMeteorite + Random.Range(-1 * varTimeMeteorite, varTimeMeteorite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddSecond()
    {
        _seconds += 1;
        if (_nextMeteorite == _seconds)
        {
            SpawnMeteorite();
            _nextMeteorite += delayMeteorite + Random.Range(-1 * varTimeMeteorite, varTimeMeteorite);
        }
    }

    private void SpawnMeteorite()
    {
        PlayerController victim = _players[Random.Range(0, _players.Length - 1)];
        Vector3 v = victim.transform.position;
        GameObject met = Instantiate(meteorite);
        met.transform.position = v + new Vector3(0, 100, 0);
        GameObject metImpact = Instantiate(meteoriteImpact);
        metImpact.transform.position = v + new Vector3(0, -1*v.y, 0);
    }
}
