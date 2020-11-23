using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Resources.Scripts;
using System.Threading.Tasks;

public class NPC : MonoBehaviour
{
    public GameObject FloatingText;
    public GameObject EnergyText;
    public List<CellObject> adjacentiles = new List<CellObject>();
    public string team;
    public  int gold;
    public bool tradeavailable = true;
    public int potionenergy;
    public int energy;
    public int startingEnergy;
    public int threshold;
    bool holds = false;
    public Sprite explored;
    public Sprite unexplored;
    public Sprite visible1;
    bool movingtovillage = false;
    string holded_resource;
    public string[,] knownmap;
    GameObject[,] visible;
    public bool clicked = false;
    string prev_resource = "";
    string find_resource = "";
    public bool dead = false;
    bool assigned_resource = false;
    bool exploring = true;
    public bool lookforenergy = false;
    bool alreadyfound = false;

    public async void StartNPC()
    {
        InstantiateMap(Grid_Inspector.board.GetLength(0), Grid_Inspector.board.GetLength(1));
        energy = startingEnergy;
        Check_Explored();
        if (team.Equals("Team1"))
        {
            while (true)
            {
                await NPCBehaviour();
                if (dead)
                {
                    break;
                }
                if(find_resource=="G"&& Grid_Inspector.CURRENT_GOLD1 >= Grid_Inspector.NEED_GOLD1)
                {
                    break;
                }
                else if (find_resource == "F" && Grid_Inspector.CURRENT_WOOD1 >= Grid_Inspector.NEED_WOOD1)
                {
                    break;
                }
                else if (find_resource == "S" && Grid_Inspector.CURRENT_STONE1 >= Grid_Inspector.NEED_STONE1)
                {
                    break;
                }
                else if (find_resource == "B" && Grid_Inspector.CURRENT_BERRIES1 >= Grid_Inspector.NEED_BERRIES1)
                {
                    break;
                }
            }
        }
        else
        {
            while (true)
            {
                await NPCBehaviour();
                if (dead)
                {
                    break;
                }
                if (Grid_Inspector.CURRENT_GOLD2 >= Grid_Inspector.NEED_GOLD2 && Grid_Inspector.CURRENT_WOOD2 >= Grid_Inspector.NEED_WOOD2 && Grid_Inspector.CURRENT_STONE2 >= Grid_Inspector.NEED_STONE2 && Grid_Inspector.CURRENT_BERRIES2 >= Grid_Inspector.NEED_BERRIES2)
                {
                    break;
                }
            }

        }
        if (team.Equals("Team1"))
        {
            Grid_Inspector.npcs1.Remove(this.gameObject);
        }
        else
        {
            Grid_Inspector.npcs2.Remove(this.gameObject);
        }
        Destroy(this.gameObject);
    }

   
    /// <summary>
    /// Reset the visible areas.
    /// </summary>
    public void resetmap()
    {
        for (int i = 0; i < Grid_Inspector.board.GetLength(0); i++)
        {
            for (int j = 0; j < Grid_Inspector.board.GetLength(1); j++)
            {
                Destroy(visible[i, j]);
               
            }
        }
    }
    /// <summary>
    /// Show the explored areas.
    /// </summary>
    public void Showexplored()
    {
        if (clicked)
        {
            int xpos = 0, ypos = 0;
            for (int i = 0; i < knownmap.GetLength(0); i++)
            {
                for (int j = 0; j < knownmap.GetLength(1); j++)
                {
                    if (knownmap[i, j] == "0")
                    {
                        Destroy(visible[i, j]);
                        visible[i,j]= Instantiate(Resources.Load("Prefabs/Unexplored") as GameObject, new Vector2(i, j), new Quaternion());
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
    }
    /// <summary>
    /// Reveal newly explored areas.
    /// </summary>
    public void Check_Explored()
    {
        knownmap[(int)this.transform.position.x, (int)this.transform.position.y] = "1";
        if (this.transform.position.x - 1 >= 0)
        {

            knownmap[(int)this.transform.position.x - 1, (int)this.transform.position.y] = "1";
            if (this.transform.position.y + 1 < knownmap.GetLength(1))
            {

                knownmap[(int)this.transform.position.x - 1, (int)this.transform.position.y + 1] = "1";

            }
            if (this.transform.position.y - 1 >= 0)
            {


                knownmap[(int)this.transform.position.x - 1, (int)this.transform.position.y - 1] = "1";

            }
        }
        if (this.transform.position.x + 1 < knownmap.GetLength(0))
        {
            knownmap[(int)this.transform.position.x + 1, (int)this.transform.position.y] = "1";

            if (this.transform.position.y + 1 < knownmap.GetLength(1))
            {

                knownmap[(int)this.transform.position.x + 1, (int)this.transform.position.y + 1] = "1";
            }
            if (this.transform.position.y - 1 >=0)
            {

                knownmap[(int)this.transform.position.x + 1, (int)this.transform.position.y - 1] = "1";
            }
        }
        if (this.transform.position.y - 1 >= 0)
        {

            knownmap[(int)this.transform.position.x, (int)this.transform.position.y - 1] = "1";
        }
        if (this.transform.position.y + 1 < knownmap.GetLength(1))
        {

            knownmap[(int)this.transform.position.x, (int)this.transform.position.y + 1] = "1";

        }
    }
    /// <summary>
    /// Instantiate the visible map.
    /// </summary>
    /// <param name="N"></param>
    /// <param name="M"></param>
    void InstantiateMap(int N, int M)
    {
        knownmap = new string[N, M];
        visible = new GameObject[N, M];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                visible[i,j]= Instantiate(Resources.Load("Prefabs/Visible") as GameObject, new Vector2(i, j), new Quaternion());
                knownmap[i, j] = "0";
            }
        }
    }
    /// <summary>
    /// Move to a certain point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    async Task<bool> Move_To_Point(Vector2 point)
    {

        AStar npcpath = new AStar();
        Stack<AStar.Node> moves = npcpath.starimplementation(this.transform.position, point);
        while (true)
        {
            if (Time.timeScale != 0)
             {

                if (movingtovillage)
                {
                    ShowReturningText(holded_resource);
                }
                else if (exploring)
                {
                    ShowSearchingText(find_resource);
                }
                else if (alreadyfound)
                {
                    ShowMovingText(find_resource);
                }
                Vector2 move;
                if (moves.Count > 0)
                {
                    move = moves.Pop().location;
                }
                else
                {
                    move = new Vector2(0, 0);
                }
                Grid_Inspector.board[(int)transform.position.x, (int)transform.position.y].contain = null;
                this.transform.position = move;
                tradeavailable = true;
                GetAdjacentTiles();
                if (Grid_Inspector.board[(int)transform.position.x, (int)transform.position.y].contain != null)
                {
                    if (Grid_Inspector.board[(int)transform.position.x, (int)transform.position.y].contain.name == "Energy_Potion(Clone)")
                    {
                        alreadyfound = false;
                        energy += potionenergy;
                        Grid_Inspector.board[(int)transform.position.x, (int)transform.position.y].type = "";
                        Destroy(Grid_Inspector.board[(int)transform.position.x, (int)transform.position.y].contain);
                        if (team == "Team1")
                        {
                            Grid_Inspector.team1potions++;
                        }
                        else
                        {
                            Grid_Inspector.team2potions++;
                        }
                    }
                }
                if (lookforenergy && !IsEnergyLow())
                {
                    lookforenergy = false;
                    return true;
                }
                if (lookforenergy && alreadyfound)
                {
                    if (Grid_Inspector.board[(int)point.x, (int)point.y].type != "E")
                    {
                        return false;
                    }
                }
                Grid_Inspector.board[(int)transform.position.x, (int)transform.position.y].contain = this;
                if (move == point)
                {
                    alreadyfound = false;
                    if (movingtovillage)
                    {
                        UnloadResource();
                        energy--;
                        Checkifdeadorfinished();
                        ShowEnergy();
                        if (dead)
                        {
                            return false;
                        }
                        await Task.Delay(1000);
                    }
                    return true;
                }

                Check_Explored();
                Showexplored();
                energy--;
                ShowEnergy();
                Checkifdeadorfinished();
                if (!lookforenergy)
                {
                    if (IsEnergyLow())
                    {
                        await Task.Delay(1000);
                        await FindEnergy();
                        moves = npcpath.starimplementation(this.transform.position, point);
                    }

                }

                if (dead)
                {
                    return false;
                }
                if (exploring && FindExploredResource(find_resource).x != -1)
                {
                    alreadyfound = true;
                    exploring = false;
                    return true;
                }

                await Task.Delay(1000);
            }
            else
            {
                while (Time.timeScale == 0)
                {
                    await Task.Delay(1000);
                }
            }
        }

    }
    /// <summary>
    /// Select resource for each villager, Team 2 = Random Resource - Team 1 = Predefined Resource.
    /// </summary>
    /// <returns></returns>
    string SelectResource()
    {
        if (team.Equals("Team2")){
            List<string> neededresources = new List<string>();
            if (Grid_Inspector.CURRENT_GOLD2 < Grid_Inspector.NEED_GOLD2)
            {
                neededresources.Add("G");
            }
            if (Grid_Inspector.CURRENT_BERRIES2 < Grid_Inspector.NEED_BERRIES2)
            {
                neededresources.Add("B");
            }
            if (Grid_Inspector.CURRENT_STONE2 < Grid_Inspector.NEED_STONE2)
            {
                neededresources.Add("S");
            }
            if (Grid_Inspector.CURRENT_WOOD2 < Grid_Inspector.NEED_WOOD2)
            {
                neededresources.Add("F");
            }
            int sel = UnityEngine.Random.Range(0, neededresources.Count);
            return neededresources[sel];

        }
        else
        {
            if (assigned_resource)
            {
                return find_resource;
            }
            else
            {
                string resource = Grid_Inspector.NeededResources1.First();
                Grid_Inspector.NeededResources1.RemoveAt(0);
                assigned_resource = true;
                return resource;
                
            }
        }
    }
    /// <summary>
    /// Return the resource location if it exists in the explored map.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    Vector2 FindExploredResource(string resource)
    {
        List<Vector2> locations = new List<Vector2>();
        List<float> distances = new List<float>();
        for(int i = 0; i < Grid_Inspector.board.GetLength(0); i++)
        {
            for (int j = 0; j < Grid_Inspector.board.GetLength(1); j++)
            {
                if (Grid_Inspector.board[i, j].type == resource && knownmap[i, j] == "1")
                {
                    locations.Add(new Vector2(i, j));
                    distances.Add(Vector2.Distance(this.transform.position, new Vector2(i, j)));
                    
                }
            }
        }
        if (locations.Count > 0)
        {
            alreadyfound = true;
            Vector2[] loc = locations.ToArray();
            float[] dist = distances.ToArray();
            System.Array.Sort(dist, loc);
            return loc.First();
        }
        else
        {
            return new Vector2(-1, -1);
        }

    }
    /// <summary>
    /// Main NPC Behaviour
    /// </summary>
    /// <returns></returns>
    async Task<bool> NPCBehaviour()
    {
        find_resource = SelectResource();
        Vector2 resfound = FindExploredResource(find_resource);
        while(resfound.x == -1)
        {
            await ExploreForResource();
            resfound = FindExploredResource(find_resource);
            if (dead)
            {
                return false;
            }
        }
        await Move_To_Point(resfound);
        if (dead)
        {
            return false;
        }
        if (find_resource != "E")
        {
            await CollectResource();
            await BackToVillage();
        }
        return true;
    }
    /// <summary>
    /// Return back to vilage after collecting the resource.
    /// </summary>
    /// <returns></returns>
    async Task<bool> BackToVillage()
    {
        float[] distances = new float[4];
        Vector2[] locations = new Vector2[4];
        Vector2 pickedloc;
        if (team.Equals("Team1"))
        {
            distances[0] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_1_loc.x, Grid_Inspector.village_1_loc.y + 1));
            locations[0] = new Vector2(Grid_Inspector.village_1_loc.x, Grid_Inspector.village_1_loc.y + 1);
            distances[1] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_1_loc.x, Grid_Inspector.village_1_loc.y - 1));
            locations[1] = new Vector2(Grid_Inspector.village_1_loc.x, Grid_Inspector.village_1_loc.y - 1);
            distances[2] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_1_loc.x + 1, Grid_Inspector.village_1_loc.y));
            locations[2] = new Vector2(Grid_Inspector.village_1_loc.x + 1, Grid_Inspector.village_1_loc.y);
            distances[3] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_1_loc.x - 1, Grid_Inspector.village_1_loc.y));
            locations[3] = new Vector2(Grid_Inspector.village_1_loc.x - 1, Grid_Inspector.village_1_loc.y);
            System.Array.Sort(distances, locations);
            pickedloc = locations.First();
        }
        else
        {
            distances[0] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_2_loc.x, Grid_Inspector.village_2_loc.y + 1));
            locations[0] = new Vector2(Grid_Inspector.village_2_loc.x, Grid_Inspector.village_2_loc.y + 1);
            distances[1] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_2_loc.x, Grid_Inspector.village_2_loc.y - 1));
            locations[1] = new Vector2(Grid_Inspector.village_2_loc.x, Grid_Inspector.village_2_loc.y - 1);
            distances[2] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_2_loc.x + 1, Grid_Inspector.village_2_loc.y));
            locations[2] = new Vector2(Grid_Inspector.village_2_loc.x + 1, Grid_Inspector.village_2_loc.y);
            distances[3] = Vector2.Distance(this.transform.position, new Vector2(Grid_Inspector.village_2_loc.x - 1, Grid_Inspector.village_2_loc.y));
            locations[3] = new Vector2(Grid_Inspector.village_2_loc.x - 1, Grid_Inspector.village_2_loc.y);
            System.Array.Sort(distances, locations);
            pickedloc = locations.First();
        }
        movingtovillage = true;
        await Move_To_Point(pickedloc);
        if (dead)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Collect the found resource.
    /// </summary>
    /// <returns></returns>
    async Task<bool> CollectResource()
    {
        if (FloatingText)
        {
            ShowCollectingText(find_resource);
        }
        holds = true;
        energy--;
        Checkifdeadorfinished();
        holded_resource = find_resource;
        await Task.Delay(1000); 
        if (dead)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    ///Search for the assigned resource.
    /// </summary>
    /// <returns></returns>
    async Task<bool> ExploreForResource()
    {
        int x, y;
        do
        {
            x = Random.Range(0, knownmap.GetLength(0) - 1);
            y = Random.Range(0, knownmap.GetLength(1) - 1);

        } while (knownmap[x, y] != "0");

        Vector2 point = new Vector2(x, y);
        AStar npcpath = new AStar();
        exploring = true;
        await Move_To_Point(point);
        return true;
    }
    /// <summary>
    /// Check if run out of energy or finished collecting resources.
    /// </summary>
    void Checkifdeadorfinished()
    {
        if (energy <= 0)
        {
            
            dead = true;
            return;
        }
        if (team.Equals("Team1"))
        {

            if (find_resource == "G" && Grid_Inspector.CURRENT_GOLD1 >= Grid_Inspector.NEED_GOLD1)
            {
                dead = true;
                return;
            }
            else if (find_resource == "F" && Grid_Inspector.CURRENT_WOOD1 >= Grid_Inspector.NEED_WOOD1)
            {
                dead = true;
                return;
            }
            else if (find_resource == "S" && Grid_Inspector.CURRENT_STONE1 >= Grid_Inspector.NEED_STONE1)
            {
                dead = true;
                return;
            }
            else if (find_resource == "B" && Grid_Inspector.CURRENT_BERRIES1 >= Grid_Inspector.NEED_BERRIES1)
            {
                dead = true;
                return;
            }
        }
        else
        {

            if (Grid_Inspector.CURRENT_GOLD2 >= Grid_Inspector.NEED_GOLD2 && Grid_Inspector.CURRENT_WOOD2 >= Grid_Inspector.NEED_WOOD2 && Grid_Inspector.CURRENT_STONE2 >= Grid_Inspector.NEED_STONE2 && Grid_Inspector.CURRENT_BERRIES2 >= Grid_Inspector.NEED_BERRIES2)
            {
                dead = true;
                return;
            }
        }
        
    }
    /// <summary>
    /// Shows the search for resource text.
    /// </summary>
    /// <param name="resource"></param>
    void ShowSearchingText(string resource)
    {
        GameObject txt = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (resource == "G")
        {
            txt.GetComponent<TextMesh>().text += "Searching for Gold...";
        }
        else if (resource == "F")
        {
            txt.GetComponent<TextMesh>().text += "Searching for Wood...";
        }
        else if (resource == "B")
        {
            txt.GetComponent<TextMesh>().text += "Searching for Berries...";
        }
        else if (resource == "S")
        {
            txt.GetComponent<TextMesh>().text += "Searching for Stone...";
        }
        else if (resource == "E")
        {
            txt.GetComponent<TextMesh>().text += "Searching for Energy...";
        }
    }
    /// <summary>
    /// Shows text if moving to already known location of a resource.
    /// </summary>
    /// <param name="resource"></param>
    void ShowMovingText(string resource)
    {
        GameObject txt = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (resource == "G")
        {
            txt.GetComponent<TextMesh>().text += "Moving to Gold...";
        }
        else if (resource == "F")
        {
            txt.GetComponent<TextMesh>().text += "Moving to Wood...";
        }
        else if (resource == "B")
        {
            txt.GetComponent<TextMesh>().text += "Moving to Berries...";
        }
        else if (resource == "S")
        {
            txt.GetComponent<TextMesh>().text += "Moving to Stone...";
        }
        else if (resource == "E")
        {
            txt.GetComponent<TextMesh>().text += "Moving to Energy...";
        }
    }
    /// <summary>
    /// Shows the collecting resource text.
    /// </summary>
    /// <param name="resource"></param>
    void ShowCollectingText(string resource)
    {
        GameObject txt=Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (resource == "G")
        {
            txt.GetComponent<TextMesh>().text += "Collecting Gold...";
        }
        else if (resource == "F")
        {
            txt.GetComponent<TextMesh>().text += "Collecting Wood...";
        }
        else if (resource == "B")
        {
            txt.GetComponent<TextMesh>().text += "Collecting Berries...";
        }
        else if (resource == "S")
        {
            txt.GetComponent<TextMesh>().text += "Collecting Stone...";
        }
    }
    /// <summary>
    /// Shows the returning to village text.
    /// </summary>
    /// <param name="resource"></param>
    void ShowReturningText(string resource)
    {
        GameObject txt = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (resource == "G")
        {
            txt.GetComponent<TextMesh>().text += "Returning To Village\n Holding Gold...";
        }
        else if (resource == "F")
        {
            txt.GetComponent<TextMesh>().text += "Returning To Village\n Holding Wood...";
        }
        else if (resource == "B")
        {
            txt.GetComponent<TextMesh>().text += "Returning To Village\n Holding Berries...";
        }
        else if (resource == "S")
        {
            txt.GetComponent<TextMesh>().text += "Returning To Village\n Holding Stone...";
        }
    }
    /// <summary>
    /// Shows the current energy.
    /// </summary>
    void ShowEnergy()
    {
            GameObject txt = Instantiate(EnergyText, transform.position, Quaternion.identity, transform);
            txt.GetComponent<TextMesh>().text = "Energy " + energy.ToString();
        
    }
   /// <summary>
   /// Begins the search for energy.
   /// </summary>
   /// <returns></returns>
   async Task<bool> FindEnergy()
    {
        GameObject txt = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        lookforenergy = true;
        bool was_exploring = exploring;
        bool was_returning = movingtovillage;
        exploring = false;
        movingtovillage = false;
        prev_resource = find_resource;
        find_resource = "E";
        Vector2 resfound = FindExploredResource("E");
        while (resfound.x == -1)
        {
            await ExploreForResource();
            resfound = FindExploredResource("E");
            if (dead)
            {
                break;
            }
        }
        await Move_To_Point(resfound);
        exploring = was_exploring;
        movingtovillage = was_returning;
        find_resource = prev_resource;
        return true;
    }

    /// <summary>
    /// Checks if the energy is below the threshold to be considered low.
    /// </summary>
    bool IsEnergyLow()
    {
        if (energy < startingEnergy * threshold / 100)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Unload resouce to village.
    /// </summary>
    void UnloadResource()
    {
        if (team.Equals("Team1"))
        {
            if (holded_resource == "G")
            {
                Grid_Inspector.CURRENT_GOLD1++;
            }
            else if (holded_resource == "F")
            {
                Grid_Inspector.CURRENT_WOOD1++;
            }
            else if (holded_resource == "S")
            {
                Grid_Inspector.CURRENT_STONE1++;
            }
            else
            {
                Grid_Inspector.CURRENT_BERRIES1++;
            }
        }
        else
        {
            if (holded_resource == "G")
            {
                Grid_Inspector.CURRENT_GOLD2++;
            }
            else if (holded_resource == "F")
            {
                Grid_Inspector.CURRENT_WOOD2++;
            }
            else if (holded_resource == "S")
            {
                Grid_Inspector.CURRENT_STONE2++;
            }
            else
            {
                Grid_Inspector.CURRENT_BERRIES2++;
            }
        }
        UIMaster.RefreshResources();
        holds = false;
        holded_resource = "";
        movingtovillage = false;
    }
    /// <summary>
    /// Get the tilles arround the villager.
    /// </summary>
    void GetAdjacentTiles()
    {
        adjacentiles.Clear();
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        if (x + 1 < Grid_Inspector.M)
        {
            adjacentiles.Add(Grid_Inspector.board[x + 1, y]);
        }
        if (x - 1 >=0)
        {
            adjacentiles.Add(Grid_Inspector.board[x - 1, y]);
        }
        if (y + 1 < Grid_Inspector.N)
        {
            adjacentiles.Add(Grid_Inspector.board[x, y+1]);
        }
        if (y - 1 >= 0)
        {
            adjacentiles.Add(Grid_Inspector.board[x, y-1]);
        }
    }
}   

