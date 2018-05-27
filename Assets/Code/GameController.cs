using UnityEngine;

public class GameController : MonoBehaviour {

    [Header("speed per second")]
    public float Speed = 10;

    private float serverDeltaTime = 0;
    private Vector3 ServerPos = Vector3.zero;

    private Vector3 ClientPos = Vector3.zero;
    private bool Running;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Running = true;
        }
        if (!Running) return;
        ServerSnake();
        ClientSnake();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ServerPos, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(ClientPos, 0.1f);
    }

    private void ServerSnake() {
        serverDeltaTime += Time.deltaTime;
        if (serverDeltaTime >= 0.5f) {
            serverDeltaTime -= 30 * 0.0167f;
            Debug.LogFormat("ServerSnake() - {0}", serverDeltaTime);

            ServerPos += Vector3.up * Speed * 30 * 0.0167f;
        }
    }

    private void ClientSnake() {
        ClientPos += Vector3.up * Speed * Time.deltaTime;
    }
}
