using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAssetGenerator : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        // GameObject[] halls = GameObject.FindGameObjectsWithTag("halls");
        // GameObject[] stairs = GameObject.FindGameObjectsWithTag("stairs");

        Debug.Log(placeHolderCubes.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
