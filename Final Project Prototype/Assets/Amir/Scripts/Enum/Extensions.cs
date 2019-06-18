public static class Extensions
{
    #region Methods
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf(Arr, src) + 1;
        return (Arr.Length <= j) ? Arr[0] : Arr[j];
    }

    public static T Pre<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf(Arr, src) - 1;
        return (Arr.Length < 0) ? Arr[0] : Arr[j];
    }
    #endregion Methods
}