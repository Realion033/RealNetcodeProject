using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoNameGun.Entities
{
    public class Entity : MonoBehaviour
    {
        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            AddComponentToDictionary();
            ComponentInit();
            AfterInitalize();
        }

        private void AddComponentToDictionary()
        {
            GetComponentsInChildren<IEntityComponent>(true).ToList().
            ForEach(component => _components.Add(component.GetType(), component));
        }

        private void ComponentInit()
        {
            _components.Values.ToList().ForEach(component => component.Init(this));
        }

        protected virtual void AfterInitalize()
        {
            _components.Values.OfType<IAfterInitable>().ToList().ForEach(afterInitCompo => afterInitCompo.AfterInIt());
        }

        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component;
            }

            if (isDerived == false) return default;

            Type findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != null)
            {
                return (T)_components[findType];
            }

            return default;
        }
    }
}
