using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAppDocente.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private int count;

        [ObservableProperty]
        private int rest = 10;

        [RelayCommand]
        public void Sumar()
        {
            Count++;
        }
        [RelayCommand]
        public void Restar()
        {
            Rest--;
        }
    }
}
