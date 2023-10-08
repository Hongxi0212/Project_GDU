using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QPFramework {

   public class HXEvent: UnityEvent, IHXEvent {
      private Dictionary<string, IHXEvent> events;

      public void Clear() {
         events = new Dictionary<string, IHXEvent>();
      }

      public void Register(string eventName, UnityAction listener) {
         IHXEvent thisEvent = null;

         if(events.TryGetValue(eventName, out thisEvent)) {
            if(!(thisEvent is HXEvent)) {
               Debug.Log("This name has already used for another event with different signiture: " + eventName);
               return;
            }
         }
         else {
            thisEvent = new HXEvent();
            (thisEvent as HXEvent).AddListener(listener);
            events.Add(eventName, thisEvent);
         }
      }

      public void Send(string eventName) {
         IHXEvent thisEvent = null;
         if(events.TryGetValue(eventName, out thisEvent)) {
            (thisEvent as HXEvent).Invoke();
         }
      }

      public void Unregister(string eventName, UnityAction listener) {
         if(events == null) {
            Debug.Log("EventPool have not been built!");
            return;
         }

         IHXEvent thisEvent = null;

         if(events.TryGetValue(eventName, out thisEvent)) {
            (thisEvent as HXEvent).RemoveListener(listener);
         }
      }
   }
}