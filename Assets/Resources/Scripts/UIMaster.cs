using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    public static GameObject Gold1;
    public static GameObject Stone1;
    public static GameObject Wood1;
    public static GameObject Berries1;
    public static GameObject Gold2;
    public static GameObject Stone2;
    public static GameObject Wood2;
    public static GameObject Berries2;
    GameObject[,] visible = new GameObject[Grid_Inspector.N, Grid_Inspector.M];
    string[,] known = new string[Grid_Inspector.N, Grid_Inspector.M];
    public string selectedview="";
    public  GameObject player;
    void Start()
    {
        StartCoroutine(RoutineView());
        Gold1 = GameObject.FindGameObjectWithTag("Gold1");
        Stone1 = GameObject.FindGameObjectWithTag("Stone1");
        Wood1 = GameObject.FindGameObjectWithTag("Wood1");
        Berries1 = GameObject.FindGameObjectWithTag("Berries1");
        Gold2 = GameObject.FindGameObjectWithTag("Gold2");
        Stone2 = GameObject.FindGameObjectWithTag("Stone2");
        Wood2 = GameObject.FindGameObjectWithTag("Wood2");
        Berries2 = GameObject.FindGameObjectWithTag("Berries2");
        RefreshResources();
    }

    void Update()
    {
    }
    public static void RefreshResources()
    {
        Gold1.GetComponent<UnityEngine.UI.Text>().text ="Gold "+ Grid_Inspector.CURRENT_GOLD1 + "/" + Grid_Inspector.NEED_GOLD1;
        Gold2.GetComponent<UnityEngine.UI.Text>().text = "Gold " + Grid_Inspector.CURRENT_GOLD2 + "/" + Grid_Inspector.NEED_GOLD2;
        Stone1.GetComponent<UnityEngine.UI.Text>().text = "Stone " + Grid_Inspector.CURRENT_STONE1 + "/" + Grid_Inspector.NEED_STONE1;
        Stone2.GetComponent<UnityEngine.UI.Text>().text = "Stone " + Grid_Inspector.CURRENT_STONE2 + "/" + Grid_Inspector.NEED_STONE2;
        Wood1.GetComponent<UnityEngine.UI.Text>().text = "Wood " + Grid_Inspector.CURRENT_WOOD1 + "/" + Grid_Inspector.NEED_WOOD1;
        Wood2.GetComponent<UnityEngine.UI.Text>().text = "Wood " + Grid_Inspector.CURRENT_WOOD2 + "/" + Grid_Inspector.NEED_WOOD2;
        Berries1.GetComponent<UnityEngine.UI.Text>().text = "Berries " + Grid_Inspector.CURRENT_BERRIES1 + "/" + Grid_Inspector.NEED_BERRIES1;
        Berries2.GetComponent<UnityEngine.UI.Text>().text = "Berries " + Grid_Inspector.CURRENT_BERRIES2 + "/" + Grid_Inspector.NEED_BERRIES2;
    }
    public void ShowExploredAreas(string team)
    {
        for (int i = 0; i < known.GetLength(0); i++)
        {
            for (int j = 0; j < known.GetLength(1); j++)
            {
                known[i, j] = "0";
            }
        }
        if (team == "Team1")
        {
            if (Grid_Inspector.npcs1.Count > 0)
            {
                foreach (GameObject gm in Grid_Inspector.npcs1)
                {
                    string[,] map = gm.GetComponent<NPC>().knownmap;
                    for (int i = 0; i < known.GetLength(0); i++)
                    {
                        for (int j = 0; j < known.GetLength(1); j++)
                        {
                            if (map[i, j] == "1" && known[i, j] == "0")
                            {
                                known[i, j] = "1";
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (Grid_Inspector.npcs2.Count > 0)
            {
                foreach (GameObject gm in Grid_Inspector.npcs2)
                {
                    string[,] map = gm.GetComponent<NPC>().knownmap;
                    for (int i = 0; i < known.GetLength(0); i++)
                    {
                        for (int j = 0; j < known.GetLength(1); j++)
                        {
                            if (map[i, j] == "1" && known[i, j] == "0")
                            {
                                known[i, j] = "1";
                            }
                        }
                    }
                }
            }
        }
        int xpos = 0, ypos = 0;
        for (int i = 0; i < visible.GetLength(0); i++)
        {
            for (int j = 0; j < visible.GetLength(1); j++)
            {
                if (known[i, j] == "0")
                {
                    Destroy(visible[i, j]);
                    visible[i, j] = Instantiate(Resources.Load("Prefabs/Unexplored") as GameObject, new Vector2(i, j), new Quaternion());
                }
                else
                {
                    Destroy(visible[i, j]);
                    visible[i, j] = Instantiate(Resources.Load("Prefabs/Explored") as GameObject, new Vector2(i, j), new Quaternion());
                }
                xpos += 1;
            }
            xpos = 0;
            ypos += 1;
        }

    }
    public void ShowAllMap()
    {
        selectedview = "";
        for (int i = 0; i < known.GetLength(0); i++)
        {
            for (int j = 0; j < known.GetLength(1); j++)
            {
                known[i, j] = "1";
            }
        }
        int xpos = 0, ypos = 0;
        for (int i = 0; i < visible.GetLength(0); i++)
        {
            for (int j = 0; j < visible.GetLength(1); j++)
            {
                if (known[i, j] == "0")
                {
                    Destroy(visible[i, j]);
                    visible[i, j] = Instantiate(Resources.Load("Prefabs/Unexplored") as GameObject, new Vector2(i, j), new Quaternion());
                }
                else
                {
                    Destroy(visible[i, j]);
                    visible[i, j] = Instantiate(Resources.Load("Prefabs/Explored") as GameObject, new Vector2(i, j), new Quaternion());
                }
                xpos += 1;
            }
            xpos = 0;
            ypos += 1;
        }
    }
    public void ShowSelected(GameObject npc)
    {
        if (npc != null)
        {
            string[,] known = npc.GetComponent<NPC>().knownmap;
            int xpos = 0, ypos = 0;
            for (int i = 0; i < visible.GetLength(0); i++)
            {
                for (int j = 0; j < visible.GetLength(1); j++)
                {
                    if (known[i, j] == "0")
                    {
                        Destroy(visible[i, j]);
                        visible[i, j] = Instantiate(Resources.Load("Prefabs/Unexplored") as GameObject, new Vector2(i, j), new Quaternion());
                    }
                    else
                    {
                        Destroy(visible[i, j]);
                        visible[i, j] = Instantiate(Resources.Load("Prefabs/Explored") as GameObject, new Vector2(i, j), new Quaternion());
                    }
                    xpos += 1;
                }
                xpos = 0;
                ypos += 1;
            }
        }
        else
        {
            return;
        }
    }
    public void Team1Select()
    {
        selectedview = "Team1";
    }
    public void Team2Select()
    {
        selectedview = "Team2";
    }
    IEnumerator RoutineView()
    {
        while (true)
        {
            if (selectedview == "Team1" || selectedview == "Team2")
            {
                ShowExploredAreas(selectedview);
            }
            else if (selectedview == "One")
            {
                ShowSelected(player);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
