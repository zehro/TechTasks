using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TaskBundle {

    // tasks are randomly picked from current game area
    public int NumberOfTasks;

    // seconds until the next group of tasks appears
    public int SecondsUntilNextTaskGroup;

    // seconds allotted for these tasks
    public int SecondsPerTask;
}

public class TaskManager : MonoBehaviour {

    [SerializeField]
    private GameArea current;

    [SerializeField]
    private TaskList taskList;

    [SerializeField]
    private float timeUntilFirstTaskIsAssigned;

    [SerializeField]
    private Waypoint[] possibleWaypoints;

    [SerializeField]
    private PauseMenuManager pause;

    [SerializeField]
    private int taskDuration;

    [SerializeField]
    private int scorePerTask;

    [SerializeField]
    private TaskBundle[] tasksToDo;

    private void Start() {
        StartCoroutine(StartAssigningTasks());
    }

    private IEnumerator StartAssigningTasks() {
        int highestPossibleScore = 0;
        yield return new WaitForSeconds(timeUntilFirstTaskIsAssigned);
        foreach (TaskBundle task in tasksToDo) {
            Waypoint[] waypoints = current.GetRandomWaypoints(task.NumberOfTasks);
            foreach (Waypoint waypoint in waypoints) {
                highestPossibleScore += scorePerTask;
                AddTask(task.SecondsPerTask, waypoint, waypoint.WaypointName, scorePerTask);
            }
            yield return new WaitForSeconds(task.SecondsUntilNextTaskGroup);
        }
        pause.DoGameOver(highestPossibleScore);
    }

    private void AddTask(int duration, Waypoint waypoint, string objectiveName, int score) {
        int id = taskList.AddTask(objectiveName, duration);
        waypoint.InitializeValues(id, duration, score);
    }

    private void AddTask(Waypoint waypoint, int score) {
        AddTask(taskDuration, waypoint, waypoint.WaypointName, score);
    }
}