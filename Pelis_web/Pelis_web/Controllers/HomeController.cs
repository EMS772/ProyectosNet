using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pelis_web.Models;
using Pelis_web.Models.Data;
using Pelis_web.Models.Entidades;
using Pelis_web.Models.Services;
using System.Diagnostics;
using System.Linq;

namespace Pelis_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EncuestaContext _context;
        private const string PeliculasRecomendadasSessionKey = "PeliculasRecomendadas";

        public HomeController(EncuestaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var preguntas = _context.Preguntas.Include(p => p.Opciones).ToList();
            return View(preguntas);
        }

        public IActionResult Peliculas(string title, string description, string imageUrl, string genero, double? calificacionImdb)
        {
            var viewModel = new PeliculaViewModel
            {
                Titulo = title,
                Description = description,
                ImageUrl = imageUrl,
                Genero = genero,
                CalificacionImdb = calificacionImdb
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult GetRecommendation([FromBody] List<string> selections)
        {
            var pelicula = RecomendarPeliculaConPesos(selections);
            if (pelicula == null)
            {
                return Json(new { error = "No se encontró ninguna película que coincida con tus preferencias." });
            }
            return Json(new
            {
                title = pelicula.Titulo,
                description = pelicula.Descripcion,
                genero = pelicula.Genero,
                calificacionImdb = pelicula.CalificacionImdb,
                imageUrl = pelicula.ImagenUrl
            });
        }

        private Pelicula RecomendarPeliculaConPesos(List<string> selections)
        {
            // Obtener la lista de películas recomendadas desde la sesión
            var peliculasRecomendadas = HttpContext.Session.Get<List<int>>(PeliculasRecomendadasSessionKey) ?? new List<int>();

            // Filtrar primero las películas que coinciden con el género seleccionado
            var peliculas = _context.Peliculas
                .Where(p => !peliculasRecomendadas.Contains(p.Id) &&
                            p.Genero.ToLower() == selections[0].ToLower())
                .ToList();

            // Si no se encontraron películas que coincidan con el género
            if (!peliculas.Any())
            {
                // Seleccionar una película aleatoria de la base de datos que no esté en la lista de recomendadas
                peliculas = _context.Peliculas
                    .Where(p => !peliculasRecomendadas.Contains(p.Id))
                    .ToList();

                if (!peliculas.Any())
                {
                    // Si no hay películas disponibles, devolver null
                    return null;
                }

                // Seleccionar una película aleatoria
                var random = new Random();
                var peliculaAleatoria = peliculas[random.Next(peliculas.Count)];

                peliculasRecomendadas.Add(peliculaAleatoria.Id);
                HttpContext.Session.Set(PeliculasRecomendadasSessionKey, peliculasRecomendadas);

                return peliculaAleatoria;
            }

            // Calcular la puntuación para cada película restante
            var peliculasPuntuadas = peliculas.Select(p => new
            {
                Pelicula = p,
                Puntuacion = CalcularPuntuacion(p, selections)
            })
            .OrderByDescending(x => x.Puntuacion)
            .FirstOrDefault(); // Selecciona la película con mayor puntuación

            var peliculaRecomendada = peliculasPuntuadas?.Pelicula;

            if (peliculaRecomendada != null)
            {
                peliculasRecomendadas.Add(peliculaRecomendada.Id);
                HttpContext.Session.Set(PeliculasRecomendadasSessionKey, peliculasRecomendadas);
            }

            return peliculaRecomendada;
        }

        private int CalcularPuntuacion(Pelicula pelicula, List<string> selections)
        {
            int puntuacion = 0;

            // Asignar puntos adicionales al género para asegurarse de que sea prioritario
            if (pelicula.Genero.ToLower() == selections[0].ToLower()) puntuacion += 10;
            if (pelicula.TipoHistoria.ToLower() == selections[1].ToLower()) puntuacion += 2;
            if (pelicula.Epoca.ToLower() == selections[2].ToLower()) puntuacion += 2;
            if (pelicula.TipoFinal.ToLower() == selections[3].ToLower()) puntuacion += 1;
            if (pelicula.TipoAmbientacion.ToLower() == selections[4].ToLower()) puntuacion += 1;
            if (pelicula.TipoTrama.ToLower() == selections[5].ToLower()) puntuacion += 2;
            if (pelicula.EstructuraNarrativa.ToLower() == selections[6].ToLower()) puntuacion += 1;
            if (pelicula.TipoMusica.ToLower() == selections[7].ToLower()) puntuacion += 1;
            if (pelicula.TipoRitmo.ToLower() == selections[8].ToLower()) puntuacion += 1;

            return puntuacion;
        }

    }

}
