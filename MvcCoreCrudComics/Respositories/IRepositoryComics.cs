using MvcCoreCrudComics.Models;
using System.Numerics;

namespace MvcCoreCrudComics.Respositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        List<Comic> BuscadorComic(int idcomic);
        void InsertComicProcedure(string nombre, string imagen, string descripcion);
        void InsertComicLambda(string nombre, string imagen, string descripcion);
        Comic GetComic(int idcomic);
        void DeleteComic(int idcomic);
    }
}
