using System;
using Autofac;

public class IoC
{
    private static readonly ContainerBuilder Builder = new ContainerBuilder();
    private static readonly Lazy<IContainer> Container = new Lazy<IContainer>(() => Builder.Build());

    public static T Resolve<T>()
    {
        return Container.Value.Resolve<T>();
    }

    public static object Resolve(Type t)
    {
        return Container.Value.Resolve(t);
    }

    public void Register<TInterface, TConcrete>()
    {
        Builder.RegisterType<TConcrete>().As<TInterface>();
    }

    public void RegisterSingleton<TInterface, TConcrete>()
    {
        Builder.RegisterType<TConcrete>().As<TInterface>().SingleInstance();
    }
}
