using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


/* <copyright file=BoolToStringConverter company="GromovProd">
   Copyright (c) 2014 All Rights Reserved
   </copyright>
   <author>Gromov Daniil</author>
   <date>11/20/2014 10:47:31 PM</date>
   <summary></summary>*/

namespace HammingCode.Model
{
    public class BoolToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool temp = (bool)value;
            bool param = (bool)parameter;

            if(param)
            {
                if (temp)
                {
                    return -90;
                }
                else
                {
                    return -45;
                }
            }
            else 
            {
                if (temp)
                {
                    return 0;
                }
                else
                {
                    return -45;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}