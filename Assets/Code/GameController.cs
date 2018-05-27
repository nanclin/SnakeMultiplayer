using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    [Header("speed per second")]
    public float Speed = 10;
    public float TimeInterval = 1;

    private float serverDeltaTime = 0;
    private Vector3 ServerPos = Vector3.zero;
    private List<Vector3> ServerPosList = new List<Vector3>();

    private Vector3 ClientPos = Vector3.zero;
    private List<Vector3> ClientPosList = new List<Vector3>();
    private float ClientAngle = 0;
    private bool Running;


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            serverDeltaTime = 0;
            ClientPos = Vector3.zero;
            ServerPos = Vector3.zero;
            ClientPosList.Clear();
            ServerPosList.Clear();
            ClientAngle = 0;
            Running = true;
        }
        if (!Running) return;
        ServerSnake();
        ClientSnake();
    }

    void OnDrawGizmos() {
        for (int i = 0; i < ClientPosList.Count; i++) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(ClientPosList[i], 0.1f);
        }
        for (int i = 0; i < ServerPosList.Count; i++) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ServerPosList[i], 0.1f);
        }
    }

    private void ServerSnake() {
        serverDeltaTime += Time.deltaTime;
        if (serverDeltaTime >= TimeInterval) {
            serverDeltaTime -= 60 * TimeInterval * 0.0167f;
            Debug.LogFormat("ServerSnake() - {0}", serverDeltaTime);

            float ang = ClientAngle + Mathf.PI * 0.5f;
            Vector3 dir = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);

            ServerPos += dir * Speed * 60 * TimeInterval * 0.0167f;
            ServerPosList.Add(ServerPos);
            if (ServerPosList.Count > 10) {
                ServerPosList.RemoveAt(0);
            }
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
        ClientPosList.Add(ClientPos);
        if (ClientPosList.Count > 100) {
            ClientPosList.RemoveAt(0);
        }
    }
}
