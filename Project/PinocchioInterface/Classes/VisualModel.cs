using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PinocchioInterface.Classes
{
    public class VisualModel
    {
        /// <summary>
        /// To create an instane of VisualModel object you need to provide path to the desired obj file
        /// </summary>
        /// <param name="path"></param>
        public VisualModel(string path)
        {
            if (File.Exists(path))
            {
                Path = path;
                SetVisualModelGroup();
                ApplyTranslation();
            }
            else
                throw new FileNotFoundException("File not found.");
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }


        private Model3DGroup _model3DGroup;

        public Model3DGroup Model3DGroup
        {
            get { return _model3DGroup; }
            set { _model3DGroup = value; }
        }

        private void SetVisualModelGroup()
        {
            ObjReader CurrentHelixObjReader = new ObjReader();
            Model3DGroup = CurrentHelixObjReader.Read(Path);
        }

        private void ApplyTranslation()
        {
            Rect3D rectangle = Model3DGroup.Children[Model3DGroup.Children.Count - 1].Bounds;

            double y_transform_value = rectangle.Y + rectangle.SizeY / 2;
            double x_transform_value = rectangle.X + rectangle.SizeX / 2;
            double z_transform_value = rectangle.Z + rectangle.SizeZ / 2;

            Model3DGroup.Transform = new TranslateTransform3D(-x_transform_value, -y_transform_value, -z_transform_value);
        }
    }
}
