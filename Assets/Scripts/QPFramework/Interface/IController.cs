namespace QPFramework {

   public interface IController: IFactoryGetable, IQueryUsable, ICommandUsable, IModelGetable, ISystemGetable, IEventUseable {
      //表现层的对象因为经常创建或摧毁，注册到ContainerSetter是没有意义的
      //此处接口的作用在于标记Controller层的对象属于Controller
      //并且可通过ContainerSetter访问Model和System（父级接口中的ContainerSetter对象
   }
}