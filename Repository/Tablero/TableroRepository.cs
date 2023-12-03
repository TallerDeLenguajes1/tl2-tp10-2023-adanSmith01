using System.Data.SQLite;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public class TableroRepository: ITableroRepository
{
    private string connectionString = @"Data Source = DB/kanban.sqlite;Initial Catalog=Northwind;" + "Integrated Security=true";

    public void CrearTablero(Tablero nuevoTablero){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            
            string queryString = @"
            INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
            VALUES (@idPropietario, @nombreTablero, @descripcionTablero);
            ";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idPropietario", nuevoTablero.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombreTablero", nuevoTablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcionTablero", nuevoTablero.Descripcion));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModificarTablero(Tablero tableroModificar){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"
            UPDATE Tablero 
            SET id_usuario_propietario = @idPropietario, nombre = @nombreTablero, descripcion = @descripcionTablero 
            WHERE id = @idTablero;
            ";

            var command = new SQLiteCommand(queryString, connection);
            
            command.Parameters.Add(new SQLiteParameter("@idPropietario", tableroModificar.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombreTablero", tableroModificar.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcionTablero", tableroModificar.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@idTablero", tableroModificar.Id));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Tablero> GetAllTableros(){
        List<Tablero> tableros = new List<Tablero>();

        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tablero;";
            var command = new SQLiteCommand(queryString, connection);

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tablero tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero); 
                }
            }

            connection.Close();
        }

        return tableros;
    }

    public Tablero? GetTablero(int idTablero){
        Tablero tableroEncontrado = null;
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tablero WHERE id = @idTablero";
            
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    tableroEncontrado = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                }
            }
            connection.Close();
        }

        return tableroEncontrado;
    }

    public List<Tablero> GetTablerosDeUsuario(int idUsuario){
        List<Tablero> tablerosUsuario = new List<Tablero>();

        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idPropietario;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idPropietario", idUsuario));

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    Tablero tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tablerosUsuario.Add(tablero); 
                }
            }

            connection.Close();
        }

        return tablerosUsuario;
    }

    public void EliminarTablero(int idTablero){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"DELETE FROM Tablero WHERE id = @idTablero;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}