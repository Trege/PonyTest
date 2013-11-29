using UnityEngine;
using System.Collections;

public class AppleBloomStats : PlayerStats
   
{

   static public PlayerStats stats = new PlayerStats();
   public void PlayerStats(float maxhp, float hp, float hpGrowth, float agiGrowth) //Constructor.//
    
   
   
{
    stats.maxhp = 20; this.hp = this.maxhp;
}

	// Use this for initialization
	void Start () 
    
    {
        stats.maxhp = 20 *stats.LVL;      //Max hitPoints//
        stats.hp = stats.maxhp;           //Make the HP start at the max when the game begins//
        stats.LVL = 1;                    //The Entity's current level.//    
        stats.fli = 1;                    //Flight and jumping skill.//




	}
	
	// Update is called once per frame
	void Update () 
    {

        stats.maxhp = 20;    //Max hitPoints//
        stats.sp = 10*stats.LVL;    //MP/Stamina//
        stats.str = 4*stats.LVL;    //Strength//
        stats.def = 4*stats.LVL;    //Defense//
        stats.agi = 5*stats.LVL;    //Agility//
        
        
	}
}
