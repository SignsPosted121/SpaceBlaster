using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public GameObject Sounds;
    
    public void PlaySound(string name)
	{
        var sound = GetSound(name);
        if (sound != null)
            sound.GetComponent<AudioSource>().Play();
	}

    public AudioSource GetSound(string name)
    {
        var sound = Sounds.transform.Find(name);
        if (sound != null)
            return sound.GetComponent<AudioSource>();
        else
            return null;
    }
}
