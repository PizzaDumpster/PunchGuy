using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Scanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Rescan", 0, 5f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Rescan()
    {
        AstarPath.active.Scan();
    }
}
