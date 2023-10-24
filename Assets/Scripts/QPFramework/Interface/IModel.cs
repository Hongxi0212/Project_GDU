namespace QPFramework {

   public interface IModel: IFactoryGetable, IFactorySetable, IUtilityGetable {

      void Init();

      //think about stipulate reading and writing json and xml files Methods for data persistence
   }
}