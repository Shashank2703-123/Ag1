using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string Levelname;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Levelname);
        }
    }

    public void Lvlselect()
    {
        SceneManager.LoadScene(1);
    }

    public void Lvlone()
    {
        SceneManager.LoadScene(2);
    }

    public void Lvltwo()
    {
        SceneManager.LoadScene(3);
    }

    public void Lvlthree()
    {
        SceneManager.LoadScene(4);
    }
    public void ToMM()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quited");
    }
}
