using PinocchioInterface.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PinocchioInterface
{
    public class RiggingModel : INotifyPropertyChanged
    {
        private int _xRot;
        private int _yRot;
        private int _zRot;
        private double _scaleFactor;

        private string _path;
        private string _name;

        private Motion _motion;
        private Skeleton _skeleton;
        
        private VisualModel _visualModel;

        private List<Joint> _joints;

        public RiggingModel(string path)
        {
           
            Path = path;
            XRot = 0;
            YRot = 0;
            ZRot = 0;
            ScaleFactor = 1;
            Motion = Motion.None;
            Skeleton = Skeleton.Human;

            Joints = new List<Joint>();
            

            ModelsOnScreen = new VisualModel(Path);
        }


        public List<Joint> Joints
        {
            get { return _joints; }
            set
            {
                _joints = value;
                NotifyPropertyChanged("Joints");
            }
        }

        /// <summary>
        /// Degrees value for rotation aroun X axis.
        /// </summary>
        public int XRot
        {
            get { return _xRot; }
            set
            {
                _xRot = value;
                NotifyPropertyChanged("XRot");
            }
        }

        /// <summary>
        /// Degrees value for rotation aroun Y axis.
        /// </summary>
        public int YRot
        {
            get { return _yRot; }
            set
            {
                _yRot = value;
                NotifyPropertyChanged("YRot");
            }
        }

        /// <summary>
        /// Degrees value for rotation aroun Z axis.
        /// </summary>
        public int ZRot
        {
            get { return _zRot; }
            set
            {
                _zRot = value;
                NotifyPropertyChanged("ZRot");
            }
        }

        /// <summary>
        /// Skeleton scaling factor
        /// </summary>
        public double ScaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = Math.Round(value, 2);
                NotifyPropertyChanged("ScaleFactor");
            }
        }

        /// <summary>
        /// Path to the model file
        /// </summary>
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Name = System.IO.Path.GetFileName(Path);
            }
        }

        /// <summary>
        /// Name of the model file, extracted from Path property
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }


        
        /// <summary>
        /// Motion
        /// </summary>
        public Motion Motion
        {
            get
            {
                return _motion;
            }
            set
            {
                _motion = value;
                if (_motion != Motion.None)
                    Skeleton = Skeleton.Human;
            }
        }

        /// <summary>
        /// Skeleton
        /// </summary>
        public Skeleton Skeleton
        {
            get { return _skeleton; }
            set { _skeleton = value; NotifyPropertyChanged("Skeleton"); }
        }


        /// <summary>
        /// Model shown in interface
        /// </summary>
        public VisualModel ModelsOnScreen
        {
            get { return _visualModel; }
            set { _visualModel = value; NotifyPropertyChanged("ModelsOnScreen"); }
        }

        //TODO: If not using anymore, delet this.
        ////public string GetCommandLineArguments(string motionFolder)
        ////{
        ////    string[] parameters = new string[7];
        ////    parameters[0] = GetPathForCmd();
        ////    parameters[1] = GetXRotForCmd();
        ////    parameters[2] = GetYRotForCmd();
        ////    parameters[3] = GetZRotForCmd();
        ////    parameters[4] = GetScaleFactorCmd();

        ////    parameters[5] = GetMotionForCmd(motionFolder);
        ////    parameters[6] = GetSkeletonCmd();
        ////    return String.Join(" ", parameters);
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetMotionForCmd(string motionFolder)
        ////{
        ////    switch (Motion)
        ////    {
        ////        case Motion.Jump:
        ////            return "-motion " + System.IO.Path.Combine(motionFolder, "jumpAround.txt");

        ////        case Motion.Walk:
        ////            return "-motion " + System.IO.Path.Combine(motionFolder, "walk.txt");

        ////        case Motion.Run:
        ////            return "-motion " + System.IO.Path.Combine(motionFolder, "runAround.txt");
        ////    }

        ////    return "";
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetSkeletonCmd()
        ////{
        ////    return "-skel " + Skeleton.ToString().ToLower();
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetScaleFactorCmd()
        ////{
        ////    return "-scale " + ScaleFactor;
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetZRotForCmd()
        ////{
        ////    return "-rot 0 0 1 " + ZRot;
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetYRotForCmd()
        ////{
        ////    return "-rot 0 1 0 " + YRot;
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetXRotForCmd()
        ////{
        ////    return "-rot 1 0 0 " + XRot;
        ////}

        //TODO: If not using anymore, delet this.
        ////private string GetPathForCmd()
        ////{
        ////    return "\"" + Path + "\"";
        ////}


        #region INotifyPropertyChanged implentation

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #endregion
    }




    public enum Motion
    {
        None,
        Walk,
        Run,
        Jump
    }

    public enum Skeleton
    {
        Human,
        Quad,
        Horse,
        Centaur
    }

}
