using HelixToolkit.Wpf;
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

                var axis = new Vector3D(0, 0, 1);
                var angle = modelToConvert.ZRot;

                var matrix = visualModel.Transform.Value;
                matrix.Rotate(new Quaternion(axis, angle));

                visualModel.Transform = new MatrixTransform3D(matrix);

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
