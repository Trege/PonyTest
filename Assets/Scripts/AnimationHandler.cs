using UnityEngine;
using System.Collections;
public enum PlayerStates { Stand, Walk, Jump, Hit } //Stand = 0; Walk = 1; Jump = 2; Hit = 3; //
public class AnimationHandler : MonoBehaviour
{
    private int stateIndex
    {
        get
        {
            return (int)state;
        }
    }

    AnimatedSprite[] sprites = new AnimatedSprite[3];
    public AnimatedSprite sprite
    {
        get
        {
            return sprites[stateIndex];
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
        sprites[0] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_SR") as Texture2D, 135, 100, 102, 15, true);
        sprites[1] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_WR") as Texture2D, 16, 100, 102, 15, true);
        sprites[2] = new AnimatedSprite(Resources.Load("Apple Bloom/AB_JR") as Texture2D, 23, 102, 122, 15);
        sprite.onAnimationStart += OnAnimationStart;
        sprite.onAnimationEnd += OnAnimationEnd;
        state = PlayerStates.Stand;
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
