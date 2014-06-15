using Nancy.Routing;
using Nancy.Swagger.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nancy.Swagger
{
    public static class Extensions
    {
        #region Nancy.Swagger.DocumententMethod

        internal static DocumentedMethod Find(this IEnumerable<DocumentedMethod> documentedMethods, NancyModule module, Route route)
        {
            var relativePath = route.Description.Path.Remove(0, module.ModulePath.Length);
                        
            return documentedMethods
                        .Where(d => d.OperationAttribute.Path == relativePath)
                        .Where(d => d.OperationAttribute.Method == route.Description.Method)                        
                        .FirstOrDefault();
        }

        #endregion

        #region System.Reflection.MemberInfo

        internal static T GetAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            return Attribute.GetCustomAttribute(member, typeof(T)) as T;
        }
        
        #endregion

        #region System.Reflection.MethodBase

        internal static T GetAttribute<T>(this MethodBase method)
            where T : Attribute
        {
            return Attribute.GetCustomAttribute(method, typeof(T)) as T;
        }

        #endregion
        
        
    }
}