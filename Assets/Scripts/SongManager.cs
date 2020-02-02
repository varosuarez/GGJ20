using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{

    private AudioSource audio;

    public AudioClip powerlessAudio;
    public AudioClip canJumpAudio;
    public AudioClip canLoadAudio;
    public AudioClip canClimbAudio;
    public AudioClip canPhaseAudio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        ChangeClip(DinamicPlayer.State.Powerless);
    }


    void ChangeClip(DinamicPlayer.State state)
    {
        switch (state)
        {
            case DinamicPlayer.State.Powerless:
                audio.clip = powerlessAudio;
                break;
            case DinamicPlayer.State.CanJump:
                audio.clip = canJumpAudio;
                break;
            case DinamicPlayer.State.CanLoad:
                audio.clip = canLoadAudio;
                break;
            case DinamicPlayer.State.CanClimb:
                audio.clip = canClimbAudio;
                break;
            case DinamicPlayer.State.CanPhase:
                audio.clip = canPhaseAudio;
                break;      
        }
        audio.Play();
    }
}
