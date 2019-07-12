using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public static class SOUND
{
    [FMODUnity.EventRef]
    static EventInstance ambiance;
    static EventInstance musiqueGameplay;


    public static void StartAmbiance()
    {
        ambiance = FMODUnity.RuntimeManager.CreateInstance("event:/Ambiance");
        ambiance.start();
    } 
    public static void SopAmbiance()
    {
        ambiance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public static void StartMusic()
    {
        musiqueGameplay = FMODUnity.RuntimeManager.CreateInstance("event:/Music_Gameplay");
        musiqueGameplay.start();
    }
    public static void StopMusic()
    {
        musiqueGameplay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public static void BiteApple()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_BiteApple").start();
    }
    public static void Dash()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_Dash").start();
    }
    public static void Grab()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_Grab").start();
    }
    public static void HitPlayer()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_HitPlayer").start();
    }
    public static void HitTree()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_HitTree").start();
    }
    public static void MouseClick()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_MouseClik").start();
    }
    public static void MouseOver()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_MouseOver").start();
    }
    public static void PommePop()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_PommeDrop").start();
    }
    public static void Step()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_Step").start();
    }
    public static void Stun()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_Stun").start();
    }
    public static void ThrowApple()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_ThrowApple").start();
    }
    public static void WinPoint()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_WinPoint").start();
    }
    public static void MeteorSpawn()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_MeteorSpawn").start();
    }
    public static void MeteorImpact()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_MeteorImpact").start();
    }
    public static void Drop()
    {
        FMODUnity.RuntimeManager.CreateInstance("event:/SFX_Drop").start();
    }

}
