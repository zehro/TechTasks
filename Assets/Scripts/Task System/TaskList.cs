using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TaskList : MonoBehaviour {

    [SerializeField]
    private Task taskPrefab;

    // int is the instance id
    private IDictionary<int, Task> tasks;

    /// <summary>
    /// Adds the task.
    /// </summary>
    /// <param name="objectiveName">Name of the objective.</param>
    /// <param name="initialTime">The initial time.</param>
    /// <returns>ID of the task just added.</returns>
    public int AddTask(string objectiveName, int initialTime) {
        Task newTask = Instantiate<Task>(taskPrefab);
        AppendTaskToUI(newTask, objectiveName, initialTime);
        return newTask.gameObject.GetInstanceID();
    }

    public void CompleteTask(int taskID, bool isSuccessful) {
        RemoveTask(taskID, isSuccessful);
    }

    private void AppendTaskToUI(Task newTask, string objectiveName, int initialTime) {
        newTask.InitializeValues(objectiveName, initialTime);
        newTask.transform.SetParent(this.transform);
        tasks.Add(newTask.gameObject.GetInstanceID(), newTask);
        StartCoroutine(newTask.PlayAddEffect());
    }

    private void RemoveTask(int id, bool isSuccessful) {
        if (tasks.ContainsKey(id)) {
            Task taskToRemove = tasks[id];
            StartCoroutine(taskToRemove.PlayDestroyEffect(isSuccessful, () => Destroy(taskToRemove.gameObject)));
            tasks.Remove(id);
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
            RemoveTask(id, false);
        }
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

    private void Start() {
        this.tasks = new Dictionary<int, Task>();
    }

    private void Update() {
        SortTasksByTimeRemaining();
        RemoveTimedOutTasks();
    }
}