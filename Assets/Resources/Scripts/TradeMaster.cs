using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TradeMaster : MonoBehaviour
{
    public GameObject TradeText;
    public static int energy_threshold;
    public static int energy_trade_amount;
    public static int map_price;
    public static int map_threshold ;
    public static int energy_price;
    
    private  void  Start()
    {
        InvokeRepeating("MakeTrade", 1.0f, 1f);
    }
     void MakeTrade()
    {
        List<(NPC,NPC)>availabletrades = GetAvailableforTrade();
        if (availabletrades.Count > 0)
        {
            foreach ((NPC, NPC) trade in availabletrades)
            {
                TradeEnergy(trade);
                TradeMap(trade);
            }
        }
    }
    List<(NPC,NPC)> GetAvailableforTrade()
    {
        List<(NPC, NPC)> availabletrades = new List<(NPC, NPC)>();
        for(int i=0; i < Grid_Inspector.npcs1.Count; i++)
        {
            foreach(CellObject obj in Grid_Inspector.npcs1[i].GetComponent<NPC>().adjacentiles)
            {
                
                var newobj = obj.contain as NPC;
                if (newobj != null)
                {
                    if (newobj.tradeavailable)
                    {
                        availabletrades.Add((Grid_Inspector.npcs1[i].GetComponent<NPC>(), newobj));
                    }
                }
            }
        }
        for (int i = 0; i < Grid_Inspector.npcs2.Count; i++)
        {
            foreach (CellObject obj in Grid_Inspector.npcs2[i].GetComponent<NPC>().adjacentiles)
            {
                var newobj = obj.contain as NPC;
                if (newobj != null)
                {
                    if (newobj.tradeavailable)
                    {
                        availabletrades.Add((Grid_Inspector.npcs2[i].GetComponent<NPC>(), newobj));
                    }
                }
                
                    

            }
        }
        return availabletrades;

    }
    void TradeEnergy((NPC,NPC) trade)
    {
        if (trade.Item1.tradeavailable&&trade.Item2.tradeavailable)
        {
            if (trade.Item1.lookforenergy)
            {
                if (trade.Item2.energy >= trade.Item2.startingEnergy * energy_threshold / 100)
                {
                    if (trade.Item1.gold >= energy_price)
                    {
                        trade.Item1.gold -= energy_price;
                        trade.Item2.gold += energy_price;
                        trade.Item1.energy += energy_trade_amount;
                        trade.Item2.energy -= energy_trade_amount;
                        GameObject txt = Instantiate(TradeText, trade.Item1.transform.position, Quaternion.identity, transform);
                        txt.GetComponentInChildren<TextMesh>().text = "Energy Trade Done!";
                        if(trade.Item1.team==trade.Item2.team && trade.Item1.team == "Team1")
                        {
                            Grid_Inspector.Team1EnergyTrades++;

                        }
                        else if (trade.Item1.team==trade.Item2.team && trade.Item1.team == "Team2")
                        {
                            Grid_Inspector.Team2EnergyTrades++;
                        }
                        else if (trade.Item1.team != trade.Item2.team)
                        {
                            Grid_Inspector.CrossTeamEnergyTrades++;
                        }
                        trade.Item1.tradeavailable = false;
                        trade.Item2.tradeavailable = false;
                    }
                }
            }
        }
    }
    void TradeMap((NPC, NPC) trade)
    {
        if (trade.Item1.tradeavailable && trade.Item2.tradeavailable)
        {
            int commonknown = 0;
            int notknown = 0;
            for(int i =0; i < trade.Item1.knownmap.GetLength(0); i++)
            {
                for (int j = 0; j < trade.Item2.knownmap.GetLength(1); j++)
                {
                    if (trade.Item2.knownmap[i, j] == "1")
                    {
                        if (trade.Item1.knownmap[i, j] == trade.Item2.knownmap[i, j])
                        {
                            commonknown++;
                        }
                        else
                        {
                            notknown++;
                        }
                    }
                }
            }
            if (notknown >= (commonknown * map_threshold / 100))
            {
                if (trade.Item1.gold >= map_price)
                {
                    for (int i = 0; i < trade.Item1.knownmap.GetLength(0); i++)
                    {
                        for (int j = 0; j < trade.Item2.knownmap.GetLength(1); j++)
                        {
                            if (trade.Item2.knownmap[i, j] == "1")
                            {
                                trade.Item1.knownmap[i, j] = "1";
                            }
                        }
                    }
                    trade.Item1.gold -= map_price;
                    trade.Item2.gold += map_price;
                    GameObject txt = Instantiate(TradeText, trade.Item1.transform.position, Quaternion.identity, transform);
                    txt.GetComponentInChildren<TextMesh>().text = "Map Trade Done!";
                    if (trade.Item1.team == trade.Item2.team && trade.Item1.team == "Team1")
                    {
                        Grid_Inspector.Team1EnergyTrades++;

                    }
                    else if (trade.Item1.team == trade.Item2.team && trade.Item1.team == "Team2")
                    {
                        Grid_Inspector.Team2EnergyTrades++;
                    }
                    else if (trade.Item1.team != trade.Item2.team)
                    {
                        Grid_Inspector.CrossTeamMapTrades++;
                    }
                    trade.Item1.tradeavailable = false;
                    trade.Item2.tradeavailable = false;
                }

            }
        }
    }
}
