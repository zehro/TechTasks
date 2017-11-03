using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
    private static TaskList _taskList;
    private static ScoreManager _score;

    [SerializeField]
    private new MeshRenderer renderer;

    private int taskID;
    private int pointsWorth;
    private bool isActive;
    private Coroutine current;

    private static TaskList TaskList {
        get {
            if (_taskList == null) {
                _taskList = FindObjectOfType<TaskList>();
            }
            return _taskList;
        }
    }

    private static ScoreManager Score {
        get {
            if (_score == null) {
                _score = FindObjectOfType<ScoreManager>();
            }
            return _score;
        }
    }

    private void Start() {
        renderer.enabled = false;
    }

    // TODO check if other is player
    private void OnTriggerEnter(Collider other) {
        if (renderer.enabled) {
            TaskList.CompleteTask(this.taskID, true);
            Score.AddToScore(pointsWorth);
            StopCoroutine(current);
            renderer.enabled = false;
        }
    }

    public void InitializeValues(int taskID, int duration, int pointsWorth) {
        this.taskID = taskID;
        this.pointsWorth = pointsWorth;
        if (current != null) {
            StopCoroutine(current);
        }
        this.current = StartCoroutine(BecomeActiveWaypoint(duration));
    }

    private IEnumerator BecomeActiveWaypoint(int duration) {
        renderer.enabled = true;
        yield return new WaitForSeconds(duration);
        Score.AddMissedTask();
        renderer.enabled = false;
    }
}