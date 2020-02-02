using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private bool jump = true;
    private bool climb = true;

    public Sprite jumpActive;
    public Sprite jumpDeactivate;

    public Sprite grabActive;
    public Sprite grabDeactivate;

    public Sprite climbActive;
    public Sprite climbDeactivate;

    public Sprite phaseActive;
    public Sprite phaseDeactivate;

    public Image rightActive;
    public Image rightDeactivate;

    public Image leftActive;
    public Image leftDeactivate;

    public void Awake()
    {
        switch (GameObject.FindGameObjectWithTag("Player").GetComponent<DinamicPlayer>().GetState())
        {
            case DinamicPlayer.State.CanPhase:
                discoverPhase();
                discoverClimb();
                discoverGrab();
                discoverJump();
                break;
            case DinamicPlayer.State.CanClimb:
                discoverClimb();
                discoverGrab();
                discoverJump();
                break;
            case DinamicPlayer.State.CanLoad:
                discoverGrab();
                discoverJump();
                break;
            case DinamicPlayer.State.CanJump:
                discoverJump();
                break;
        }
    }

    public void changeRight()
    {
        if (jump)
        {
            jump = false;
            rightActive.sprite = grabActive;
            rightDeactivate.sprite = jumpDeactivate;
        }
        else
        {
            jump = true;
            rightActive.sprite = jumpActive;
            rightDeactivate.sprite = grabDeactivate;
        }
    }

    public void changeLeft()
    {
        if (climb)
        {
            climb = false;
            leftActive.sprite = phaseActive;
            leftDeactivate.sprite = climbDeactivate;
        }
        else
        {
            climb = true;
            leftActive.sprite = climbActive;
            leftDeactivate.sprite = phaseDeactivate;
        }
    }

    public void discoverJump()
    {
        rightActive.sprite = jumpActive;
    }

    public void discoverGrab()
    {
        rightDeactivate.sprite = grabDeactivate;
    }

    public void discoverClimb()
    {
        leftActive.sprite = climbActive;
    }

    public void discoverPhase()
    {
        leftDeactivate.sprite = phaseDeactivate;
    }


}
