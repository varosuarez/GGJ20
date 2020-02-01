using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    private DinamicPlayer player = default;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Floor")) {
            player.groundColliders++;
            player.graceFramesRemaining = player.graceFrames;
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
