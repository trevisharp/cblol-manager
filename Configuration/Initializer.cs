using System.Linq;
using System.Reflection;

namespace CBLoLManager.Configuration;

public abstract class Initializer
{
    public abstract void Initialize();

    public static void InitializeAll()
    {
        Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.BaseType == typeof(Initializer))
            .Select(t => t.GetConstructors().First().Invoke(null) as Initializer)
            .ToList()
            .ForEach(i => i.Initialize());
    }
}