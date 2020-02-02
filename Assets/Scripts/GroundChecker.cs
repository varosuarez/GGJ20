using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    private DinamicPlayer player = default;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Floor")) {
            player.groundColliders++;
            player.graceFramesRemaining = player.graceFrames;
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.CompareTag("Floor")) {
            if (player.groundColliders == 0) {
                player.groundColliders++;
            }
            player.graceFramesRemaining = player.graceFrames;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("Floor")) {
            if (player.groundColliders > 0) {
                player.groundColliders--;
            }
            player.graceFramesRemaining = player.graceFrames;
        }
    }
}
