using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitGame:MonoBehaviour
{
    public void ExitGameMenu()
    {
        Application.Quit();
    }
    public void ExitGame()
    {
        foreach (GameObject gm in Grid_Inspector.npcs1)
        {
            gm.GetComponent<NPC>().dead = true;
        }
        foreach (GameObject gm in Grid_Inspector.npcs2)
        {
            gm.GetComponent<NPC>().dead = true;
        }
        System.Threading.Thread.Sleep(3000);
        Application.Quit();
    }
}
