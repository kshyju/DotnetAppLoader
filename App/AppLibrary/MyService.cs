using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLibrary
{
    public interface IMyService
    {
        string GetMessage();
    }

    public class MyService : IMyService
    {
        private IConfiguration? _configuration;
        private ILogger<MyService> _logger;

        public MyService(ILogger<MyService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public MyService(IConfiguration configuration, ILogger<MyService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetMessage()
        {
            if (_configuration is not null)
            {
                _logger.LogInformation($"Name: {_configuration["Name"]}");
            }
            _logger.LogInformation("Inside GetMessage");
            return $"Hello from MyService at {DateTime.Now}";
        }
    }
}
