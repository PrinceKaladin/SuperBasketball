using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTravel : MonoBehaviour
{
    public Transform[] menusW;
    public AudioSource sf;
    public void makeMenu(int k)
    {
        foreach (var menu in menusW) { menu.gameObject.SetActive(false); }

        menusW[k].gameObject.SetActive(true);
        sf.Play();
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
