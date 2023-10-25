using System;

namespace QPFramework {

   public enum TaskState {
      NotStart,
      Started,
      Finish
   }

   public class Task {
      public float DelaySeconds { get; set; }

      public Action OnDelayFinish { get; set; }

      public float StartTime { get; set; }

      public float FinishTime { get; set; }

      public TaskState State { get; set; }
   }
}