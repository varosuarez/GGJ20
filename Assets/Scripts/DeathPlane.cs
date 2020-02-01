using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{

    /// <summary>
	/// Punto de spawn inicial
	/// </summary>
	public Transform m_SpawnPoint = null;

    /// <summary>
	/// Si algo choca contra nosotros, comprobaremos si es el player
	/// </summary>
	/// <param name="other">
	/// Objeto que ha entrado en el trigger <see cref="Collider"/>
	/// </param>
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Death>() != null)  //Si no se comprueba apareceran demasiado warnings
            other.SendMessage("OnDeath", m_SpawnPoint);
    }
}

