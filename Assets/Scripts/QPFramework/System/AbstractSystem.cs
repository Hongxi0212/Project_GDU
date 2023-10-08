namespace QPFramework {

   public abstract class AbstractSystem: ISystem {
      private IFactory mContainerSetter;

      IFactory IFactoryGetable.GetFactory() {
         return mContainerSetter;
      }

      void ISystem.Init() {
         OnInit();
      }

      void IFactorySetable.SetFactory(IFactory containerSetter) {
         mContainerSetter = containerSetter;
      }

      protected abstract void OnInit();
   }
}