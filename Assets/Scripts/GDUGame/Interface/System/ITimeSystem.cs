using QPFramework;
using System;

namespace QPFramework {

   public interface ITimeSystem: ISystem {
      float CurrentTime { get; }

      void AddDelayTask(float delaySeconds, Action onDelayFinish);
   }
}