using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausescreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onpause()
    {
        Time.timeScale = 0;
    }

    public void Onresume()
    {
        Time.timeScale = 1;
    }
}
