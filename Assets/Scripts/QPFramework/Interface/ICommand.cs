namespace QPFramework {

   /// <summary>
   ///
   /// 使用Command符合读写分离原则（Command Query Responsibility Segregation）CQRS，
   /// 即Command做到实现的逻辑进行调用和执行在时空上分离，而通过方法实现的逻辑进行的调用和执行不分离，调用方法立即执行方法
   /// 空间上，Command进行的执行和调用分在两个文件里
   /// 时间上，Command的方法被调用后过了一点时间才会被执行
   /// 在StrangeIOC、UFrame、PureMVC、Loxodon Framework、DDD（领域驱动设计）都会实现
   /// </summary>
   public interface ICommand: IFactoryGetable, IFactorySetable, ISystemGetable, IModelGetable, IUtilityGetable, IQueryUsable, ICommandUsable, IEventUseable {

      void Execute();
   }
}