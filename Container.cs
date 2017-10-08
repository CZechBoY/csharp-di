using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CZechBoY.DI
{
    public class Container
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public Container()
        {
            // add classes with multiple constructors
            this.services.Add(typeof(Random), new Random());

            // add self as service :-)
            this.services.Add(this.GetType(), this);
        }

        private void CreateService(Type type)
        {
            object[] parameters = this.GetParametersFor(type);

            object service = Activator.CreateInstance(type, parameters);

            this.services.Add(type, service);
        }

        private object[] GetParametersFor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length > 1)
            {
                throw new NotSingleConstructorException(type);
            }

            ConstructorInfo constructor = constructors.First();
            ParameterInfo[] requiredParameters = constructor.GetParameters();

            List<object> parametersResolved = new List<object>(requiredParameters.Length);

            foreach (ParameterInfo requiredParameter in requiredParameters)
            {
                Type parameterType = requiredParameter.ParameterType;

                parametersResolved.Add(this.GetService(parameterType));
            }

            return parametersResolved.ToArray();
        }

        public T GetService<T>()
        {
            return (T) this.GetService(typeof(T));
        }

        private object GetService(Type type)
        {
            if (this.services.ContainsKey(type) == false)
            {
                this.CreateService(type);
            }

            return this.services[type];
        }

        public IEnumerable<T> GetByInterface<T>()
        {
            List<T> servicesImplementingInterface = new List<T>();

            Type[] classesImplementingInterface = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(classType => classType.GetInterfaces().Contains(typeof(T)))
                .ToArray();

            foreach (Type classType in classesImplementingInterface)
            {
                T service = (T) this.GetService(classType);
                servicesImplementingInterface.Add(service);
            }

            return servicesImplementingInterface;
        }
    }
}
