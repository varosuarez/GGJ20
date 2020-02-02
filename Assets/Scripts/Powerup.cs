using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private DinamicPlayer.State power = DinamicPlayer.State.Powerless;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<DinamicPlayer>().SetState(power);
            col.GetComponent<DinamicPlayer>().DiscoverUI(power);
            Destroy(gameObject);
        }
    }
}
