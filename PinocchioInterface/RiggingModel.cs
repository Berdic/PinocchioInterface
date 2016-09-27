using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinocchioInterface
{
    public class RiggingModel : INotifyPropertyChanged
    {
        private int _xRot;
        private int _yRot;
        private int _zRot;

        private double _scaleFactor;

        public RiggingModel(string path)
        {
            XRot = 0;
            YRot = 0;
            ZRot = 0;
            ScaleFactor = 1;
            Path = path;
        }


        public int XRot
        {
            get { return _xRot; }
            set { _xRot = value; NotifyPropertyChanged("XRot"); }
        }


        public int YRot
        {
            get { return _yRot; }
            set { _yRot = value; NotifyPropertyChanged("YRot"); }
        }


        public int ZRot
        {
            get { return _zRot; }
            set { _zRot = value; NotifyPropertyChanged("ZRot"); }
        }


        public double ScaleFactor
        {
            get { return _scaleFactor; }
            set { _scaleFactor = Math.Round(value, 2); NotifyPropertyChanged("ScaleFactor"); }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Name = System.IO.Path.GetFileNameWithoutExtension(Path);
                NotifyPropertyChanged("Path");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        private string _name;


        private Motion _motion;

        public Motion Motion
        {
            get { return _motion; }
            set { _motion = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public enum Motion
    {
        None,
        Walk,
        Run,
        Jump
    }
}
