using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class GameController : MonoBehaviour {

    [Header("speed per second")]
    public float Speed = 10;
    public float TimeInterval = 1;
    public float TimeModifier = 1;
    [Range(0, 1)]
    public float PacketLossChance = 1;
    public Vector3 StartingPoint = Vector3.zero;
    public Color ServerColor = Color.red * 0.2f;
    public Color ClientColor = Color.white;

    private float serverDeltaTime = 0;
    private Vector3 ServerPos = Vector3.zero;
    private List<Vector3> ServerPosList = new List<Vector3>();

    private Vector3 ClientPos = Vector3.zero;
    private List<Vector3> ClientPosList = new List<Vector3>();
    private float ClientAngle = 0;
    private Stopwatch PacketStopwatch = new Stopwatch();
    private bool Running = true;

    void Start() {
        ClientPos = StartingPoint;
        ServerPos = StartingPoint;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            serverDeltaTime = 0;
            ClientPos = StartingPoint;
            ServerPos = StartingPoint;
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
            Gizmos.color = ClientColor;
            Gizmos.DrawWireSphere(ClientPosList[i], 0.1f);
        }
        for (int i = 0; i < ServerPosList.Count; i++) {
            Gizmos.color = ServerColor;
            Gizmos.DrawWireSphere(ServerPosList[i], 0.1f);
        }
    }

    private void ServerSnake() {
        serverDeltaTime += Time.deltaTime;
        if (serverDeltaTime >= TimeInterval) {
            serverDeltaTime -= 60 * TimeInterval * 0.0167f;

            float ang = ClientAngle + Mathf.PI * 0.5f;
            Vector3 dir = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);

            ServerPos += dir * Speed * 60 * TimeInterval * 0.0167f;

            if (Random.value < PacketLossChance) return;
            PacketStopwatch.Reset();
            PacketStopwatch.Start();

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

//        float ang = ClientAngle + Mathf.PI * 0.5f;
//        Vector3 dir = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);
//
//        ClientPos += dir * Speed * Time.deltaTime;

        if (ServerPosList.Count >= 2) {
            int count = ServerPosList.Count;
            float t = PacketStopwatch.ElapsedMilliseconds * 0.001f * TimeModifier / TimeInterval;
            ClientPos = Vector3.Lerp(ServerPosList[count - 2], ServerPosList[count - 1], t);
        }

        ClientPosList.Add(ClientPos);
        if (ClientPosList.Count > 100) {
            ClientPosList.RemoveAt(0);
        }
    }
}
