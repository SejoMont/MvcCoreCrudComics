using Microsoft.AspNetCore.Http.HttpResults;
using MvcCoreCrudComics.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

#region PROCEDURES
//create or replace procedure sp_delete_comic
//(p_idcomic COMICS.IDCOMIC%TYPE)
//as
//begin
//  delete from COMICS where IDCOMIC=p_idcomic;
//commit;
//end;

//CREATE OR REPLACE PROCEDURE sp_insert_comic (
//    p_nombre IN COMICS.NOMBRE%TYPE,
//    p_imagen IN COMICS.IMAGEN%TYPE,
//    p_descripcion IN COMICS.DESCRIPCION%TYPE
//)
//AS
//    nextidcomic NUMBER;
//BEGIN
//    SELECT NVL(MAX(IDCOMIC), 0) + 1 INTO nextidcomic FROM COMICS;

//INSERT INTO COMICS VALUES (nextidcomic, p_nombre, p_imagen, p_descripcion);

//COMMIT;
//END;
#endregion

namespace MvcCoreCrudComics.Respositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {
        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand com;
        public RepositoryComicsOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from Comics";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
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

        public void DeleteComic(int idcomic)
        {
            OracleParameter pamIdComic = new OracleParameter(":p_idcomic", idcomic);
            this.com.Parameters.Add(pamIdComic);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_delete_comic";

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

        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            int nextId = this.tablaComics.AsEnumerable().Max(x => x.Field<int>("IDCOMIC")) + 1;

            string sql = "INSERT INTO Comics (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION) VALUES (:idcomic, :nombre, :imagen, :descripcion)";

            OracleParameter pamIdComic = new OracleParameter(":idcomic", nextId);
            this.com.Parameters.Add(pamIdComic);

            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);

            OracleParameter pamDescripcion = new OracleParameter(":descripcion", descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            OracleParameter pamNombre = new OracleParameter(":p_nombre", nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":p_imagen", imagen);
            this.com.Parameters.Add(pamImagen);

            OracleParameter pamDescripcion = new OracleParameter(":p_descripcion", descripcion);
            this.com.Parameters.Add(pamDescripcion);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_insert_comic";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
