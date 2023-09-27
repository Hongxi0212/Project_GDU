using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace HXFramework {
   public class Event: UnityEvent, IEvent {
      public static readonly Event Pool;

      private Dictionary<string, IEvent> events;

      public void Clear() {
         events = new Dictionary<string, IEvent>();
      }

      public void Register(string eventName, UnityAction listener) {
         IEvent thisEvent = null;

         if(events.TryGetValue(eventName, out thisEvent)) {
            if(!(thisEvent is Event)) {
               Debug.Log("This name has already used for another event with different signiture: " + eventName);
               return;
            }
         }
         else {
            thisEvent = new Event();
            (thisEvent as Event).AddListener(listener);
            events.Add(eventName, thisEvent);
         }
      }

      public void Send(string eventName) {
         IEvent thisEvent = null;
         if(Pool.events.TryGetValue(eventName, out thisEvent)) {
            (thisEvent as Event).Invoke();
         }
      }

      public void Unregister(string eventName, UnityAction listener) {
         if(Pool == null) {
            Debug.Log("The Pool have not been built!");
            return;
         }

         IEvent thisEvent = null;

         if(Pool.events.TryGetValue(eventName, out thisEvent)) {
            (thisEvent as Event).RemoveListener(listener);
         }
      }
   }
}