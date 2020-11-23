using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public static float delay ;
    CellObject[,] map;
    void Start()
    {
        map = Grid_Inspector.board;
        StartCoroutine(GenerateEnergyPot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GenerateEnergyPot()
    {
        GameObject energypot = (Resources.Load("Prefabs/Energy_Potion") as GameObject);
        int x, y;
        while (true)
        {
           do
           {
                x = Random.Range(0, Grid_Inspector.board.GetLength(0));
                y = Random.Range(0, Grid_Inspector.board.GetLength(1));
            } while (map[x, y].contain != null || map[x, y].type!="R");
            Grid_Inspector.board[x,y].type="E";
            Grid_Inspector.board[x,y].contain=Instantiate(energypot, new Vector2(x, y), new Quaternion());
            yield return new WaitForSeconds(delay);
        }
    }
}
