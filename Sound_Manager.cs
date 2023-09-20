using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    public static Sound_Manager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }
    public void SetClip(AudioClip clip)
    {
       source.clip = clip;
       source.Play();
    }
}
