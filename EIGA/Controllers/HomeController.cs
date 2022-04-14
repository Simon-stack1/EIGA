using EIGA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EIGA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var names = GetAllMovies();

            return View(names);
        }

        [Route("film/{id}")]
        public IActionResult FilmDetail(int id)
        {
            var film = GetMovieDetails(id);
            return View(film);
        }

        public Film GetMovieDetails(int movieId)
        {
            // stel in waar de database gevonden kan worden
            //string connectionString = "Server=172.16.160.21;Port=3306;Database=110632;Uid=110632;Pwd=inf2122sql;";
            string connectionString = "Server=nd3;Port=18306;Database=abc;Uid=110632;Pwd=inf2122sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Film> films = new List<Film>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand($"select * from film where id = {movieId}", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string Name = reader["Naam"].ToString();
                        string Poster = reader["Poster"].ToString();

                        Film p = new Film();
                        // voeg de naam toe aan de lijst met namen
                        p.Id = Convert.ToInt32(reader["id"].ToString());
                        p.Name = Name;
                        p.Poster = Poster;

                        films.Add(p);

                    }
                }
            }

            // return de lijst met namen
            return films[0];
        }


        public List<Film> GetAllMovies()
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110632;Uid=110632;Pwd=inf2122sql;";
            string connectionString = "Server=nd3;Port=18306;Database=abc;Uid=110632;Pwd=inf2122sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Film> Film = new List<Film>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from film", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string Name = reader["Naam"].ToString();
                        string Poster = reader["Poster"].ToString();

                        Film p = new Film();
                        // voeg de naam toe aan de lijst met namen
                        p.Id = Convert.ToInt32(reader["id"].ToString());
                        p.Name = Name;
                        p.Poster = Poster;

                        Film.Add(p);

                    }
                }
            }

            // return de lijst met namen
            return Film;
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
