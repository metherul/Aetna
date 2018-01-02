using AutomatonDevkit.Model;
using System.ComponentModel;

namespace AutomatonDevkit.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ModPack ModPack { get; set; }

        public MainViewModel()
        {

        }
    }
}