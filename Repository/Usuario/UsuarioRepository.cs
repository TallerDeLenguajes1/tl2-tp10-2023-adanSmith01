using System.Data.SQLite;
using tl2_tp10_2023_adanSmith01.Models;
namespace tl2_tp10_2023_adanSmith01.Repository;

public class UsuarioRepository: IUsuarioRepository
{
    private string connectionString = @"Data Source = DB/kanban.sqlite;Initial Catalog=Northwind;" + "Integrated Security=true";


    public void CrearUsuario(Usuario nuevoUsuario){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            
            string queryString = @"INSERT INTO Usuario (nombre_de_usuario) VALUES (@nombreUsuario);";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nuevoUsuario.NombreUsuario));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModificarUsuario(Usuario usuarioModificar){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string queryString = @"UPDATE Usuario SET nombre_de_usuario = @nombreNuevo WHERE id = @idUsuario;";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", usuarioModificar.Id));
            command.Parameters.Add(new SQLiteParameter("@nombreNuevo", usuarioModificar.NombreUsuario));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Usuario> GetAllUsuarios(){
        List<Usuario> usuarios = new List<Usuario>();
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Usuario;";
            var command = new SQLiteCommand(queryString, connection);
            using(var reader = command.ExecuteReader())
            {
                while(reader.Read()){
                    var usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreUsuario = reader["nombre_de_usuario"].ToString();
                    usuarios.Add(usuario);
                }
            }
            connection.Close();
        }

        return usuarios;
    }

    public Usuario GetUsuario(int idUsuario){
        Usuario usuarioEncontrado = new Usuario();
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Usuario WHERE id = @idUsuario";
            
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    usuarioEncontrado.Id = Convert.ToInt32(reader["id"]);
                    usuarioEncontrado.NombreUsuario = reader["nombre_de_usuario"].ToString();
                }
            }
            connection.Close();
        }

        return usuarioEncontrado;
    }

    public void EliminarUsuario(int idUsuario){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"DELETE FROM Usuario WHERE id = @idUsuario;";
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}