using QPFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QPFramework {

   public class TimeSystem: AbstractSystem, ITimeSystem {
      public float CurrentTime { get; private set; }

      private LinkedList<Task> allTasks = new LinkedList<Task>();

      public void AddDelayTask(float delaySeconds, Action onDelayFinish) {
         var task = new Task() {
            DelaySeconds = delaySeconds,
            OnDelayFinish = onDelayFinish,
            State = TaskState.NotStart
         };
      }

      protected override void OnInit() {
         var monoTSGO = new GameObject("TimeSystem");
         var monoTS = monoTSGO.AddComponent<MonoTimeSystem>();

         monoTS.OnUpdate += TimeUpdate;

         CurrentTime = 0;
      }

      private void TimeUpdate() {
         CurrentTime+= Time.deltaTime;

         if(allTasks.Count > 0) {
            var currentTask = allTasks.First;

            while(currentTask != null) {
               var nextTask=currentTask.Next;
               var task = currentTask.Value;

               ProcessTask(task);

               currentTask = nextTask;
            }
         }
      }

      private void ProcessTask(Task task) {
         if(task.State == TaskState.NotStart) {
            task.State = TaskState.Started;
            task.StartTime = CurrentTime;
            task.FinishTime = CurrentTime + task.DelaySeconds;
         }
         else if(task.State == TaskState.Started) {
            if(CurrentTime>=task.FinishTime) {
               task.State = TaskState.Finish;
               task.OnDelayFinish();

               task.OnDelayFinish = null;

               allTasks.Remove(task);
            }
         }
      }
   }

   public class MonoTimeSystem: MonoBehaviour {
      public event Action OnUpdate;

      private void Update() {
         OnUpdate?.Invoke();
      }
   }
}