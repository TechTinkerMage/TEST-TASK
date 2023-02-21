namespace TEST_TASK
{
    public static class RouteRepository
    {
        private static readonly List<Route> _routes = new List<Route>();

        static RouteRepository()
        {
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var route = new Route
                {
                    Id = Guid.NewGuid(),
                    Origin = "Origin" + i,
                    Destination = "Destination" + i,
                    OriginDateTime = DateTime.Now.AddDays(random.Next(1, 10)),
                    DestinationDateTime = DateTime.Now.AddDays(random.Next(11, 20)),
                    Price = random.Next(100, 1000),
                    TimeLimit = DateTime.FromBinary(random.Next(1, 120))
                };
                _routes.Add(route);
            }
        }


        public static void Add(Route route)
        {
            _routes.Add(route);
        }

        public static IEnumerable<Route> GetAll()
        {
            return _routes;
        }
    }
}
