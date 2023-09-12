using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework {

    #region ContainerSetter

    public interface IContainerSetter {

        void RegisterSystem<T>(T system) where T : ISystem;

        void RegisterModel<T>(T model) where T : IModel;

        void RegisterUtility<T>(T utility) where T : IUtility;

        T GetModel<T>() where T : class, IModel;

        T GetUtility<T>() where T : class, IUtility;

        T GetSystem<T>() where T : class, ISystem;

        void SendCommand<T>() where T : ICommand, new();

        void SendCommand<T>(T command) where T : ICommand;

        TResult SendQuery<TResult>(IQuery<TResult> query);

        void SendEvent<T>() where T : new();

        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        void UnRegisterEvent<T>(Action<T> onEvent);
    }

    public abstract class ContainerSetter<T> : IContainerSetter where T : ContainerSetter<T>, new() {
        private static T mContainerSetter;

        private IOCContainer mContainer = new IOCContainer();
        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();
        private List<IModel> mModels = new List<IModel>();
        private List<ISystem> mSystems = new List<ISystem>();

        private bool mInited = false;

        public static Action<T> OnRegisterPatch = containerSetter => { };

        public static IContainerSetter Interface {
            get {
                if (mContainerSetter == null) {
                    MakeSureContainer();
                }

                return mContainerSetter;
            }
        }

        protected abstract void Init();

        private static void MakeSureContainer() {
            if (mContainerSetter == null) {
                mContainerSetter = new T();
                mContainerSetter.Init();

                OnRegisterPatch?.Invoke(mContainerSetter);

                foreach (var containerSetterModel in mContainerSetter.mModels) {
                    containerSetterModel.Init();
                }

                foreach (var containerSetterSystem in mContainerSetter.mSystems) {
                    containerSetterSystem.Init();
                }
                mContainerSetter.mModels.Clear();
                mContainerSetter.mInited = true;
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : IModel {
            model.SetContainerSetter(this);
            mContainer.Register<TModel>(model);

            if (!mInited) {
                mModels.Add(model);
            }
            else {
                model.Init();
            }
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem {
            system.SetContainerSetter(this);
            mContainer.Register<TSystem>(system);

            if (!mInited) {
                mSystems.Add(system);
            }
            else {
                system.Init();
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility {
            mContainer.Register<TUtility>(utility);
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility {
            return mContainer.Get<TUtility>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel {
            return mContainer.Get<TModel>();
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem {
            return mContainer.Get<TSystem>();
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new() {
            var command = new TCommand();
            command.SetContainerSetter(this);
            command.Execute();
            command.SetContainerSetter(null);
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand {
            command.SetContainerSetter(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query) {
            query.SetContainerSetter(this);
            return query.Do();
        }

        public void SendEvent<TEvent>() where TEvent : new() {
            mTypeEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e) {
            mTypeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent) {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent) {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }
    }

    #endregion

    #region Controller

    public interface IController : IBelongToContainerSetter, ICanSendQuery, ICanSendCommand, ICanGetModel, ICanGetSystem, ICanRegisterEvent {

    }

    #endregion

    #region System

    public interface ISystem : IBelongToContainerSetter, ICanSetContainerSetter, ICanGetModel, ICanGetUtility, ICanSendEvent, ICanRegisterEvent, ICanGetSystem {

        void Init();
    }

    public abstract class AbstractSystem : ISystem {
        private IContainerSetter mContainerSetter;

        IContainerSetter IBelongToContainerSetter.GetContainerSetter() {
            return mContainerSetter;
        }

        void ISystem.Init() {
            OnInit();
        }

        void ICanSetContainerSetter.SetContainerSetter(IContainerSetter containerSetter) {
            mContainerSetter = containerSetter;
        }

        protected abstract void OnInit();
    }

    #endregion

    #region Model
    public interface IModel : IBelongToContainerSetter, ICanSetContainerSetter, ICanGetUtility {

        void Init();
    }

    public abstract class AbstractModel : IModel {
        private IContainerSetter mContainerSetter;

        IContainerSetter IBelongToContainerSetter.GetContainerSetter() {
            return mContainerSetter;
        }

        //接口阉割
        void IModel.Init() {
            OnInit();
        }

        void ICanSetContainerSetter.SetContainerSetter(IContainerSetter containerSetter) {
            mContainerSetter = containerSetter;
        }

        protected abstract void OnInit();
    }

    #endregion

    #region Utility

    public interface IUtility {

    }

    #endregion

    #region Command

    public interface ICommand : IBelongToContainerSetter, ICanSetContainerSetter, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendQuery, ICanSendCommand, ICanSendEvent {

        void Execute();
    }

    public abstract class AbstractCommand : ICommand {
        private IContainerSetter mContainerSetter;

        void ICommand.Execute() {
            OnExecute();
        }

        IContainerSetter IBelongToContainerSetter.GetContainerSetter() {
            return mContainerSetter;
        }

        void ICanSetContainerSetter.SetContainerSetter(IContainerSetter containerSetter) {
            mContainerSetter = containerSetter;
        }

        protected abstract void OnExecute();
    }

    #endregion

    #region Query

    public interface IQuery<TResult> : IBelongToContainerSetter, ICanSetContainerSetter, ICanGetModel, ICanSendQuery {

        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T> {
        private IContainerSetter mContainerSetter;

        public T Do() {
            return OnDo();
        }

        protected abstract T OnDo();

        public IContainerSetter GetContainerSetter() {
            return mContainerSetter;
        }

        public void SetContainerSetter(IContainerSetter containerSetter) {
            mContainerSetter = containerSetter;
        }
    }

    #endregion

    #region Rule

    public interface IBelongToContainerSetter {

        IContainerSetter GetContainerSetter();
    }

    public interface ICanSetContainerSetter {

        void SetContainerSetter(IContainerSetter containerSetter);
    }

    public interface ICanGetSystem : IBelongToContainerSetter {
    }

    public static class CanGetSystemExtension {

        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem {
            return self.GetContainerSetter().GetSystem<T>();
        }
    }

    public interface ICanGetModel : IBelongToContainerSetter {
    }

    public static class CanGetModelExtension {

        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel {
            return self.GetContainerSetter().GetModel<T>();
        }
    }

    public interface ICanGetUtility : IBelongToContainerSetter {
    }

    public static class CanGetUtilityExtension {

        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility {
            return self.GetContainerSetter().GetUtility<T>();
        }
    }

    public interface ICanRegisterEvent : IBelongToContainerSetter {
    }

    public static class CanRegisterEventExtension {

        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) {
            return self.GetContainerSetter().RegisterEvent<T>(onEvent);
        }

        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) {
            self.GetContainerSetter().UnRegisterEvent<T>(onEvent);
        }
    }

    public interface ICanSendEvent : IBelongToContainerSetter {
    }

    public static class CanSendEventExtension {

        public static void SendEvent<T>(this ICanSendEvent self) where T : new() {
            self.GetContainerSetter().SendEvent<T>();
        }

        public static void SendEvent<T>(this ICanSendEvent self, T e) {
            self.GetContainerSetter().SendEvent<T>(e);
        }
    }

    public interface ICanSendCommand : IBelongToContainerSetter {
    }

    public static class CanSendCommandExtension {

        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new() {
            self.GetContainerSetter().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand {
            self.GetContainerSetter().SendCommand<T>(command);
        }
    }

    public interface ICanSendQuery : IBelongToContainerSetter {
    }

    public static class CanSendQueryExtension {

        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query) {
            return self.GetContainerSetter().SendQuery(query);
        }
    }

    #endregion

    #region TypeEvent

    public interface ITypeEventSystem {

        void Send<T>() where T : new();

        void Send<T>(T e);

        IUnRegister Register<T>(Action<T> onEvent);

        void UnRegister<T>(Action<T> onEvent);
    }

    public interface IUnRegister {

        void UnRegister();
    }

    public static class UnRegisterExtension {

        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject) {
            var trigger = gameObject.GetComponent<UnRegisterDestroyTrigger>();

            if (!trigger) {
                trigger = gameObject.AddComponent<UnRegisterDestroyTrigger>();
            }
            trigger.AddUnRegister(unRegister);
        }
    }

    public class TypeEventSystem : ITypeEventSystem {
        public static readonly TypeEventSystem Global = new TypeEventSystem();

        private Dictionary<Type, IRegistration> mEventRegistrations = new Dictionary<Type, IRegistration>();

        public interface IRegistration {
        }

        public class Registration<T> : IRegistration {
            public Action<T> OnEvent = e => { };
        }

        public IUnRegister Register<T>(Action<T> onEvent) {
            var type = typeof(T);
            IRegistration registrations;

            if (mEventRegistrations.TryGetValue(type, out registrations)) {
            }
            else {
                registrations = new Registration<T>();
                mEventRegistrations.Add(type, registrations);
            }

            (registrations as Registration<T>).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>() {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        public void Send<T>() where T : new() {
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e) {
            var type = typeof(T);
            IRegistration registrations;

            if (mEventRegistrations.TryGetValue(type, out registrations)) {
                (registrations as Registration<T>).OnEvent(e);
            }
        }

        public void UnRegister<T>(Action<T> onEvent) {
            var type = typeof(T);
            IRegistration registrations;

            if (mEventRegistrations.TryGetValue(type, out registrations)) {
                (registrations as Registration<T>).OnEvent -= onEvent;
            }
        }
    }

    public class UnRegisterDestroyTrigger : MonoBehaviour {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister) {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy() {
            foreach (var unRegister in mUnRegisters) {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    public struct TypeEventSystemUnRegister<T> : IUnRegister {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister() {
            TypeEventSystem.UnRegister<T>(OnEvent);
            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    public interface IOnEvent<T> {
        void OnEvent(T e);
    }

    public static class OnGlobalEventExtension {
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct {
            return TypeEventSystem.Global.Register<T>(self.OnEvent);
        }

        public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct {
            TypeEventSystem.Global.UnRegister<T>(self.OnEvent);
        }
    }

    #endregion

    #region IOCContainer

    public class IOCContainer {
        private Dictionary<Type, object> mInstance = new Dictionary<Type, object>();

        public void Register<T>(T instance) {
            var key = typeof(T);
            if (mInstance.ContainsKey(key)) {
                mInstance[key] = instance;
            }
            else {
                mInstance.Add(key, instance);
            }
        }

        public T Get<T>() where T : class {
            var key = typeof(T);

            if (mInstance.TryGetValue(key, out var retInstance)) {
                return retInstance as T;
            }

            return null;
        }
    }

    #endregion

    #region BindableProperty

    public class BindableProperty<T> {
        public BindableProperty(T defaultValue = default(T)) {
            mValue = defaultValue;
        }

        private T mValue = default(T);

        public T Value {
            get {
                return mValue;
            }
            set {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;

                mValue = value;
                mOnValueChanged?.Invoke(value);
            }
        }

        private Action<T> mOnValueChanged = (v) => { };

        public IUnRegister Register(Action<T> onValueChanged) {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>() {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged) {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }

        public static implicit operator T(BindableProperty<T> property) {
            return property.Value;
        }

        public override string ToString() {
            return Value.ToString();

        }

        public void UnRegister(Action<T> onValueChanged) {
            mOnValueChanged -= onValueChanged;
        }

        public class BindablePropertyUnRegister<TProperty> : IUnRegister {
            public BindableProperty<TProperty> BindableProperty { get; set; }

            public Action<TProperty> OnValueChanged { get; set; }

            public void UnRegister() {
                BindableProperty.UnRegister(OnValueChanged);

                BindableProperty = null;
                OnValueChanged = null;
            }
        }
    }

    #endregion

}
