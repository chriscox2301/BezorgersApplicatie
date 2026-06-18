using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{

    public class Package : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public double Weight { get; set; }
        public string Barcode { get; set; }

        private string? _status;
        public string? Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
