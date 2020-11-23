using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Grid_Inspector : MonoBehaviour
{
    public static  int N;
    public static  int M;
    public static int NPC_VILLAGE_1;
    public static int NPC_VILLAGE_2;
    public static int GOLD, WOOD, STONE, BERRIES;
    public static int NEED_GOLD1, NEED_WOOD1, NEED_STONE1, NEED_BERRIES1;
    public static int NEED_GOLD2, NEED_WOOD2, NEED_STONE2, NEED_BERRIES2;
    public static int CURRENT_GOLD1, CURRENT_WOOD1, CURRENT_STONE1, CURRENT_BERRIES1;
    public static int CURRENT_GOLD2, CURRENT_WOOD2, CURRENT_STONE2, CURRENT_BERRIES2;
    public static int TEAM1ENERGY;
    public static int TEAM2ENERGY;
    public static int TEAM1GOLD;
    public static int TEAM2GOLD;
    public static int TEAM1ENERGYTHRESHOLD;
    public static int TEAM2ENERGYTHRESHOLD;
    public static int MAP_PRICE;
    public static int ENERGY_PRICE;
    public static int POTION_ENERGY;
    public static Vector2 village_1_loc;
    public static Vector2 village_2_loc;
    public static List<GameObject> npcs1 = new List<GameObject>();
    public static List<GameObject> npcs2 = new List<GameObject>();
    public static CellObject[,] board;
    public static List<string> NeededResources1;
    public static GameObject pausepanel;
    public static GameObject endpanel;
    public static GameObject gameui;
    public static int Team1EnergyTrades;
    public static int Team2EnergyTrades;
    public static int Team1MapTrades;
    public static int Team2MapTrades;
    public static int CrossTeamEnergyTrades;
    public static int CrossTeamMapTrades;
    public static float time;
    public static float oldtime;
    public static string winner;
    public static int team1potions;
    public static int team2potions;
    public static bool instancerunning = false;
    public static bool savemap = false;
    public static bool usepreloadedmap = false;
    public static char[,] preloadedmap;
    public static string folderpath;

    private void Awake()
    {
        instancerunning = false;
        oldtime = Time.realtimeSinceStartup;
        time = 0f;
        Team1EnergyTrades = 0;
        Team2EnergyTrades = 0;
        Team1MapTrades = 0;
        Team2MapTrades = 0;
        CrossTeamEnergyTrades = 0;
        CrossTeamMapTrades = 0;
        gameui = GameObject.FindGameObjectWithTag("gameui");
        pausepanel = GameObject.FindGameObjectWithTag("pause");
        endpanel=GameObject.FindGameObjectWithTag("gameover");
        pausepanel.SetActive(false);
        endpanel.SetActive(false);
        board = new CellObject[N, M];
        NeededResources1 = RequiredResourcesVillage1();
        CreateGrid();
        Debug.Log("Grid Created");
    }
    void Start()
    {
        CURRENT_GOLD1 = 0;
        CURRENT_WOOD1 = 0;
        CURRENT_STONE1 = 0;
        CURRENT_BERRIES1= 0;
        CURRENT_GOLD2 = 0;
        CURRENT_WOOD2 = 0;
        CURRENT_STONE2 = 0;
        CURRENT_BERRIES2 = 0;
        if (usepreloadedmap)
        {
            GenerateFromPreloadedMap();
            Spawn_Npcs();
            Debug.Log("NPCS Created");
        }
        else
        {
            Generate_First_Village();
            Debug.Log("Firstd Created");
            Generate_Second_Village();
            Debug.Log("Second Created");
            Spawn_Npcs();
            Debug.Log("NPCS Created");
            Generate_Resources();
            Debug.Log("Resources Created");
        }
        for(int i=0; i <npcs1.Count;i++)
        {
            npcs1[i].GetComponent<NPC>().StartNPC();
        }
        for (int i = 0; i < npcs2.Count; i++)
        {
            npcs2[i].GetComponent<NPC>().StartNPC();
        }
        if (savemap)
        {
            StreamWriter OSW;
            folderpath += "/SavedMap.txt";
            using (OSW = new StreamWriter(folderpath))
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                       OSW.Write(board[j,i].type);
                    }
                    OSW.WriteLine();
                }
            }
        }
    }

    /// <summary>
    /// Creates a grid with grass.
    /// </summary>
    public void CreateGrid()
    {
        Sprite grass = Resources.Load<Sprite>("sprites/medievalTile_57");
        int xpos = 0, ypos = 0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
               // Debug.Log(i + " " + j);
                board[i, j] = new CellObject(xpos, ypos, "grass");
                board[i, j].type = "R";
                Instantiate(Resources.Load("Prefabs/Grass") as GameObject, new Vector2(xpos,ypos),new Quaternion());
                ypos += 1;
            }
            ypos = 0;
            xpos += 1;

        }

        
    }
    /// <summary>
    /// Loads and generates the user loaded map.
    /// </summary>
    public void GenerateFromPreloadedMap()
    {

        GameObject gold_sprite = (Resources.Load("Prefabs/Gold_Ore") as GameObject);
        GameObject forest_sprite = (Resources.Load("Prefabs/Forest") as GameObject);
        GameObject stone_sprite = (Resources.Load("Prefabs/Stone") as GameObject);
        GameObject berries_sprite = (Resources.Load("Prefabs/Berries") as GameObject);
        GameObject left1 = (Resources.Load("Prefabs/Left_House_1") as GameObject);
        GameObject right1 = (Resources.Load("Prefabs/Right_House_1") as GameObject);
        GameObject top1 = (Resources.Load("Prefabs/Top_House_1") as GameObject);
        GameObject bottom1 = (Resources.Load("Prefabs/Bottom_House_1") as GameObject);
        GameObject statue1 = (Resources.Load("Prefabs/Statue") as GameObject);
        GameObject left2 = (Resources.Load("Prefabs/Left_House_2") as GameObject);
        GameObject right2 = (Resources.Load("Prefabs/Right_House_2") as GameObject);
        GameObject top2 = (Resources.Load("Prefabs/Top_House_2") as GameObject);
        GameObject bottom2 = (Resources.Load("Prefabs/Bottom_House_2") as GameObject);
        GameObject statue2 = (Resources.Load("Prefabs/Statue") as GameObject);
        int count1 = 0;
        int count2 = 0;
        for(int i =0; i < preloadedmap.GetLength(0); i++)
        {
            for(int j = 0; j < preloadedmap.GetLength(1); j++)
            {
                 if (preloadedmap[i, j].Equals('F'))
                {
                    Instantiate(forest_sprite, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = forest_sprite;
                    board[i, j].type = "F";
                }
                else if (preloadedmap[i, j].Equals('G'))
                {
                    Instantiate(gold_sprite, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = gold_sprite;
                    board[i, j].type = "G";
                }
                else if (preloadedmap[i, j].Equals('S'))
                {
                    Instantiate(stone_sprite, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = stone_sprite;
                    board[i, j].type = "S";
                }
                else if (preloadedmap[i, j].Equals('B'))
                {
                    Instantiate(berries_sprite, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = berries_sprite;
                    board[i, j].type = "B";
                }
                else if (preloadedmap[i, j].Equals('1')&&count1==0)
                {
                    Instantiate(top1, new Vector2(i, j), new Quaternion());
                    board[i,j].contain = top1;
                    board[i, j].type = "1";
                    count1++;
                }
                else if (preloadedmap[i, j].Equals('1') && count1 == 1)
                {
                    Instantiate(left1, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = left1;
                    board[i, j].type = "1";
                    count1++;
                }
                else if (preloadedmap[i, j].Equals('1') && count1 == 2)
                {
                    Instantiate(statue1, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = statue1;
                    board[i, j].type = "1";
                    count1++;
                    village_1_loc = new Vector2(i, j);
                }
                else if (preloadedmap[i, j].Equals('1') && count1 == 3)
                {
                    Instantiate(right1, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = right1;
                    board[i, j].type = "1";
                    count1++;
                }
                else if (preloadedmap[i, j].Equals('1') && count1 == 4)
                {
                    Instantiate(bottom1, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = bottom1;
                    board[i, j].type = "1";
                    count1++;
                }
                else if (preloadedmap[i, j].Equals('2') && count2 == 0)
                {
                    Instantiate(top2, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = top2;
                    board[i, j].type = "2";
                    count2++;
                }
                else if (preloadedmap[i, j].Equals('2') && count2 == 1)
                {
                    Instantiate(left2, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = left2;
                    board[i, j].type = "2";
                    count2++;
                }
                else if (preloadedmap[i, j].Equals('2') && count2 == 2)
                {
                    Instantiate(statue2, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = statue2;
                    board[i, j].type = "2";
                    count2++;
                    village_2_loc = new Vector2(i, j);
                }
                else if (preloadedmap[i, j].Equals('2') && count2 == 3)
                {
                    Instantiate(right2, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = right2;
                    board[i, j].type = "2";
                    count2++;
                }
                else if (preloadedmap[i, j].Equals('2') && count2 == 4)
                {
                    Instantiate(bottom2, new Vector2(i, j), new Quaternion());
                    board[i, j].contain = bottom2;
                    board[i, j].type = "2";
                    count2++;
                }
            }
        }


    }
    /// <summary>
    /// Randomly generates the resources on the map.
    /// </summary>
    public void Generate_Resources()
    {
        GameObject gold_sprite = (Resources.Load("Prefabs/Gold_Ore") as GameObject);
        for (int i = 0; i < GOLD; i++)
        {
          
            int y_pos = UnityEngine.Random.Range(0, N);
            int x_pos = UnityEngine.Random.Range(0, M);
            while(board[y_pos, x_pos].contain != null)
            {
                 y_pos = UnityEngine.Random.Range(0, N);
                 x_pos = UnityEngine.Random.Range(0, M);
            }
            Instantiate(gold_sprite, new Vector2(y_pos, x_pos),new Quaternion());
            board[y_pos, x_pos].contain = gold_sprite;
            board[y_pos, x_pos].type = "G";
        }
        GameObject forest_sprite = (Resources.Load("Prefabs/Forest") as GameObject);
        for (int i = 0; i < WOOD; i++)
        {

            int y_pos = UnityEngine.Random.Range(0, N);
            int x_pos = UnityEngine.Random.Range(0, M);
            while (board[y_pos, x_pos].contain != null )
            {
                  y_pos = UnityEngine.Random.Range(0, N);
                  x_pos = UnityEngine.Random.Range(0, M);
            }
            Instantiate(forest_sprite, new Vector2(y_pos, x_pos), new Quaternion());
            board[y_pos, x_pos].contain = forest_sprite;
            board[y_pos, x_pos].type = "F";
        }
        
        GameObject stone_sprite = (Resources.Load("Prefabs/Stone") as GameObject);
        for (int i = 0; i < STONE; i++)
        {

            int y_pos = UnityEngine.Random.Range(0, N);
            int x_pos = UnityEngine.Random.Range(0, M);
            while (board[y_pos, x_pos].contain != null)
            {
                y_pos = UnityEngine.Random.Range(0, N);
                x_pos = UnityEngine.Random.Range(0, M);
            }
            Instantiate(stone_sprite, new Vector2(y_pos, x_pos), new Quaternion());
            board[y_pos, x_pos].contain = stone_sprite;
            board[y_pos, x_pos].type = "S";
        }
        GameObject berries_sprite = (Resources.Load("Prefabs/Berries") as GameObject);
        for (int i = 0; i < BERRIES; i++)
        {

            int y_pos = UnityEngine.Random.Range(0, N);
            int x_pos = UnityEngine.Random.Range(0, M);
            while (board[y_pos, x_pos].contain != null )
            {
                y_pos = UnityEngine.Random.Range(0, N);
                x_pos = UnityEngine.Random.Range(0, M);
            }
            Instantiate(berries_sprite, new Vector2(y_pos, x_pos), new Quaternion());
            board[y_pos, x_pos].contain = berries_sprite;
            board[y_pos, x_pos].type = "B";
        }
    }
    /// <summary>
    /// Randomly generates the first village.
    /// </summary>
    public void Generate_First_Village()
    {   
        GameObject left = (Resources.Load("Prefabs/Left_House_1") as GameObject);
        GameObject right = (Resources.Load("Prefabs/Right_House_1") as GameObject);
        GameObject top = (Resources.Load("Prefabs/Top_House_1") as GameObject);
        GameObject bottom = (Resources.Load("Prefabs/Bottom_House_1") as GameObject);
        GameObject statue = (Resources.Load("Prefabs/Statue") as GameObject);
            int y_pos = UnityEngine.Random.Range(1, N-1);
            int x_pos = UnityEngine.Random.Range(1, M-1);
            while (board[y_pos, x_pos].contain != null || board[y_pos-1, x_pos].contain != null || board[y_pos+1, x_pos].contain != null || board[y_pos, x_pos-1].contain != null || board[y_pos, x_pos+1].contain != null)
            {
                y_pos = UnityEngine.Random.Range(4, N-4);
                x_pos = UnityEngine.Random.Range(4, M-4);
            }
        Instantiate(statue, new Vector2(y_pos, x_pos), new Quaternion());
        board[y_pos, x_pos].contain = statue;
        board[y_pos, x_pos].type = "1";
        Instantiate(right, new Vector2(y_pos+1, x_pos), new Quaternion());
        board[y_pos+1, x_pos].contain = right;
        board[y_pos + 1, x_pos].type = "1";
        Instantiate(left, new Vector2(y_pos-1, x_pos), new Quaternion());
        board[y_pos-1, x_pos].contain = left;
        board[y_pos - 1, x_pos].type = "1";
        Instantiate(bottom, new Vector2(y_pos, x_pos-1), new Quaternion());
        board[y_pos, x_pos-1].contain = bottom;
        board[y_pos, x_pos - 1].type = "1";
        Instantiate(top, new Vector2(y_pos, x_pos+1), new Quaternion());
        board[y_pos, x_pos+1].contain = top;
        board[y_pos, x_pos + 1].type = "1";
        village_1_loc = new Vector2(y_pos,x_pos);
    }

    /// <summary>
    /// Randomly generates the second village.
    /// </summary>
    public void Generate_Second_Village()
    {
        GameObject left = (Resources.Load("Prefabs/Left_House_2") as GameObject);
        GameObject right = (Resources.Load("Prefabs/Right_House_2") as GameObject);
        GameObject top = (Resources.Load("Prefabs/Top_House_2") as GameObject);
        GameObject bottom = (Resources.Load("Prefabs/Bottom_House_2") as GameObject);
        GameObject statue = (Resources.Load("Prefabs/Statue") as GameObject);
        int y_pos = UnityEngine.Random.Range(1, N - 1);
        int x_pos = UnityEngine.Random.Range(1, M -1);
        while (board[y_pos, x_pos].contain != null || board[y_pos - 1, x_pos].contain != null || board[y_pos + 1, x_pos].contain != null || board[y_pos, x_pos - 1].contain != null || board[y_pos, x_pos + 1].contain != null||(Math.Abs(village_1_loc.x-x_pos)<5&&Math.Abs(village_1_loc.y - y_pos) < 5))
        {
            y_pos = UnityEngine.Random.Range(1, N - 1);
            x_pos = UnityEngine.Random.Range(1, M - 1);
        }
        Instantiate(statue, new Vector2(y_pos, x_pos), new Quaternion());
        board[y_pos, x_pos].contain = statue;
        board[y_pos, x_pos].type = "2";
        Instantiate(right, new Vector2(y_pos + 1, x_pos), new Quaternion());
        board[y_pos + 1, x_pos].contain = right;
        board[y_pos + 1, x_pos].type = "2";
        Instantiate(left, new Vector2(y_pos - 1, x_pos), new Quaternion());
        board[y_pos - 1, x_pos].contain = left;
        board[y_pos - 1, x_pos].type = "2";
        Instantiate(bottom, new Vector2(y_pos, x_pos - 1), new Quaternion());
        board[y_pos, x_pos - 1].contain = bottom;
        board[y_pos, x_pos - 1].type = "2";
        Instantiate(top, new Vector2(y_pos, x_pos + 1), new Quaternion());
        board[y_pos, x_pos + 1].contain = top;
        board[y_pos, x_pos + 1].type = "2";
        village_2_loc = new Vector2(y_pos, x_pos);

    }
    /// <summary>
    /// Spawn villages near their team village.
    /// </summary>
    public void Spawn_Npcs()
    {
        GameObject villager_male1 = (Resources.Load("Prefabs/Villager_Male_1") as GameObject);
        GameObject villager_female1 = (Resources.Load("Prefabs/Villager_Female_1") as GameObject);
        GameObject villager_male2 = (Resources.Load("Prefabs/Villager_Male_2") as GameObject);
        GameObject villager_female2 = (Resources.Load("Prefabs/Villager_Female_2") as GameObject);
        List<Vector2> available_places_village1 = new List<Vector2>();
        if (village_1_loc.y + 2 < N)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village1.Add(new Vector2(village_1_loc.x - 1 + i, village_1_loc.y + 2));
            }
        }
        if (village_1_loc.y - 2 >= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village1.Add(new Vector2(village_1_loc.x - 1 + i, village_1_loc.y - 2));
            }
        }
        if (village_1_loc.x - 2 >= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village1.Add(new Vector2(village_1_loc.x - 2, village_1_loc.y - 1 + i));
            }
        }
        if (village_1_loc.x + 2 < M)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village1.Add(new Vector2(village_1_loc.x + 2, village_1_loc.y - 1 + i));
            }

        }
        if (village_1_loc.x - 2 > 0 && village_1_loc.y + 2 < N)
        {
            available_places_village1.Add(new Vector2(village_1_loc.x - 2, village_1_loc.y + 2));
        }
        if (village_1_loc.x - 2 > 0 && village_1_loc.y - 2 > 0)
        {
            available_places_village1.Add(new Vector2(village_1_loc.x - 2, village_1_loc.y - 2));
        }
        if (village_1_loc.x + 2 > M && village_1_loc.y + 2 < N)
        {
            available_places_village1.Add(new Vector2(village_1_loc.x + 2, village_1_loc.y + 2));
        }
        if (village_1_loc.x + 2 > M && village_1_loc.y - 2 > 0)
        {
            available_places_village1.Add(new Vector2(village_1_loc.x + 2, village_1_loc.y - 2));
        }

        List<Vector2> available_places_village2 = new List<Vector2>();
        if (village_2_loc.y + 2 < N)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village2.Add(new Vector2(village_2_loc.x - 1 + i, village_2_loc.y + 2));
            }
        }
        if (village_2_loc.y - 2 >= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village2.Add(new Vector2(village_2_loc.x - 1 + i, village_2_loc.y - 2));
            }
        }
        if (village_2_loc.x - 2 >= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village2.Add(new Vector2(village_2_loc.x - 2, village_2_loc.y - 1 + i));
            }
        }
        if (village_2_loc.x + 2 < M)
        {
            for (int i = 0; i < 3; i++)
            {
                available_places_village2.Add(new Vector2(village_2_loc.x + 2, village_2_loc.y - 1 + i));
            }

        }
        if (village_2_loc.x - 2 > 0 && village_2_loc.y + 2 < N)
        {
            available_places_village2.Add(new Vector2(village_2_loc.x - 2, village_2_loc.y + 2));
        }
        if (village_2_loc.x - 2 > 0 && village_2_loc.y - 2 > 0)
        {
            available_places_village2.Add(new Vector2(village_2_loc.x - 2, village_2_loc.y - 2));
        }
        if (village_2_loc.x + 2 > M && village_2_loc.y + 2 < N)
        {
            available_places_village2.Add(new Vector2(village_2_loc.x + 2, village_2_loc.y + 2));
        }
        if (village_2_loc.x + 2 > M && village_2_loc.y - 2 > 0)
        {
            available_places_village2.Add(new Vector2(village_2_loc.x + 2, village_2_loc.y - 2));
        }
        for(int i = 0; i < NPC_VILLAGE_1; i++)
        {
            if (available_places_village1.Count > 0)
            {
                int gender = UnityEngine.Random.Range(0,2);
               Vector2 npc_loc= available_places_village1[i];
                GameObject gm;
                if (gender == 0)
                {
                     gm = Instantiate(villager_female1, npc_loc, new Quaternion());
                    gm.GetComponent<NPC>().team = "Team1";
                    board[(int)npc_loc.x, (int)npc_loc.y].contain = villager_female1;
                    
                }
                else
                {
                     gm = Instantiate(villager_male1, npc_loc, new Quaternion());
                    gm.GetComponent<NPC>().team = "Team1";
                    board[(int)npc_loc.x, (int)npc_loc.y].contain = villager_male1;
                }
                gm.GetComponent<NPC>().startingEnergy = TEAM1ENERGY;
                gm.GetComponent<NPC>().threshold = TEAM1ENERGYTHRESHOLD;
                gm.GetComponent<NPC>().potionenergy = POTION_ENERGY;
                gm.GetComponent<NPC>().gold = TEAM1GOLD;
                npcs1.Add(gm);
            }
            else
            {
                break;
            }
        }
        for (int i = 0; i < NPC_VILLAGE_2; i++)
        {
            if (available_places_village2.Count > 0)
            {
                int gender = UnityEngine.Random.Range(0, 2);
                Vector2 npc_loc = available_places_village2[i];
                GameObject gm;
                if (gender == 0)
                {
                     gm = Instantiate(villager_female2, npc_loc, new Quaternion());
                    gm.GetComponent<NPC>().team = "Team2";
                    board[(int)npc_loc.x, (int)npc_loc.y].contain = villager_female2;
                }
                else
                {
                     gm = Instantiate(villager_male2, npc_loc, new Quaternion());
                    gm.GetComponent<NPC>().team = "Team2";
                    board[(int)npc_loc.x, (int)npc_loc.y].contain = villager_male2;
                }
                gm.GetComponent<NPC>().startingEnergy = TEAM2ENERGY;
                gm.GetComponent<NPC>().threshold = TEAM2ENERGYTHRESHOLD;
                gm.GetComponent<NPC>().potionenergy = POTION_ENERGY;
                gm.GetComponent<NPC>().gold = TEAM2GOLD;
                npcs2.Add(gm);
            }
            else
            {
                break;
            }
        }
    }
    /// <summary>
    /// Assignes roles to the villagers of team1.
    /// </summary>
    /// <returns></returns>
    List<string> RequiredResourcesVillage1()
    {
        List<string> allresources = new List<string> ();
        List<string> returneddistrubuted = new List<string>();
        if (NEED_GOLD1 > 0)
        {
            returneddistrubuted.Add("G");
            allresources.Add("G");
        }
        if (NEED_STONE1>0)
        {
            returneddistrubuted.Add("S");
            allresources.Add("S");
        }
        if (NEED_WOOD1 > 0)
        {
            returneddistrubuted.Add("F");
            allresources.Add("F");
        }
        if (NEED_BERRIES1 > 0)
        {
            returneddistrubuted.Add("B");
            allresources.Add("B");
        }
        while (returneddistrubuted.Count < NPC_VILLAGE_1)
        {
            returneddistrubuted.Add(allresources[UnityEngine.Random.Range(0, 3)]);
        }
        return returneddistrubuted;
    }
}
