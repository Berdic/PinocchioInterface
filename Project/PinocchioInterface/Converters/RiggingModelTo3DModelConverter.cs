using HelixToolkit.Wpf;
using PinocchioInterface.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace PinocchioInterface.Converters
{
    public class RiggingModelTo3DModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //Casting object to RiggingModel
                RiggingModel modelToConvert = (RiggingModel)value;

                //If null return null.
                if (modelToConvert == null)
                    return null;
                
                //Read the file.
                ObjReader CurrentHelixObjReader = new ObjReader();
                Model3DGroup visualModel = CurrentHelixObjReader.Read(modelToConvert.Path);

                Rect3D rectangle = visualModel.Children[visualModel.Children.Count - 1].Bounds;

                double y_transform_value = rectangle.Y + rectangle.SizeY / 2;
                double y_transform = rectangle.Y > 0 ? -y_transform_value : y_transform_value;

                double x_transform_value = rectangle.X + rectangle.SizeX / 2;
                double x_transform = rectangle.X > 0 ? -x_transform_value : x_transform_value;

                double z_transform_value = rectangle.Z + rectangle.SizeZ / 2;
                double z_transform = rectangle.Z > 0 ? -x_transform_value : x_transform_value;

                
                visualModel.Transform = new TranslateTransform3D(x_transform, y_transform, z_transform);

                //var axis = new Vector3D(0, 0, 1);
                //var angle = modelToConvert.ZRot;

                //var matrix = visualModel.Transform.Value;
                //matrix.Rotate(new Quaternion(axis, angle));

                //visualModel.Transform = new MatrixTransform3D(matrix);

                return visualModel;

            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
