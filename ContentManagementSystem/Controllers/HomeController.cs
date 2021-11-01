using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContentManagementSystem.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using Amazon;


namespace ContentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MySqlConnection _sqlConnection;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            try
            {
                //var server = "http://bgp-cms-alb-180197331.us-east-1.elb.amazonaws.com";
                var defaultHostName = "bgp-rds.cdlugrfvcunk.us-east-2.rds.amazonaws.com";
                var port = 3306;
                var defaultUsername = "root";
                var defaultPassword = "Pokemon1";
                var defaultDatabaseName = "DB_BGP";

                // ENDPOINT, USER, PASS

                var hostName = Environment.GetEnvironmentVariable("ENDPOINT") ?? defaultHostName;
                var username = Environment.GetEnvironmentVariable("USER") ?? defaultUsername;
                var password = Environment.GetEnvironmentVariable("Pass") ?? defaultPassword;
                var databaseName = Environment.GetEnvironmentVariable("DBNAME") ?? defaultDatabaseName;

                //var databaseName = "BGP_DB";
                //var authToken = Amazon.RDS.Util.RDSAuthTokenGenerator.GenerateAuthToken(RegionEndpoint.USEast1, hostName, port, username);

                var connectionString = $"server={hostName};port={port};user={username};password={password}";
                _sqlConnection = new MySqlConnection(connectionString);

                Debug.WriteLine("Connected to database");

                _sqlConnection.Open();

                Debug.WriteLine("Opened database");

                var sqlCommand = new MySqlCommand("SHOW DATABASES", _sqlConnection);
                var dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    Debug.WriteLine($"Reader Information: {dataReader[0]}");
                }

                dataReader.Close();

                _sqlConnection.Close();
            }
            catch (MySqlException exception)
            {
                Debug.WriteLine($"MySQLException:::::::::::::::::::::::::\n{exception}");
            }

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
