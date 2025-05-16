using CommunityToolkit.Mvvm.ComponentModel;

namespace P_ReadME_GHE.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private string username = "Nom d'utilisateur";

    [ObservableProperty]
    private string role = "Lecteur";

    [ObservableProperty]
    private string email = "exemple@domaine.com";
}
