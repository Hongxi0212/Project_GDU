using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QPFramework {

   public abstract class Factory<T>: IFactory where T : Factory<T>, new() {
      private static T factory;

      private ObjectContainer container = new ObjectContainer();
      private IHXEvent eventpool = new HXEvent();
      private List<IModel> models = new List<IModel>();
      private List<ISystem> systems = new List<ISystem>();

      private bool inited = false;

      public static Action<T> OnRegisterPatch = fatory => { };

      public static IFactory Line {
         get {
            if(factory == null) {
               Build();
            }

            return factory;
         }
      }

      protected abstract void Init();

      private static void Build() {
         if(factory == null) {
            factory = new T();
            factory.Init();

            OnRegisterPatch?.Invoke(factory);

            foreach(var model in factory.models) {
               model.Init();
            }

            foreach(var system in factory.systems) {
               system.Init();
            }

            factory.models.Clear();
            factory.eventpool.Clear();

            factory.inited = true;
         }
      }

      public void SetModel<TModel>(TModel model) where TModel : IModel {
         model.SetFactory(this);
         container.Inject<TModel>(model);

         if(!inited) {
            models.Add(model);
         }
         else {
            model.Init();
         }
      }

      public void SetSystem<TSystem>(TSystem system) where TSystem : ISystem {
         system.SetFactory(this);
         container.Inject<TSystem>(system);

         if(!inited) {
            systems.Add(system);
         }
         else {
            system.Init();
         }
      }

      public void SetUtility<TUtility>(TUtility utility) where TUtility : IUtility {
         container.Inject<TUtility>(utility);
      }

      public TUtility GetUtility<TUtility>() where TUtility : class, IUtility {
         return container.Extract<TUtility>();
      }

      public TModel GetModel<TModel>() where TModel : class, IModel {
         return container.Extract<TModel>();
      }

      public TSystem GetSystem<TSystem>() where TSystem : class, ISystem {
         return container.Extract<TSystem>();
      }

      public void SendCommand<TCommand>() where TCommand : ICommand, new() {
         var command = new TCommand();
         command.SetFactory(this);
         command.Execute();
         command.SetFactory(null);
      }

      public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand {
         command.SetFactory(this);
         command.Execute();
      }

      public TResult SendQuery<TResult>(IQuery<TResult> query) {
         query.SetFactory(this);
         return query.Do();
      }

      public void RegisterEvent(string eventName, UnityAction listener) {
         eventpool.Register(eventName, listener);
      }

      public void UnRegisterEvent(string eventName, UnityAction listener) {
         eventpool.Unregister(eventName, listener);
      }

      public void SendEvent(string eventName) {
         eventpool.Send(eventName);
      }

      public void ClearEvents() {
         eventpool.Clear();
      }
   }
}