using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour, InputMaster.IPlayerActions
{

    private GameManager.Phase phase =  GameManager.Phase.RED_RIGHT;
    private bool carrying = false;
    private GameObject objectToCatch = null;
    private bool availableCatch = false;
    private Vector3 m_EnterScale = Vector3.one;

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (carrying)
        {
            //DROP
        }
        else
        {
            //GRAB
            if (availableCatch)
            {
                m_EnterScale = objectToCatch.transform.localScale;
                objectToCatch.transform.parent = transform.transform;
                carrying = true;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftPhase(InputAction.CallbackContext context)
    {
        if (phase == GameManager.Phase.RED_RIGHT)
        {
            phase = GameManager.Phase.RED_RIGHT;
            //TODO: Cambio color
        }
    }

    public void OnRightPhase(InputAction.CallbackContext context)
    {
        if (phase != GameManager.Phase.RED_RIGHT)
        {
            phase = GameManager.Phase.BLUE_LEFT;
            //TODO: Cambio color
        }
    }

    public GameManager.Phase GetPhase()
    {
        return phase;
    }

    public void EnableCatch(GameObject objeto)
    {
        availableCatch = true;
        objectToCatch = objeto;

    }

    public void DisableCatch()
    {
        availableCatch = false;
        objectToCatch = null;
    }

}
