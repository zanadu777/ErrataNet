namespace Errata.Functional;
public static class FuncExtensions
  {
    public static Func<T, T> Pipeline<T>(this IEnumerable<Func<T, T>> functions)
    {
      T Func(T x)
      {
        var current = x;
        foreach (var f in functions) 
          current = f(current);

        return current;
      }

      return Func;
    }

}