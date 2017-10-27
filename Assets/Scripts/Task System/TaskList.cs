using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskList : MonoBehaviour {

    [SerializeField]
    private Task taskPrefab;

    // int is the instance id
    private IDictionary<int, Task> tasks;

    private void Start() {
        this.tasks = new Dictionary<int, Task>();
    }

    public void AddTask(string objectiveName, int initialTime) {
        Task newTask = GameObject.Instantiate<Task>(taskPrefab);
        newTask.InitializeValues(objectiveName, initialTime);
        newTask.transform.SetParent(this.transform);
        tasks.Add(newTask.gameObject.GetInstanceID(), newTask);
        StartCoroutine(newTask.PlayAddEffect());
    }

    // Used for testing purposes
    public void AddRandomTask() {
        AddTask(Path.GetRandomFileName().Substring(0, 6), UnityEngine.Random.Range(5, 15));
    }

    public void Update() {
        SortTasksByTimeRemaining();
        RemoveTimedOutTasks();
    }

    private void SortTasksByTimeRemaining() {
        Task[] children = GetComponentsInChildren<Task>();
        foreach (Task child in children) {
            child.transform.SetParent(null);
        }
        Array.Sort(children);
        foreach (Task child in children) {
            child.transform.SetParent(this.transform);
        }
    }

    private void RemoveTimedOutTasks() {
        List<int> timedOutIds = new List<int>();
        foreach (KeyValuePair<int, Task> pair in tasks) {
            if (pair.Value.IsExpired) {
                timedOutIds.Add(pair.Key);
                // TODO game over on essential tasks
            }
        }
        foreach (int id in timedOutIds) {
            Task taskToRemove = tasks[id];
            StartCoroutine(taskToRemove.PlayDestroyEffect(() => Destroy(taskToRemove.gameObject)));
            tasks.Remove(id);
        }
    }
}