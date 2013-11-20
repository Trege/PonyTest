using UnityEngine;
using System;
using System.Collections;

public enum PlayerStates { Stand, Walk, Jump, Hit } //Stand = 0; Walk = 1; Jump = 2; Hit = 3; //
public enum PlayerName { AppleBloom, SweetieBelle, Scootaloo, PlaceHolder }; //0 = AppleBloom//1 = Sweetiebelle//2 = Scootaloo//
public class AnimationHandler : MonoBehaviour
{
    AnimatedSprite[,] sprites = new AnimatedSprite[Enum.GetNames(typeof(PlayerName)).Length, Enum.GetNames(typeof(PlayerStates)).Length];
    public AnimatedSprite sprite
    {
        get
        {
            return sprites[(int)character, (int)state];
        }
    }

    private PlayerName _character = PlayerName.AppleBloom; //This value stores the player state.//
    public PlayerName character
    {
        get
        {
            return _character;
        }
        set
        {
            if (_character != value)
            {
                _character = value;
                sprite.PlayAnimation();
            }
        }
    }

    private PlayerStates _state = PlayerStates.Stand; //This value stores the player state.//
    public PlayerStates state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
            {
                _state = value;
                sprite.PlayAnimation();
            }
        }
    }
    
    public bool loopAnimation = false;
    public bool reverseAnimation = false;
    public int framesPerSecond = 30;
    public int maxImages = 0;

	void Start()
    {
        PlayerName playerName;
        
        //Applebloom
        playerName = PlayerName.AppleBloom;
        sprites[(int)playerName, (int)PlayerStates.Stand] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_SR") as Texture2D, 135, 100, 102, 15, true);
        sprites[(int)playerName, (int)PlayerStates.Walk] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_WR") as Texture2D, 16, 100, 102, 15, true);
        sprites[(int)playerName, (int)PlayerStates.Jump] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_JR") as Texture2D, 23, 102, 122, 15);
        //Sweetiebelle
        playerName = PlayerName.SweetieBelle;
        sprites[(int)playerName, (int)PlayerStates.Stand] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_SR") as Texture2D, 135, 100, 102, 15, true);
        sprites[(int)playerName, (int)PlayerStates.Walk] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_WR") as Texture2D, 16, 100, 102, 15, true);
        sprites[(int)playerName, (int)PlayerStates.Jump] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_JR") as Texture2D, 23, 102, 122, 15);
        //Scootaloo
        playerName = PlayerName.Scootaloo;
        sprites[(int)playerName, (int)PlayerStates.Stand] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_SR") as Texture2D, 135, 100, 102, 15, true);
        sprites[(int)playerName, (int)PlayerStates.Walk] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_WR") as Texture2D, 16, 100, 102, 15, true);
        sprites[(int)playerName, (int)PlayerStates.Jump] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_JR") as Texture2D, 23, 102, 122, 15);

        //Setup animation stuff
        sprite.onAnimationStart += OnAnimationStart;
        sprite.onAnimationEnd += OnAnimationEnd;
        sprite.PlayAnimation();
	}
	
	void Update ()
    {
        sprite.Animate();
        renderer.material.mainTexture = sprite.currentImage;
	}

    void OnAnimationStart(object sender, AnimationArgs args)
    {
        //Debug.Log(name + ": Starting animation.");
    }

    void OnAnimationEnd(object sender, AnimationArgs args)
    {
        //Debug.Log(name + ": Done animating.");
    }
}
