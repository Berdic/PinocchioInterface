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
    /// <summary>
    /// Class represents visual representation of a model. Its all visual elements.
    /// </summary>
    public class VisualModel
    {
        /// <summary>
        /// To create an instane of VisualModel object you need to provide path to the desired obj file
        /// </summary>
        /// <param name="path"></param>
        public VisualModel(string path)
        {
            Path = path;
            SetVisualModelGroup();
            
            Rect3D rectangle = Model3DGroup.Children[Model3DGroup.Children.Count - 1].Bounds;
            ApplyTranslation(rectangle);
            
            ScaleFactorGrid = 1 / rectangle.SizeY;

            Joints = new Point3DCollection();

        }
        
        private double _scaleFactorGrid;

        public double ScaleFactorGrid
        {
            get { return _scaleFactorGrid; }
            set { _scaleFactorGrid = value; }
        }


        public string Path { get; set; }

        private Model3DGroup _model3DGroup;

        public Model3DGroup Model3DGroup
        {
            get { return _model3DGroup; }
            set { _model3DGroup = value; }
        }

        public Point3DCollection Joints { get; set; }

        private void SetVisualModelGroup()
        {
            ObjReader CurrentHelixObjReader = new ObjReader();
            Model3DGroup = CurrentHelixObjReader.Read(Path);
        }

        private void ApplyTranslation(Rect3D rectangle)
        {

            double y_transform_value = rectangle.Y + rectangle.SizeY / 2;
            double x_transform_value = rectangle.X + rectangle.SizeX / 2;
            double z_transform_value = rectangle.Z + rectangle.SizeZ / 2;

            Model3DGroup.Transform = new TranslateTransform3D(-x_transform_value, -y_transform_value, -z_transform_value);
        }
        
        public bool SetJoints(List<Joint> joints)
        {
            foreach (var item in joints)
            {
                Joints.Add(new Point3D(item.X, item.Y, item.Z));
            }

            return true;
        }
    }
}
