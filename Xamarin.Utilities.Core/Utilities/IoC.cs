using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public class IoC
{
    public static void RegisterAssemblyServicesAsSingletons(Assembly asm)
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

    public static T Resolve<T>() where T : class
    {
        return TinyIoC.TinyIoCContainer.Current.Resolve<T>();
    }

    public static object Resolve(Type t)
    {
        return TinyIoC.TinyIoCContainer.Current.Resolve(t);
    }

    public static void Register<TInterface, TConcrete>() where TConcrete : class, TInterface where TInterface : class
    {
        TinyIoC.TinyIoCContainer.Current.Register<TInterface, TConcrete>();
    }

    public static void Register<TInterface>(TInterface o) where TInterface : class
    {
        TinyIoC.TinyIoCContainer.Current.Register(o);
    }

    public static void RegisterSingleton<TInterface, TConcrete>() where TConcrete : class, TInterface where TInterface : class
    {
        TinyIoC.TinyIoCContainer.Current.Register<TInterface, TConcrete>().AsSingleton();
    }

    public static void RegisterSingleton(Type @interface, Type implementation)
    {
        TinyIoC.TinyIoCContainer.Current.Register(@interface, implementation);
    }

    public static void RegisterAsInstance<TInterface, TConcrete>() where TConcrete : class, TInterface where TInterface : class
    {
        TinyIoC.TinyIoCContainer.Current.Register<TInterface, TConcrete>().AsMultiInstance();
    }
}
