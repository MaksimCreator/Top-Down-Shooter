namespace Model
{
    public class ServiceLocator
    {
        public void RegisterService<TService>(TService service) where TService : IService
        => Implementation<TService>.Instance = service;

        public TService GetSevice<TService>() where TService : IService
        => Implementation<TService>.Instance;

        private static class Implementation<TService> where TService : IService
        {
            public static TService Instance;
        }
    }
}