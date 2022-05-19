using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MapButton : MonoBehaviour
{
    public void MapSelect()
    {
        SceneManager.LoadScene(1);
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void MarioMap()
    {
        SceneManager.LoadScene(2);
    }
    public void ComunityMap()
    {
        SceneManager.LoadScene(3);
    }
    public void Ramdom()
    {
        SceneManager.LoadScene(3);
    }
}
