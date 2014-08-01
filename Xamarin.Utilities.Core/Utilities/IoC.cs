using System;
using System.Diagnostics;
using System.Linq;
using Autofac;

public class IoC
{
    private static readonly ContainerBuilder Builder = new ContainerBuilder();
    private static readonly Lazy<IContainer> Container = new Lazy<IContainer>(() => Builder.Build());

    public static void RegisterAssemblyServicesAsSingletons(System.Reflection.Assembly asm)
    {
        foreach (var type in asm.DefinedTypes.Where(x => x.Name.EndsWith("Service", StringComparison.Ordinal)))
            foreach (var iface in type.ImplementedInterfaces.Where(x => x.Name.EndsWith("Service", StringComparison.Ordinal)))
            {
#if DEBUG
                Debug.WriteLine("Registering {0} to {1}", iface.Name, type.Name);
#endif

                RegisterSingleton(iface, type.AsType());
            }
    }

    public static T Resolve<T>()
    {
        return Container.Value.Resolve<T>();
    }

    public static object Resolve(Type t)
    {
        return Container.Value.Resolve(t);
    }

    public static void Register<TInterface, TConcrete>()
    {
        Builder.RegisterType<TConcrete>().As<TInterface>();
    }

    public static void RegisterSingleton<TInterface, TConcrete>()
    {
        Builder.RegisterType<TConcrete>().As<TInterface>().SingleInstance();
    }

    public static void RegisterSingleton(Type @interface, Type implementation)
    {
        Builder.RegisterType(implementation).As(@interface).SingleInstance();
    }

    public static void RegisterAsInstance<TInterface, TConcrete>()
    {
        Builder.RegisterType<TConcrete>().As<TInterface>();
    }
}
