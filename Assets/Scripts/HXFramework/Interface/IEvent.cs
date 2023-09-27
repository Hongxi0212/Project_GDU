using System;
using UnityEngine.Events;

namespace HXFramework {
   public interface IEvent {
      void Send(string eventName);

      void Register(string eventName, UnityAction onEvent);

      void Unregister(string eventName, UnityAction onEvnet);

      void Clear();
   }
}