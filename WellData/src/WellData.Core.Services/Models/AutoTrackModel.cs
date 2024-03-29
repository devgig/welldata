﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WellData.Core.Extensions;

namespace WellData.Core.Services.Models
{
    public abstract class AutoTrackModel : Model
    {
        private readonly Type _type;
        private readonly object _instance;
        private PropInfo[] _propInfos;
        private readonly Dictionary<string, object> _trackedState = new Dictionary<string, object>();

        protected AutoTrackModel()
        {
            _instance = this;
            _type = this.GetType();
            EnsureInitialized();
        }

        
        /// <summary>
        /// This captures the properties on the model with type defaults
        /// </summary>
        private void EnsureInitialized()
        {
            _propInfos = GetPropInfos();

            foreach (var info in _propInfos)
            {
                _trackedState[info.PropertyInfo.Name] = info.PropertyInfo.PropertyType.GetDefaultValue();
            }
        }

        private PropInfo[] GetPropInfos()
        {
            var list = new List<PropInfo>();

            foreach (var propertyInfo in _type.GetProperties())
            {

                var propInfo = new PropInfo { PropertyInfo = propertyInfo };
                list.Add(propInfo);

                var propertyType = propertyInfo.PropertyType;
            }

            return list.ToArray();
        }

        /// <summary>
        /// Clean needs to be ran after the AutoTrack Model is populated with data to set the initial state.
        /// </summary>
        public void Clean()
        {
            foreach (var propInfo in _propInfos)
            {
                var propertyInfo = propInfo.PropertyInfo;

                var propValue = propertyInfo.GetValue(_instance, BindingFlags.Public | BindingFlags.Instance, null, null, null);
                _trackedState[propertyInfo.Name] = propValue;

                if (propValue == null)
                    continue;

            }
        }

        /// <summary>
        /// Checks to see if the data has changed from the original state
        /// </summary>
        /// <returns></returns>
        public bool IsDirty()
        {
            return _propInfos.Any(IsPropertyDirty);
        }

        private bool IsPropertyDirty(PropInfo propInfo)
        {
            var propertyInfo = propInfo.PropertyInfo;

            var propValue = propertyInfo.GetValue(_instance, BindingFlags.Public | BindingFlags.Instance, null, null, null);

            if (_trackedState[propertyInfo.Name] == null && propValue == null)
                return false;

            if (_trackedState[propertyInfo.Name] == null || propValue == null)
                return true;

            if (!_trackedState[propertyInfo.Name].Equals(propValue))
                return true;
                      
            return false;
        }

        class PropInfo
        {
            public PropertyInfo PropertyInfo { get; set; }
        }
    }

}
