namespace API.Jobs
{
    public class BackgroundWorker : IHostedService
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackgroundWorker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // var now = DateTime.Now;
            // var dueTime = now.Hour < 23 ? 23 - now.Hour : 24 - (now.Hour - 23);
            // dueTime = dueTime * 60 - now.Minute;

            // _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(dueTime), TimeSpan.FromHours(24));
            // return Task.CompletedTask;

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            // var masterBookService = scope.ServiceProvider.GetRequiredService<IMasterBookService>();
            // // var jsonData = masterBookService.GetRow("BK001");
            // var jsonData = masterBookService.GetRows();
            // var data = JsonConvert.SerializeObject(jsonData);
            // Console.WriteLine($"Job berjalan, dan ini job ke {a++}");
            // Console.WriteLine(data);

            // MasterBook book = new MasterBook
            // {
            //     Code = $"BK.000000{a}",
            //     BookName = $"Buku ke 1 auto",
            //     AuthorName = $"Author ke 1 autor",
            //     PublishDate = DateTime.Now,
            //     ContactPerson = $"Google@yahoo.com",
            //     Image = $"",
            //     Price = 150000,
            //     Discount = 0,
            //     Synopsis = $"asdsadasdasdasd",
            //     IsActive = "0"
            // };
            // masterBookService.Insert(book);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }


}
