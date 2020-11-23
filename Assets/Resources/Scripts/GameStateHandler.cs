using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleFileBrowser;
using System;

public class GameStateHandler : MonoBehaviour
{

    public void Start()
    {
        if (!Grid_Inspector.instancerunning)
        {
            StartCoroutine("CheckIfGameover");
            Grid_Inspector.instancerunning = true;
        }
    }

    /// <summary>
    /// Checks if the game has ended.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckIfGameover()
    {
        while (true)
        {
            if (
                Grid_Inspector.CURRENT_BERRIES1 >= Grid_Inspector.NEED_BERRIES1 &&
                Grid_Inspector.CURRENT_GOLD1 >= Grid_Inspector.NEED_GOLD1 &&
                Grid_Inspector.CURRENT_STONE1 >= Grid_Inspector.NEED_STONE1 &&
                Grid_Inspector.CURRENT_WOOD1 >= Grid_Inspector.NEED_WOOD1
                )
            {
                StopNPCS();
                Grid_Inspector.gameui.SetActive(false);
                Grid_Inspector.endpanel.SetActive(true);
                GameObject wintxt = GameObject.FindGameObjectWithTag("wontxt");
                GameObject reason = GameObject.FindGameObjectWithTag("reasontxt");
                wintxt.GetComponent<Text>().text = "TEAM 1 WON";
                reason.GetComponent<Text>().text = "Reason: Team 1 gathered first the required resources.";
                Grid_Inspector.winner = "Team 1";
               
                Grid_Inspector.time = Time.realtimeSinceStartup-Grid_Inspector.oldtime;
                Grid_Inspector.oldtime = Time.realtimeSinceStartup;
                Time.timeScale = 0;
                yield break;
            }
            else if (
                Grid_Inspector.CURRENT_BERRIES2 >= Grid_Inspector.NEED_BERRIES2 &&
                Grid_Inspector.CURRENT_GOLD2 >= Grid_Inspector.NEED_GOLD2 &&
                Grid_Inspector.CURRENT_STONE2 >= Grid_Inspector.NEED_STONE2 &&
                Grid_Inspector.CURRENT_WOOD2 >= Grid_Inspector.NEED_WOOD2
                )
            {
                StopNPCS();
                Grid_Inspector.gameui.SetActive(false);
                Grid_Inspector.endpanel.SetActive(true);
                GameObject wintxt = GameObject.FindGameObjectWithTag("wontxt");
                GameObject reason = GameObject.FindGameObjectWithTag("reasontxt");
                wintxt.GetComponent<Text>().text = "TEAM 2 WON";
                reason.GetComponent<Text>().text = "Reason: Team 2 gathered first the required resources.";
                Grid_Inspector.winner = "Team 2";
                Grid_Inspector.time = Time.realtimeSinceStartup - Grid_Inspector.oldtime;
                Grid_Inspector.oldtime = Time.realtimeSinceStartup;
                Time.timeScale = 0;
                yield break;
            }
            else if (
                (Grid_Inspector.CURRENT_BERRIES1 < Grid_Inspector.NEED_BERRIES1 ||
                Grid_Inspector.CURRENT_GOLD1 < Grid_Inspector.NEED_GOLD1 ||
                Grid_Inspector.CURRENT_STONE1 < Grid_Inspector.NEED_STONE1 ||
                Grid_Inspector.CURRENT_WOOD1 < Grid_Inspector.NEED_WOOD1) && Grid_Inspector.npcs1.Count <= 0
                )
            {
                StopNPCS();
                Grid_Inspector.gameui.SetActive(false);
                Grid_Inspector.endpanel.SetActive(true);
                GameObject wintxt = GameObject.FindGameObjectWithTag("wontxt");
                GameObject reason = GameObject.FindGameObjectWithTag("reasontxt");
                wintxt.GetComponent<Text>().text = "TEAM 2 WON";
                reason.GetComponent<Text>().text = "Reason: All the villagers from team 1 are dead.";
                Grid_Inspector.winner = "Team 2";
                Grid_Inspector.time = Time.realtimeSinceStartup - Grid_Inspector.oldtime;
                Grid_Inspector.oldtime = Time.realtimeSinceStartup;
                Time.timeScale = 0;
                yield break;

            }
            else if (
                (Grid_Inspector.CURRENT_BERRIES2 < Grid_Inspector.NEED_BERRIES2 ||
                Grid_Inspector.CURRENT_GOLD2 < Grid_Inspector.NEED_GOLD2 ||
                Grid_Inspector.CURRENT_STONE2 < Grid_Inspector.NEED_STONE2 ||
                Grid_Inspector.CURRENT_WOOD2 < Grid_Inspector.NEED_WOOD2) && Grid_Inspector.npcs2.Count <= 0
                )
            {
                StopNPCS();
                Grid_Inspector.gameui.SetActive(false);
                Grid_Inspector.endpanel.SetActive(true);
                GameObject wintxt = GameObject.FindGameObjectWithTag("wontxt");
                GameObject reason = GameObject.FindGameObjectWithTag("reasontxt");
                wintxt.GetComponent<Text>().text = "TEAM 1 WON";
                reason.GetComponent<Text>().text = "Reason: All the villagers from team 2 are dead.";
                Grid_Inspector.winner = "Team 1";
                Grid_Inspector.time = Time.realtimeSinceStartup - Grid_Inspector.oldtime;
                Grid_Inspector.oldtime = Time.realtimeSinceStartup;
                Time.timeScale = 0;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    /// <summary>
    /// Pauses the game
    /// </summary>
   public void PauseGame()
    {
        Time.timeScale = 0;
        Grid_Inspector.pausepanel.SetActive(true);
    }
    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        Grid_Inspector.pausepanel.SetActive(false);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Restarts the game with the same settings.
    /// </summary>
    public void RestartSameGame()
    {
        StopNPCS();
        System.Threading.Thread.Sleep(3000);
        Grid_Inspector.npcs1.Clear();
        Grid_Inspector.npcs2.Clear();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Restarts the game with new settings.
    /// </summary>
    public void RestartNewGame()
    {
        Time.timeScale = 1;
        StopNPCS();
        System.Threading.Thread.Sleep(3000);
        Grid_Inspector.npcs1.Clear();
        Grid_Inspector.npcs2.Clear();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Stops the NPC threads.
    /// </summary>
    public void StopNPCS()
    {
        foreach (GameObject gm in Grid_Inspector.npcs1)
        {
            gm.GetComponent<NPC>().dead = true;
        }
        foreach (GameObject gm in Grid_Inspector.npcs2)
        {
            gm.GetComponent<NPC>().dead = true;
        }
    }
    /// <summary>
    /// Save statistics to file.
    /// </summary>
    public void SaveStats()
    {
        StartCoroutine("ShowSaveDialogCoroutine");
    }

    /// <summary>
    /// Show save dialog and save.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowSaveDialogCoroutine()
    {
        yield return FileBrowser.WaitForSaveDialog(true, false, null, "Select Folder", "Save");
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {

           string path = FileBrowser.Result[0];
            path += "\\Statistics.txt";
            FileStream stream;
            stream = new FileStream(path, FileMode.OpenOrCreate);
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                sw.WriteLine("Winner Team: " + Grid_Inspector.winner);
                sw.WriteLine("Time Elapsed: " + (int)Grid_Inspector.time / 60 + "'" + (int)Grid_Inspector.time % 60 + "''" + "\n\n");
                sw.WriteLine("Team 1 Statistics\n");
                sw.WriteLine("Gold gathered: " + Grid_Inspector.CURRENT_GOLD1 + "/" + Grid_Inspector.NEED_GOLD1);
                sw.WriteLine("Wood gathered: " + Grid_Inspector.CURRENT_WOOD1 + "/" + Grid_Inspector.NEED_WOOD1);
                sw.WriteLine("Stone gathered: " + Grid_Inspector.CURRENT_STONE1 + "/" + Grid_Inspector.NEED_STONE1);
                sw.WriteLine("Berries gathered: " + Grid_Inspector.CURRENT_BERRIES1 + "/" + Grid_Inspector.NEED_BERRIES1);
                sw.WriteLine("Potions Collected: " + Grid_Inspector.team1potions);
                sw.WriteLine("\n\n");
                sw.WriteLine("Team 2 Statistics\n");
                sw.WriteLine("Gold gathered: " + Grid_Inspector.CURRENT_GOLD2 + "/" + Grid_Inspector.NEED_GOLD2);
                sw.WriteLine("Wood gathered: " + Grid_Inspector.CURRENT_WOOD2 + "/" + Grid_Inspector.NEED_WOOD2);
                sw.WriteLine("Stone gathered: " + Grid_Inspector.CURRENT_STONE2 + "/" + Grid_Inspector.NEED_STONE2);
                sw.WriteLine("Berries gathered: " + Grid_Inspector.CURRENT_BERRIES2 + "/" + Grid_Inspector.NEED_BERRIES2);
                sw.WriteLine("Potions Collected: " + Grid_Inspector.team2potions);
                sw.WriteLine("\n\n");
                sw.WriteLine("Trade Statistics\n");
                sw.WriteLine("Energy Trade between members of team 1: " + Grid_Inspector.Team1EnergyTrades);
                sw.WriteLine("Energy Trade between members of team 2: " + Grid_Inspector.Team2EnergyTrades);
                sw.WriteLine("Energy Cross team trades: " + Grid_Inspector.CrossTeamEnergyTrades);
                sw.WriteLine("Map Trade between members of team 1: " + Grid_Inspector.Team1MapTrades);
                sw.WriteLine("Map Trade between members of team 2: " + Grid_Inspector.Team2MapTrades);
                sw.WriteLine("Map Cross team trades: " + Grid_Inspector.CrossTeamMapTrades);

            }
        }
    }
   
}
