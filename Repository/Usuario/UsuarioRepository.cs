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
            
            string queryString = @"
            INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) 
            VALUES (@nombreUsuario, @contraseniaUsuario, @rolUsuario);
            ";
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nuevoUsuario.NombreUsuario));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", nuevoUsuario.Contrasenia));
            command.Parameters.Add(new SQLiteParameter("@rolUsuario", Convert.ToInt32(nuevoUsuario.RolUsuario)));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModificarUsuario(Usuario usuarioModificar){
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString;
            if(!String.IsNullOrEmpty(usuarioModificar.Contrasenia)){
                queryString = @"UPDATE Usuario SET nombre_de_usuario = @nombreNuevo, contrasenia = @contraseniaUsuario, rol = @rolUsuario  
            WHERE id = @idUsuario;";
            }else{
                queryString = @"UPDATE Usuario SET nombre_de_usuario = @nombreNuevo, rol = @rolUsuario  
            WHERE id = @idUsuario;";
            }
            var command = new SQLiteCommand(queryString, connection);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", usuarioModificar.Id));
            command.Parameters.Add(new SQLiteParameter("@nombreNuevo", usuarioModificar.NombreUsuario));
            if(!String.IsNullOrEmpty(usuarioModificar.Contrasenia))command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", usuarioModificar.Contrasenia));
            command.Parameters.Add(new SQLiteParameter("@rolUsuario", Convert.ToInt32(usuarioModificar.RolUsuario)));
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
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.RolUsuario = (Rol) Convert.ToInt32(reader["rol"]);
                    usuarios.Add(usuario);
                }
            }
            connection.Close();
        }

        return usuarios;
    }

    public Usuario? GetUsuario(int idUsuario){
        Usuario usuarioEncontrado = null;
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Usuario WHERE id = @idUsuario";
            
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    usuarioEncontrado = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        Contrasenia = reader["contrasenia"].ToString(),
                        RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                    };
                }
            }
            connection.Close();
        }

        return usuarioEncontrado;
    }

    public Usuario? GetUsuario(string nombre, string contrasenia){
        Usuario usuarioEncontrado = null;
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string queryString = @"SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contraseniaUsuario;";
            
            var command = new SQLiteCommand(queryString, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombre));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", contrasenia));
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()){
                    usuarioEncontrado = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        Contrasenia = reader["contrasenia"].ToString(),
                        RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                    };
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