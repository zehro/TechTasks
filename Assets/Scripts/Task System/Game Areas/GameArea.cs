using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameArea : MonoBehaviour {

    [SerializeField]
    private GameObject[] waypointHolders;

    private Waypoint[][] waypointGroups;

    private void Start() {
        waypointGroups = new Waypoint[waypointHolders.Length][];

        int index = 0;
        foreach (GameObject holder in waypointHolders) {
            waypointGroups[index++] = holder.GetComponentsInChildren<Waypoint>();
        }
    }

    /// <summary>
    /// Gets the random waypoints. Randomly picks one from each waypoint group.
    /// </summary>
    /// <param name="count">Number of waypoints to return.</param>
    /// <returns>Returns count number of waypoints</returns>
    public Waypoint[] GetRandomWaypoints(int count) {
        Waypoint[][] selectedWaypointGroups = waypointGroups.OrderBy(x => Random.value).Take(count).ToArray();
        Waypoint[] selectedWaypoints = new Waypoint[count];

        for (int i = 0; i < selectedWaypoints.Length; i++) {
            Waypoint[] waypointGroup = selectedWaypointGroups[i];
            selectedWaypoints[i] = waypointGroup[Random.Range(0, waypointGroup.Length)];
        }
        return selectedWaypoints;
    }
}