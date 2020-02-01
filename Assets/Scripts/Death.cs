using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    public AudioClip m_deathSound;

    private AudioSource m_audio;

    /// <summary>
    /// Tiempo de animación
    /// </summary>
    public float animationTime = 3.0f;

    // The time at which the animation started.
    private float startTime;

    private Transform end;
    private Transform begining;
    private bool update = false;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
    }

    public virtual void OnDeath(Transform spawnPoint)
    {
        if (m_deathSound != null)
        {
            m_audio.clip = m_deathSound;
            m_audio.Play();
        }

        startTime = Time.time;
        end = spawnPoint;
        begining = transform.transform;

        update = true;
    }

    void Update()
    {
        if (update)
        {
            // The center of the arc
            Vector3 center = (begining.position + end.position) * 0.5F;

            // move the center a bit downwards to make the arc vertical
            center -= new Vector3(0, 1, 0);

            // Interpolate over the arc relative to center
            Vector3 riseRelCenter = begining.position - center;
            Vector3 setRelCenter = end.position - center;

            // The fraction of the animation that has happened so far is
            // equal to the elapsed time divided by the desired time for
            // the total journey.
            float fracComplete = (Time.time - startTime) / animationTime;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;

            if (transform.transform == end)
            {
                update = false;
            }
        }
    }

}