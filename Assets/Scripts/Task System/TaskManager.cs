using System.Collections;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    [SerializeField]
    private TaskList taskList;

    [SerializeField]
    private float timeUntilFirstTaskIsAssigned;

    [SerializeField]
    private Waypoint[] possibleWaypoints;

    [SerializeField]
    private int taskDuration;

    [SerializeField]
    private int scorePerTask;

    private void Start() {
        StartCoroutine(StartAssigningTasks());
    }

    private IEnumerator StartAssigningTasks() {
        yield return new WaitForSeconds(timeUntilFirstTaskIsAssigned);
        foreach (Waypoint waypoint in possibleWaypoints) {
            AddTask(waypoint, scorePerTask);
            yield return new WaitForSeconds(10);
        }
    }

    private void AddTask(int duration, Waypoint waypoint, string objectiveName, int score) {
        int id = taskList.AddTask(objectiveName, duration);
        waypoint.InitializeValues(id, duration, score);
    }

    private void AddTask(Waypoint waypoint, int score) {
        AddTask(taskDuration, waypoint, "Go to " + waypoint.name, score);
    }
}