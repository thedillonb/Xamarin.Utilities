using System;
using System.Linq;

public static class IoC
{
    public static void RegisterAssemblyServicesAsSingletons(System.Reflection.Assembly asm)
    {
        foreach (var type in asm.GetTypes().Where(x => x.Name.EndsWith("Service", StringComparison.Ordinal)))
            foreach (var iface in type.GetInterfaces().Where(x => x.Name.EndsWith("Service", StringComparison.Ordinal)))
            {
                #if DEBUG
                Console.WriteLine("Registering {0} to {1}", iface.Name, type.Name); 
                #endif

                RegisterAsSingleton(iface, type);
            }
    }

    public static void Register<T>() where T : class
    {
        TinyIoC.TinyIoCContainer.Current.Register<T>();
    }

    public static void RegisterAsSingleton<TInterface, TConcrete>()
        where TInterface : class
        where TConcrete : class, TInterface
    {
        TinyIoC.TinyIoCContainer.Current.Register<TInterface, TConcrete>().AsSingleton();
    }

    public static void RegisterAsSingleton(Type tInterface, Type tConcrete)
    {
        TinyIoC.TinyIoCContainer.Current.Register(tInterface, tConcrete).AsSingleton();
    }

    public static void Register<T>(T instance) where T : class
    {
        TinyIoC.TinyIoCContainer.Current.Register<T>(instance);
    }

    public static T Resolve<T>() where T : class
    {
        return TinyIoC.TinyIoCContainer.Current.Resolve<T>();
    }

    public static object Resolve(Type resolveType)
    {
        return TinyIoC.TinyIoCContainer.Current.Resolve(resolveType);
    }
}

