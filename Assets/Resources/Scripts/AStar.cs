
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    class AStar
    {
        public class Node
        {
            public Node parent=null;
            public Vector2 location;
            public float score;
            public int cost=1;

        }
        /// <summary>
        /// Implements tha AStar algorithm and returns the stack of the steps the npc has to follow.
        /// </summary>
        /// <param name="currentlocation"></param>
        /// <param name="targetlocation"></param>
        /// <returns></returns>
        public Stack<Node> starimplementation(Vector2 currentlocation,Vector2 targetlocation)
        {
            Node currentnode;
            Stack<Node> Path = new Stack<Node>();
            Node startingnode = new Node();
            startingnode.location = currentlocation;
            startingnode.score = heuristic(currentlocation, targetlocation);
            Node root = startingnode;
            List<Node> openlist = new List<Node> {startingnode};
            List<Node> closedlist = new List<Node>();
            do
            {
                currentnode = openlist.OrderBy(x => x.score).FirstOrDefault();
                closedlist.Add(currentnode);
                openlist.Remove(currentnode);
                List<Vector2> possiblemoves = new List<Vector2>();
                if(PossibleToMove(new Vector2(currentnode.location.x, currentnode.location.y + 1),targetlocation)){
                    possiblemoves.Add(new Vector2(currentnode.location.x, currentnode.location.y + 1));
                }
                if (PossibleToMove(new Vector2(currentnode.location.x, currentnode.location.y - 1), targetlocation))
                {
                    possiblemoves.Add(new Vector2(currentnode.location.x, currentnode.location.y - 1));
                }
                if (PossibleToMove(new Vector2(currentnode.location.x + 1, currentnode.location.y), targetlocation))
                {
                    possiblemoves.Add(new Vector2(currentnode.location.x + 1, currentnode.location.y));
                }
                if (PossibleToMove(new Vector2(currentnode.location.x - 1, currentnode.location.y), targetlocation)) 
                {
                    possiblemoves.Add(new Vector2(currentnode.location.x - 1, currentnode.location.y));
                }
                foreach(Vector2 loc in possiblemoves)
                {
                    if (closedlist.Any(x => x.location == loc))
                    {
                        continue;
                    }
                    if (!openlist.Any(x => x.location == loc))
                    {

                        Node tempo = new Node();
                        tempo.parent = currentnode;
                        tempo.location = loc;
                        tempo.score = heuristic(loc, targetlocation);
                        tempo.score += tempo.parent.score;
                        openlist.Add(tempo);
                    }
                    else
                    {
                        if (currentnode.cost < openlist.Find(x => x.location == loc).parent.cost)
                        {
                            openlist.Find(x => x.location == loc).parent = currentnode;
                        }
                    }
                }
            } while (openlist.Count > 0 && !closedlist.Any(x => x.location == targetlocation));


            if (!closedlist.Exists(x => x.location == targetlocation))
            {
                Node s = new Node();
                s.location = currentlocation;
                s.score = 0;
                Path.Push(s);
                return Path;
            }

            Node temp = closedlist[closedlist.IndexOf(currentnode)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.parent;
            } while (temp != root && temp != null);
            return Path;

        }
        public float heuristic(Vector2 current,Vector2 goal)
        {
            return Vector2.Distance(current, goal);
        }
        bool PossibleToMove(Vector2 location,Vector2 goal)
        {
            if (Is_In_Map(location))
            {
                if ((Grid_Inspector.board[(int)location.x, (int)location.y].type == "R" || Grid_Inspector.board[(int)location.x, (int)location.y].type == "E" || location == goal) && (!Is_containing(location)||location==goal))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        bool Is_In_Map(Vector2 position)
        {
            if (position.x < 0 || position.x >= Grid_Inspector.board.GetLength(0))
            {
                return false;
            }
            else if (position.y < 0 || position.y >= Grid_Inspector.board.GetLength(1))
            {
                return false;
            }
            return true;
        }
        bool Is_containing(Vector2 position)
        {
            if (Grid_Inspector.board[(int)position.x, (int)position.y].contain != null)
            {
                if(Grid_Inspector.board[(int)position.x, (int)position.y].contain.name == "Energy_Potion(Clone)")
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
