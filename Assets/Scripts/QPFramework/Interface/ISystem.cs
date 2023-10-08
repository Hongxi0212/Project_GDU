namespace QPFramework {

   public interface ISystem: IFactoryGetable, IFactorySetable, IModelGetable, IUtilityGetable, IEventUseable, ISystemGetable {

      void Init();
   }
}