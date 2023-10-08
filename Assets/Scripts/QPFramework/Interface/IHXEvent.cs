using UnityEngine.Events;

namespace QPFramework {

   public interface IHXEvent {

      void Send(string eventName);

      void Register(string eventName, UnityAction onEvent);

      void Unregister(string eventName, UnityAction onEvnet);

      void Clear();
   }
}