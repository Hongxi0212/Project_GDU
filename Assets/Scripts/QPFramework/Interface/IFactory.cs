using UnityEngine.Events;

namespace QPFramework {

   public interface IFactory {

      void SetSystem<T>(T system) where T : ISystem;

      void SetModel<T>(T model) where T : IModel;

      void SetUtility<T>(T utility) where T : IUtility;

      T GetModel<T>() where T : class, IModel;

      T GetUtility<T>() where T : class, IUtility;

      T GetSystem<T>() where T : class, ISystem;

      void SendCommand<T>() where T : ICommand, new();

      void SendCommand<T>(T command) where T : ICommand;

      TResult SendQuery<TResult>(IQuery<TResult> query);

      void RegisterEvent(string eventName, UnityAction listener);

      void UnRegisterEvent(string eventName, UnityAction listener);

      void SendEvent(string eventName);
   }
}