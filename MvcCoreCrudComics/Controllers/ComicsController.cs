using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudComics.Models;
using MvcCoreCrudComics.Respositories;
using System.Numerics;

namespace MvcCoreCrudComics.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;
        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult CreateLambda()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateLambda(Comic comic)
        {
            this.repo.InsertComicLambda(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult CreateProcedure()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateProcedure(Comic comic)
        {
            this.repo.InsertComicProcedure(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int idcomic)
        {
            Comic comic = repo.GetComic(idcomic);
            return View(comic);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int idcomic)
        {
            this.repo.DeleteComic(idcomic);
            return RedirectToAction("Index");
        }

        public IActionResult Buscador()
        {
            List<Comic> comics = this.repo.GetComics();
            ViewData["AllComics"] = comics; 
            return View();
        }

        [HttpPost]
        public IActionResult Buscador(int idcomic)
        {
            List<Comic> comics = this.repo.BuscadorComic(idcomic);
            if (comics == null)
            {
                ViewData["MENSAJE"] = "No existe " + idcomic;
                return View();
            }
            else
            {
                ViewData["AllComics"] = this.repo.GetComics(); 
                return View(comics);
            }
        }

    }
}
