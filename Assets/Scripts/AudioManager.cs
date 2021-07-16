using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.setSource(gameObject.AddComponent<AudioSource>());
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound [" + name + "] not found.");
            return;
        }
         s.source.Play();
    }
    public void PlayWeaponShotSound(WeaponType type) 
    {
        switch (type) 
        {
            case WeaponType.Pistol:
            {
                Play("pistol shot");
                break;
            }   
            case WeaponType.Uzi: 
            {
                Play("uzi shot");
                break;
            }
            case WeaponType.Shotgun:
            {
                Play("shotgun shot");
                break;
            }
            case WeaponType.RocketLauncher: 
            {
                Play("rocketlauncher");
                break;
            }
            default: 
            {
                Play("uzi shot");
                break;
            }
        }
    }
}
