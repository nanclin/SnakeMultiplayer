using UnityEngine;

public class GameController : MonoBehaviour {

    [Header("speed per second")]
    public float Speed = 10;

    private float serverDeltaTime = 0;
    private Vector3 ServerPos = Vector3.zero;

    private Vector3 ClientPos = Vector3.zero;
    private float ClientAngle = 0;
    private bool Running;


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            serverDeltaTime = 0;
            ClientPos = Vector3.zero;
            ServerPos = Vector3.zero;
            ClientAngle = 0;
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

            float ang = ClientAngle + Mathf.PI * 0.5f;
            Vector3 dir = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);

            ServerPos += dir * Speed * 30 * 0.0167f;
        }
    }

    private void ClientSnake() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            ClientAngle += 0.1f;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            ClientAngle -= 0.1f;
        }

        float ang = ClientAngle + Mathf.PI * 0.5f;
        Vector3 dir = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);

        ClientPos += dir * Speed * Time.deltaTime;
    }
}
