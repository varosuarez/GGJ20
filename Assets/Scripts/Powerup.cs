using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private DinamicPlayer.State power = DinamicPlayer.State.Powerless;
    public AudioClip m_powerupSound;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<DinamicPlayer>().SetState(power);
            col.GetComponent<DinamicPlayer>().DiscoverUI(power);
            if (m_powerupSound != null)
            {
                AudioSource m_audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
                m_audio.clip = m_powerupSound;
                m_audio.Play();
            }
            Destroy(gameObject);
        }
    }
}
