namespace QPFramework {

   public abstract class AbstractQuery<T>: IQuery<T> {
      private IFactory mContainerSetter;

      public T Do() {
         return OnDo();
      }

      protected abstract T OnDo();

      public IFactory GetFactory() {
         return mContainerSetter;
      }

      public void SetFactory(IFactory containerSetter) {
         mContainerSetter = containerSetter;
      }
   }
}