namespace Woz.Core
{
    public static class Singleton<T>
        where T : class
    {
        public static T Instance { get; set; }
    }
}