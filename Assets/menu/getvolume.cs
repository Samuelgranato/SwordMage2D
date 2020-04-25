using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getvolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = GameState.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
