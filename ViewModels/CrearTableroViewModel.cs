namespace tl2_tp10_2023_adanSmith01.ViewModels;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_adanSmith01.Models;

public class CrearTableroViewModel
{
    private int idUsuarioPropietario;
    private TableroViewModel tableroVM;

    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public TableroViewModel TableroVM { get => tableroVM; set => tableroVM = value; }

    public CrearTableroViewModel(int idUsuario){
        this.idUsuarioPropietario = idUsuario;
    }

    public CrearTableroViewModel(){
        
    }
    
}