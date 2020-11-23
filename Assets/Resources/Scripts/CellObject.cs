using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject 
{
    public int posx, posz;
    public Object contain;
    public string type;
    public CellObject(int posX, int posZ,string typE)
    {
        posx = posX;
        posz = posZ;
        type = typE;
    }
}
