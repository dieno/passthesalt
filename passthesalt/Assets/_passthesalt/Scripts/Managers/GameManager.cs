using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform playerSpawnPoint = null;

    [SerializeField]
    private Transform playerPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        SetupObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupObjects()
    {
        playerPosition.position = playerSpawnPoint.position;
    }
}
