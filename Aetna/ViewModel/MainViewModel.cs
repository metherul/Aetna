using Aetna.Model;
using System.ComponentModel;

namespace Aetna.ViewModel
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