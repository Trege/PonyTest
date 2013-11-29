using UnityEngine;
using System.Collections;

public class PlayerStats : EntityStats

   
{
public float hpGrowth = 1,
             spGrowth = 1,
             strGrowth = 1,
             defGrowth = 1,
             agiGrowth = 1,
             level = 1;
	
	public void SetLevel(int level)
{      
    LVL = level;
    //Set other stats based on level.//
}
        
	
	
	
	
}
