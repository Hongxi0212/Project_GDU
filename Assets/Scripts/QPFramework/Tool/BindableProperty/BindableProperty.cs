using System;

namespace QPFramework {

   /// <summary>
   ///
   /// 工具类，是数据+数据变更事件的结合体，以减少代码量
   /// 为了避免Model层每增加一个属性（数据）就要重复一次以下代码的重复性工作，用泛型来建立工具类并用于创建属性
   /// </summary>
   /// <typeparam name="T"></typeparam>
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
            if(value == null && mValue == null)
               return;
            if(value != null && value.Equals(mValue))
               return;

            mValue = value;
            mOnValueChanged?.Invoke(value);
         }
      }

      private Action<T> mOnValueChanged = (v) => { };

      public void Register(Action<T> onValueChanged) {
         mOnValueChanged += onValueChanged;
      }

      public void RegisterWithInitValue(Action<T> onValueChanged) {
         onValueChanged(mValue);
      }

      public void UnRegister(Action<T> onValueChanged) {
         mOnValueChanged -= onValueChanged;
      }

      public static implicit operator T(BindableProperty<T> property) {
         return property.Value;
      }

      public override string ToString() {
         return Value.ToString();
      }
   }
}