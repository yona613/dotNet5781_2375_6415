﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
using System.Device.Location;

namespace BL
{
    public static class DeepCopyUtilities
    {
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string || value is Location)
                    propTo.SetValue(to, value);
            }
        }

        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
    }
}
