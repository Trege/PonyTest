using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void AnimationEvent(object sender, AnimationArgs args);
public class AnimationArgs : EventArgs
{
    public int AnimationID;
    public int CurrentIndex;

    public AnimationArgs(int AnimationID, int CurrentIndex)
    {
        this.AnimationID = AnimationID;
        this.CurrentIndex = CurrentIndex;
    }
}

public class AnimatedSprite
{
    //Textures
    public readonly int count;
    public readonly Texture2D[] subimages;

    //Playback
    private int _index;
    public int index
    {
        set
        {
            _index = value % count;
        }
        get
        {
            return _index;
        }
    }
    public Texture2D currentImage
    {
        get
        {
            return subimages[index];
        }
    }
    private float currentIndex = 0; //Internal frame index
    private int startIndex = 0; //Frame to start the animation from
    private int endIndex = 0; //Frame to end the animation on
    private float playbackRate = 1; //Playback rate in frames per second
    private bool loopAnimation = true; //Whether to play back from the beginning when the animation ends
    private bool animating = false; //Are we currently animating?

    public float defaultPlaybackRate = 1; //Default playback rate
    public bool doesLoop = false; //If this should loop by default

    //Animation
    private int currentAnimationID = -1;
    private List<int> animations = new List<int>();
    public event AnimationEvent onAnimationStart; //Called once when this animated sprite starts animating
    public event AnimationEvent onAnimationPlaying; //Called every frame that this sprite is animating
    public event AnimationEvent onAnimationEnd; //Called once when this animated sprite finishes animating

    //Constructor
    public AnimatedSprite(Texture2D spriteSheet, int subimageCount, int cellWidth, int cellHeight, float defaultPlaybackRate = 1, bool doesLoop = false, int paddingWidth = 0, int paddingHeight = 0, int offsetX = 0, int offsetY = 0)
    {
        this.defaultPlaybackRate = defaultPlaybackRate;
        this.doesLoop = doesLoop;
        int cellPaddedWidth = cellWidth + paddingWidth * 2;
        int cellPaddedHeight = cellHeight + paddingHeight * 2;
        int cellsH = (spriteSheet.width - offsetX) / cellPaddedWidth;
        int cellsV = (spriteSheet.height - offsetY) / cellPaddedHeight;
        count = Mathf.Min(subimageCount, cellsH * cellsV);
        subimages = new Texture2D[count];
        for(int i = 0; i < count; i++)
            subimages[i] = new Texture2D(cellWidth, cellHeight);

        int index = 0;
        
        for (int y = cellsV - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellsH; x++)
            {
                if (index >= count)
                    break;
                Texture2D tex = subimages[index++];
                tex.SetPixels(spriteSheet.GetPixels(offsetX + paddingWidth + x * cellPaddedWidth, offsetY + paddingHeight + y * cellPaddedHeight, cellWidth, cellHeight));
                tex.Apply();
                if (index >= count)
                    break;
            }
        }
    }

    public int AddAnimation(int startFrame, int endFrame)
    {
        animations.Add(startFrame + (endFrame << 16)); //Serialize startFrame and endFrame as two shorts, into one int
        return animations.Count - 1;
    }

    public void PlayAnimation()
    {
        PlayAnimation(0, count - 1, defaultPlaybackRate, doesLoop);
    }

    public void PlayAnimation(int startIndex, int endIndex, float playbackRate, bool loopAnimation = false)
    {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.playbackRate = playbackRate;
        this.loopAnimation = loopAnimation;
        currentIndex = playbackRate > 0 ? startIndex : endIndex;
        currentAnimationID = -1;

        animating = true;
        if (onAnimationStart != null)
            onAnimationStart(this, new AnimationArgs(currentAnimationID, startIndex));
    }

    public void PlayAnimation(int animationID, float playbackRate, bool loopAnimation = false)
    {
        this.startIndex = animations[animationID] & 65535;
        this.endIndex = animations[animationID] >> 16;
        this.playbackRate = playbackRate;
        this.loopAnimation = loopAnimation;
        currentIndex = playbackRate > 0 ? startIndex : endIndex;
        currentAnimationID = animationID;

        animating = true;
        if (onAnimationStart != null)
            onAnimationStart(this, new AnimationArgs(currentAnimationID, startIndex));
    }

    public void StopAnimation()
    {
        animating = false;
        if (onAnimationEnd != null)
            onAnimationEnd(this, new AnimationArgs(currentAnimationID, index));
    }

    public void SetPlaybackRate(float playbackRate)
    {
        this.playbackRate = playbackRate;
    }

    public void Animate()
    {
        if (animating)
        {
            if (currentIndex == -1)
                currentIndex = startIndex;
            else
                currentIndex += playbackRate * Time.deltaTime;

            if (currentIndex < startIndex)
            {
                if (loopAnimation)
                    currentIndex = endIndex;
                else
                {
                    currentIndex = -1;
                    animating = false;
                    if (onAnimationEnd != null)
                        onAnimationEnd(this, new AnimationArgs(currentAnimationID, index));
                }
            }

            if (currentIndex > endIndex)
            {
                if (loopAnimation)
                    currentIndex = startIndex;
                else
                {
                    currentIndex = -1;
                    index = endIndex;
                    animating = false;
                    if (onAnimationEnd != null)
                        onAnimationEnd(this, new AnimationArgs(currentAnimationID, index));
                }
            }

            if (currentIndex > -1)
                index = (int)currentIndex;

            if (onAnimationPlaying != null)
                onAnimationPlaying(this, new AnimationArgs(currentAnimationID, index));
        }
    }
}