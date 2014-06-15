﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Swagger
{
    public class NancyApiDiscoverer
    {
        private IEnumerable<Type> AllTypes
        {
            get
            {
                return AppDomain.CurrentDomain
                                .GetAssemblies()
                                .SelectMany(asm => asm.GetTypes());
            }
        }

        /// <summary>
        /// Returns all modules for which documentation should be generated
        /// </summary>
        /// <returns>All modules for which documentation should be generated</returns>
        public IEnumerable<NancyModule> GetModulesToDocument()
        {
            return AllTypes
                    .Where(t => ShouldDocument(t))
                    .Select(type => Activator.CreateInstance(type) as NancyModule);
        }

        /// <summary>
        /// Returns wether the given type is can be instantiated or not.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test</param>
        /// <returns>Wether the given type is can be instantiated or not.</returns>
        private bool CanBeInstantiated(Type type)
        {
            return !type.IsAbstract;
        }

        private bool IsExcluded(Type type)
        {
            var ns = type.Namespace != null ? type.Namespace : "";
            return type.Equals(typeof(SwaggerModule))
                || ns.StartsWith("Nancy.Diagnostics");
        }

        /// <summary>
        /// Returns wether the given type is a Nancy module or not.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test</param>
        /// <returns>Wether the given type is a Nancy module or not.</returns>
        private bool IsNancyModule(Type type)
        {
            return typeof(NancyModule).IsAssignableFrom(type);
        }

        /// <summary>
        /// Returns wether the given type should be included in the API documentation or not.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test</param>
        /// <returns>Wether the given type should be included in the API documentation or not.</returns>
        private bool ShouldDocument(Type type)
        {
            return IsNancyModule(type)
                && CanBeInstantiated(type)
                && !IsExcluded(type);
        }
    }
}