using UnityEngine;
using System.Collections;

public class AB_STATBARS : MonoBehaviour 
{


    public int barDisplay = AppleBloomStats.stats.maxhp;  //Current Progress//
    public Vector2 pos = new Vector2(20, 40);
    public Vector2 size = new Vector2(60, 20);
    public Texture2D emptyTex;
    public Texture2D fullTex;
    

    // Use this for initialization
	void OnGUI () 
    {
	//DrawBackground//
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
          GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);

        //DrawFilled-in bar.//
        GUI.BeginGroup(new Rect(0,0, size.x, size.y), fullTex);
          GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
         GUI.EndGroup();
        GUI.EndGroup();
        

	
    }

	// Update is called once per frame
	void Update () 
    
    {
	//CurrentHP//

        barDisplay-=1;



	}
}
