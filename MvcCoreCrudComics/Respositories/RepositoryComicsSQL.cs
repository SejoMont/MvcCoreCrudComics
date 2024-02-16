using Microsoft.AspNetCore.Http.HttpResults;
using MvcCoreCrudComics.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Numerics;

#region PROCEDURES

//create procedure SP_INSERT_COMIC
//(
//@NOMBRE NVARCHAR(300),
//@IMAGEN NVARCHAR(300),
//@DESCRIPCION NVARCHAR(300)
//)
//AS
//DECLARE @IDCOMIC INT;

//SELECT @IDCOMIC = MAX(IDCOMIC) + 1 FROM COMICS;

//INSERT INTO COMICS
//     VALUES
//           (@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)
//GO

//create procedure SP_DELETE_COMIC
//(@idcomic int)
//as
//	delete from COMICS where IDCOMIC=@idcomic
//go
#endregion

namespace MvcCoreCrudComics.Respositories
{
    public class RepositoryComicsSQL : IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicsSQL()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NETCORE;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;

            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaComics);
        }

        public void DeleteComic(int idcomic)
        {
            this.com.Parameters.AddWithValue("@idcomic", idcomic);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_COMIC";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Comic GetComic(int idcomic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idcomic
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {

                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;

            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public List<Comic> BuscadorComic(int idcomic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idcomic
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Comic> comics = new List<Comic>();
                foreach (var row in consulta)
                {
                    Comic doc = new Comic
                    {
                        IdComic = row.Field<int>("IDCOMIC"),
                        Nombre = row.Field<string>("NOMBRE"),
                        Imagen = row.Field<string>("IMAGEN"),
                        Descripcion = row.Field<string>("DESCRIPCION")
                    };
                    comics.Add(doc);
                }
                return comics;
            }
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.com.Parameters.AddWithValue("@NOMBRE", nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", imagen);
            this.com.Parameters.AddWithValue("@DESCRIPCION", descripcion);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            int nextId = this.tablaComics.AsEnumerable().Max(x => x.Field<int>("IDCOMIC")) + 1;

            string sql = "INSERT INTO Comics (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION) VALUES (@idcomic, @nombre, @imagen, @descripcion)";

            this.com.Parameters.AddWithValue("@idcomic", nextId);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

    }
}
