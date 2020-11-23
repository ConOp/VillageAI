using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using SimpleFileBrowser;
using System.Threading.Tasks;

public class SettingsHandler :MonoBehaviour
{
    string path;
    private void Start()
    {
        GameObject.FindGameObjectWithTag("LoadMapCheck").GetComponent<Toggle>().isOn = false;
        GameObject.FindGameObjectWithTag("SelectMap").GetComponent<Button>().interactable = false;
        Grid_Inspector.usepreloadedmap = false;
    }
    public void Check_Settings()
    {
        //load values Village 1
        int villagers_1_number;
        int wood_1_target;
        int berries_1_target;
        int stone_1_target;
        int gold_1_target;
        int starting_1_energy;
        int starting_1_money;
        int villagers_1_threshold;
        int.TryParse(GameObject.Find("villagers_1_number").GetComponent<Text>().text,out villagers_1_number);
        int.TryParse(GameObject.Find("Wood_1_Number").GetComponent<Text>().text, out wood_1_target);
        int.TryParse(GameObject.Find("Berries_1_Number").GetComponent<Text>().text, out berries_1_target); 
        int.TryParse(GameObject.Find("Stone_1_Number").GetComponent<Text>().text, out stone_1_target);
        int.TryParse(GameObject.Find("Gold_1_Number").GetComponent<Text>().text, out gold_1_target);
        int.TryParse(GameObject.Find("Energy_1").GetComponent<Text>().text, out starting_1_energy);
        int.TryParse(GameObject.Find("Money_1").GetComponent<Text>().text, out starting_1_money);
        int.TryParse(GameObject.Find("Energy_1_Threshold").GetComponent<Text>().text, out villagers_1_threshold);

        //load values Village 2
        int villagers_2_number;
        int wood_2_target;
        int berries_2_target;
        int stone_2_target;
        int gold_2_target;
        int starting_2_energy;
        int starting_2_money;
        int villagers_2_threshold;
        int.TryParse(GameObject.Find("villagers_2_number").GetComponent<Text>().text, out villagers_2_number);
        int.TryParse(GameObject.Find("Wood_2_Number").GetComponent<Text>().text, out wood_2_target);
        int.TryParse(GameObject.Find("Berries_2_Number").GetComponent<Text>().text, out berries_2_target);
        int.TryParse(GameObject.Find("Stone_2_Number").GetComponent<Text>().text, out stone_2_target);
        int.TryParse(GameObject.Find("Gold_2_Number").GetComponent<Text>().text, out gold_2_target);
        int.TryParse(GameObject.Find("Energy_2").GetComponent<Text>().text, out starting_2_energy);
        int.TryParse(GameObject.Find("Money_2").GetComponent<Text>().text, out starting_2_money);
        int.TryParse(GameObject.Find("Energy_2_Threshold").GetComponent<Text>().text, out villagers_2_threshold);

        //load trade settings
        int energy_price;
        int energy_threshold;
        int energy_amount;
        int map_price;
        int map_threshold;
        int.TryParse(GameObject.Find("energy_price").GetComponent<Text>().text, out energy_price);
        int.TryParse(GameObject.Find("energy_threshold").GetComponent<Text>().text, out energy_threshold);
        int.TryParse(GameObject.Find("energy_amount").GetComponent<Text>().text, out energy_amount);
        int.TryParse(GameObject.Find("map_price").GetComponent<Text>().text, out map_price);
        int.TryParse(GameObject.Find("map_threshold").GetComponent<Text>().text, out map_threshold);

        //load potion settings
        int potion_respawn_rate;
        int energy_gain;
        int.TryParse(GameObject.Find("potion_respawn_rate").GetComponent<Text>().text, out potion_respawn_rate);
        int.TryParse(GameObject.Find("energy_gain").GetComponent<Text>().text, out energy_gain);
        //load world generation settings
        int n_size;
        int m_size;
        int berries_gen;
        int forest_gen;
        int stone_gen;
        int gold_gen;
        int.TryParse(GameObject.Find("n_size").GetComponent<Text>().text, out n_size);
        int.TryParse(GameObject.Find("m_size").GetComponent<Text>().text, out m_size);
        int.TryParse(GameObject.Find("berries_gen").GetComponent<Text>().text, out berries_gen);
        int.TryParse(GameObject.Find("forest_gen").GetComponent<Text>().text, out forest_gen);
        int.TryParse(GameObject.Find("stone_gen").GetComponent<Text>().text, out stone_gen);
        int.TryParse(GameObject.Find("gold_gen").GetComponent<Text>().text, out gold_gen);

        //check if correct
        if (villagers_1_number < 4 || villagers_1_number > 10)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "Villagers 1 Number must be 4 - 10.";
            return;
        }else if (wood_1_target<0 || berries_1_target<0 || stone_1_target < 0 || gold_1_target < 0|| starting_1_energy < 0|| starting_1_money < 0||villagers_1_threshold<0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "Village 1 Resource Settings and Villager Settings must be 0 or greater.";
            return;
        }
        else if (wood_2_target < 0 || berries_2_target < 0 || stone_2_target < 0 || gold_2_target < 0 || starting_2_energy < 0 || starting_2_money < 0 || villagers_2_threshold < 0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "Village 2 Resource Settings and Villager Settings must be 0 or greater.";
            return;
        }
        else if (potion_respawn_rate < 1 || energy_gain <0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "Potion respawn rate must be greater than 1 and energy gain 0 or greater.";
            return;
        }
        else if (((berries_1_target > 0 || berries_2_target > 0) && berries_gen == 0)||berries_gen<0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "World generation berries must be 0 or greater. If at least one of the villages require berries then the number must be 1 or greater.";
            return;
        }
        else if (((wood_1_target > 0 ||wood_2_target > 0) && forest_gen == 0) || forest_gen < 0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "World generation forests must be 0 or greater. If at least one of the villages require forests then the number must be 1 or greater.";
            return;
        }
        else if (((stone_1_target > 0 || stone_2_target > 0) && stone_gen == 0) || stone_gen < 0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "World generation stone must be 0 or greater. If at least one of the villages require stone then the number must be 1 or greater.";
            return;
        }
        else if (((gold_1_target > 0 || gold_2_target > 0) && gold_gen == 0) || gold_gen < 0)
        {
            GameObject.Find("Errortext").GetComponent<Text>().text = "World generation gold must be 0 or greater. If at least one of the villages require gold then the number must be 1 or greater.";
            return;
        }
        //Pass world settings.
        Grid_Inspector.N = n_size;
        Grid_Inspector.M = m_size;
        Grid_Inspector.WOOD = forest_gen;
        Grid_Inspector.BERRIES = berries_gen;
        Grid_Inspector.STONE = stone_gen;
        Grid_Inspector.GOLD = gold_gen;
        //pass energy potion settings.
        PotionGenerator.delay = potion_respawn_rate;
        Grid_Inspector.POTION_ENERGY = energy_gain;
        //Pass trade settings.
        TradeMaster.energy_price = energy_price;
        TradeMaster.energy_threshold = energy_threshold;
        TradeMaster.energy_trade_amount = energy_amount;
        TradeMaster.map_price = map_price;
        TradeMaster.map_threshold = map_threshold;
        //Pass Team 1 Settings.
        Grid_Inspector.NPC_VILLAGE_1 = villagers_1_number;
        Grid_Inspector.NEED_WOOD1 = wood_1_target;
        Grid_Inspector.NEED_GOLD1 = gold_1_target;
        Grid_Inspector.NEED_STONE1 = stone_1_target;
        Grid_Inspector.NEED_BERRIES1 = berries_1_target;
        Grid_Inspector.TEAM1ENERGY = starting_1_energy;
        Grid_Inspector.TEAM1GOLD = starting_1_money;
        Grid_Inspector.TEAM1ENERGYTHRESHOLD = villagers_1_threshold;
        //Pass Team 2 Settings.
        Grid_Inspector.NPC_VILLAGE_2 = villagers_2_number;
        Grid_Inspector.NEED_WOOD2 = wood_2_target;
        Grid_Inspector.NEED_GOLD2 = gold_2_target;
        Grid_Inspector.NEED_STONE2 = stone_2_target;
        Grid_Inspector.NEED_BERRIES2 = berries_2_target;
        Grid_Inspector.TEAM2ENERGY = starting_2_energy;
        Grid_Inspector.TEAM2GOLD = starting_2_money;
        Grid_Inspector.TEAM2ENERGYTHRESHOLD = villagers_2_threshold;


        SceneManager.LoadScene("Game");
    }

    public void SelectmapFile()
    {
        StartCoroutine("ShowLoadDialogCoroutine");

        
    }

   IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, false, null, "Load Map", "Load");
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            
              path=FileBrowser.Result[0];
            if (path.Length != 0)
            {
                StreamReader sr;
                List<string> map;
                try
                {
                    sr = new StreamReader(path);
                    map = new List<string>();


                }
                catch (Exception e)
                {
                    GameObject.Find("Errortext").GetComponent<Text>().text = "An error occured when loading the file.";
                    yield break;
                }
                using (sr)
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        map.Add(line);
                    }
                }
                char[,] mapgrid = new char[map[0].Length - 1, map.Count - 2];
                for (int i = 0; i < mapgrid.GetLength(0); i++)
                {
                    for (int j = 0; j < mapgrid.GetLength(1); j++)
                    {
                        mapgrid[i, j] = map[i][j];
                    }
                }

                Grid_Inspector.preloadedmap = mapgrid;
                GameObject.Find("Errortext").GetComponent<Text>().text = "Map Loaded Successfully";
            }
        }
    }
    
    public void savemap()
    {
        Grid_Inspector.savemap = this.GetComponent<Toggle>().isOn;
        if (Grid_Inspector.savemap)
        {
            StartCoroutine("ShowSaveDialogCoroutine");
        }
    }
    IEnumerator ShowSaveDialogCoroutine()
    {
        yield return FileBrowser.WaitForSaveDialog(true, false, null, "Select Folder", "Save");
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {

            Grid_Inspector.folderpath= FileBrowser.Result[0];
        }
    }
    public void changemapmode()
    {
        if (GameObject.FindGameObjectWithTag("LoadMapCheck").GetComponent<Toggle>().isOn == true)
        {
            GameObject.FindGameObjectWithTag("SelectMap").GetComponent<Button>().interactable = true;
            Grid_Inspector.usepreloadedmap = true;
           foreach(GameObject gm in GameObject.FindGameObjectsWithTag("NewWorld"))
            {
                if (gm.GetComponent<InputField>())
                {
                    gm.GetComponent<InputField>().interactable = false;
                }
                if (gm.GetComponent<Toggle>())
                {
                    gm.GetComponent<Toggle>().interactable = false;
                    gm.GetComponent<Toggle>().isOn = false;

                }
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("SelectMap").GetComponent<Button>().interactable = false;
            Grid_Inspector.usepreloadedmap = false;
            foreach (GameObject gm in GameObject.FindGameObjectsWithTag("NewWorld"))
            {
                if (gm.GetComponent<InputField>())
                {
                    gm.GetComponent<InputField>().interactable = true;
                }
                if (gm.GetComponent<Toggle>())
                {
                    gm.GetComponent<Toggle>().interactable = true;
                }
            }
        }
    }


}
