using Pathfinding;
using UnityEngine;

public class ZombieChase : MonoBehaviour {
//        public Transform targetPosition;

    private Seeker seeker;
    private CharacterController controller;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;
    private GameObject player;
    private GameObject target;
    private IAstarAI astarAi;
    private bool targetChangable = true;

    public void Start() {
        player = GameObject.Find("Player");
        target = GameObject.Find("Target");
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        astarAi = GetComponent<IAstarAI>();
    }

    public void OnPathComplete(Path p) {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error) {
            path = p;

            currentWaypoint = 0;
        }
    }

    public void Update() {
        if (path == null) {
            Debug.Log("Path is null");
            return;
        }

        var playerPosition = player.transform.position;
        Vector3 direction = playerPosition - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        bool playerClosedBy = Vector3.Distance(playerPosition, transform.position) < 10;
        bool closeToBase = Vector3.Distance(target.transform.position, transform.position) < 3;
        reachedEndOfPath = false;
        float distanceToWaypoint;
        while (true) {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        if (targetChangable) {
            if (closeToBase) {
                astarAi.destination = target.transform.position;
                targetChangable = false;
                Debug.Log("Close to base, disable target changing");
            }
        
            if (playerClosedBy && angle < 30) {
                astarAi.destination = player.transform.position;
                Debug.Log("Seen Player, following");

            } else if(!playerClosedBy) {
                astarAi.destination = target.transform.position;
                Debug.Log("Loss Sight of player, going to base");
            } 
        }

        

        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed * speedFactor;
        controller.SimpleMove(velocity);
    }
}