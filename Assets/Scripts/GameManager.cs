using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    /// <summary>
	/// Punto de spawn inicial
	/// </summary>
	public Transform m_InitialSpawnPoint = null;
    /// <summary>
	/// ¿Quién es el jugador?
	/// </summary>
	public GameObject m_Player = null;

    // Start is called before the first frame update
    void Start()
    {
        RespawnPlayer();
    }

    void Awake()
    {
        if (!m_InitialSpawnPoint)
            Debug.LogWarning("No se ha asignado un punto de spawn inicial");
    }

    /// <summary>
	/// Esta función setea la posición del player, haciéndola coincidir
	/// con la posición del punto de spawn actual
	/// </summary>
	public void RespawnPlayer()
    {
        // Colocamos al player en el punto de spawn actual
        m_Player.transform.position = m_InitialSpawnPoint.position;
        m_Player.transform.rotation = m_InitialSpawnPoint.rotation;
    }
}

