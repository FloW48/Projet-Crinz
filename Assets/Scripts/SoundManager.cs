using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static AudioSource audioSource;
    public static AudioClip[] ballPop;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ballPop = Resources.LoadAll<AudioClip>("ballPops");
        Debug.Log(ballPop.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound()
    {
        audioSource.PlayOneShot(ballPop[Random.Range(0, ballPop.Length-1)]);
    }
}
