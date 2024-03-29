﻿using Nancy.Routing;
using Nancy.Swagger.Model;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nancy.Swagger.Providers.Attributes
{
    public class SwaggerProvider : ISwaggerProvider
    {
        #region Static Helpers

        private static Dictionary<Type, string> _swaggerTypeNames = new Dictionary<Type, string>
        {
            { typeof(System.String), "string"},
            { typeof(System.Int16), "short"},
            { typeof(System.Int32), "integer"},
            { typeof(System.Int64), "long"},
            { typeof(System.Boolean), "boolean"},
            { typeof(System.DateTime), "dateTime"}
        };

        private static string GetSwaggerTypeName(Type type)
        {
            if (_swaggerTypeNames.ContainsKey(type))
            {
                return _swaggerTypeNames[type];
            }
            else if (type.IsCollection())
            {
                return "array";
            }
            else
            {
                return type.FullName;
            }
        }

        private static bool IsPrimitive(Type type)
        {
            return _swaggerTypeNames.ContainsKey(type);
        }

        #endregion Static Helpers

        #region Constructors

        public SwaggerProvider(TinyIoCContainer container)
        {
            this.Container = container;
        }

        #endregion Constructors

        #region Properties

        public string SwaggerVersion { get { return "1.2"; } }

        protected TinyIoCContainer Container { get; private set; }

        #endregion Properties

        #region ISwaggerProvider

        public IEnumerable<ApiDeclaration> GetApiDeclarations()
        {
            var types = new NancyApiDiscoverer().GetModuleTypesToDocument();
            var modules = types.Select(t => Container.Resolve(t) as NancyModule);

            return modules.Select(module => CreateApiDeclaration(module)).OrderBy(a => a.ResourcePath);
        }

        public ResourceListing GetResourceListing(string basePath)
        {
            var types = new NancyApiDiscoverer().GetModuleTypesToDocument();
            var modules = types.Select(t => Container.Resolve(t) as NancyModule);

            return CreateResourceListing(modules, basePath);
        }

        #endregion ISwaggerProvider

        #region Methods

        public Api CreateApi(IEnumerable<Route> routes, NancyModule module)
        {
            var path = routes.First().Description.Path.Substring(module.ModulePath.Length);
            if (String.IsNullOrEmpty(path))
            {
                path = "/";
            }

            return new Api
            {
                Path = path,
                Operations = routes.Select(route => CreateOperation(route, module)).OrderBy(o => o.Nickname + " " + o.Method)
            };
        }

        public ApiDeclaration CreateApiDeclaration(NancyModule module)
        {
            return new ApiDeclaration
            {
                SwaggerVersion = SwaggerVersion,
                BasePath = CreateRoutePath(module),
                Models = CreateModels(module).ToDictionary(t => t.Id, t => t),
                Apis = CreateApis(module),
                ResourcePath = CreateRoutePath(module)
            };
        }

        public IEnumerable<Api> CreateApis(NancyModule module)
        {
            return module.Routes
                         .GroupBy(r => r.Description.Path)
                         .Select(group => CreateApi(group, module))
                         .OrderBy(api => api.Path);
        }

        public Items CreateItems(Type type)
        {
            if (!type.IsCollection())
            {
                return null;
            }

            var elementType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();
            if (IsPrimitive(type))
            {
                return new Items { Type = GetSwaggerTypeName(elementType) };
            }
            else
            {
                return new Items { Ref = GetSwaggerTypeName(elementType) };
            }
        }

        public Model.Model CreateModel(Type type)
        {
            return new Model.Model
            {
                Id = GetSwaggerTypeName(type),
                Properties = type.GetProperties()
                                .ToDictionary(
                                    pi => CreatePropertyName(pi),
                                    pi => CreatePropertyType(pi)
                                )
            };
        }

        public IEnumerable<Model.Model> CreateModels(NancyModule module)
        {
            var documentedMethods = CreateDocumentedMethods(module);

            var distinctReturnTypes = documentedMethods
                                                .Select(d => d.OperationAttribute.Type)
                                                .Distinct()
                                                .Where(t => !t.IsCollection());

            return distinctReturnTypes.Select(t => CreateModel(t));
        }

        public Operation CreateOperation(Route route, NancyModule module)
        {
            var documentedMethods = CreateDocumentedMethods(module);

            var operation = new Operation
            {
                Method = route.Description.Method,
                Nickname = route.Description.Path
            };

            var documentedMethod = documentedMethods.Find(module, route);
            if (documentedMethod == null)
            {
                operation.Summary = "Warning: No method found with matchin OperationAttribute";
            }
            else
            {
                var attr = documentedMethod.OperationAttribute;

                operation.Summary = attr.Summary;
                operation.Notes = attr.Notes;
                operation.Parameters = CreateParameters(documentedMethod.Method);
                operation.Type = GetSwaggerTypeName(attr.Type);
                operation.Items = CreateItems(attr.Type);
            }

            return operation;
        }

        public string CreatePropertyName(PropertyInfo propertyInfo)
        {
            var jsonPropertyAttribute = propertyInfo.GetAttribute<JsonPropertyAttribute>();
            return jsonPropertyAttribute != null ? jsonPropertyAttribute.PropertyName : propertyInfo.Name;
        }

        public PropertyType CreatePropertyType(PropertyInfo propertyInfo)
        {
            var jsonPropertyAttribute = propertyInfo.GetAttribute<JsonPropertyAttribute>();
            var name = jsonPropertyAttribute != null ? jsonPropertyAttribute.PropertyName : propertyInfo.Name;

            var propertyType = propertyInfo.PropertyType;
            var result = new PropertyType
            {
            };

            return SetDataTypeProperties(result, propertyInfo.PropertyType);
        }

        public Resource CreateResource(NancyModule module)
        {
            return new Resource
            {
                Path = "/swagger" + CreateRoutePath(module)
            };
        }

        public ResourceListing CreateResourceListing(IEnumerable<NancyModule> modules, string basePath)
        {
            return new ResourceListing
            {
                SwaggerVersion = SwaggerVersion,
                Apis = modules.Select(m => CreateResource(m)).OrderBy(a => a.Path)
            };
        }

        public string CreateRoutePath(NancyModule module)
        {
            var path = module.ModulePath.StartsWith("/") ? module.ModulePath : "/" + module.ModulePath;

            return path;
        }

        private IEnumerable<DocumentedMethod> CreateDocumentedMethods(NancyModule module)
        {
            foreach (var method in module.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var swagger = method.GetAttribute<OperationAttribute>();

                if (swagger != null)
                {
                    yield return new DocumentedMethod
                    {
                        OperationAttribute = swagger,
                        Method = method
                    };
                }
            }
        }

        private Parameter CreateParameter(ParameterInfo pi)
        {
            var parameter = new Parameter
            {
                Name = pi.Name,
                ParamType = ParamType.query,
                Type = GetSwaggerTypeName(pi.ParameterType),
                Items = CreateItems(pi.ParameterType)
            };

            var attr = pi.GetCustomAttributes(typeof(ParameterAttribute), true).FirstOrDefault() as ParameterAttribute;

            if (attr == null)
            {
                parameter.Description = "Warning: no ParameterAttribute found for this parameter";
            }
            else
            {
                parameter.Name = attr.Name;
                parameter.ParamType = attr.ParamType;
                parameter.Required = attr.Required;
            }

            return parameter;
        }

        private IEnumerable<Parameter> CreateParameters(MethodBase methodBase)
        {
            return methodBase.GetParameters().Select(pi => CreateParameter(pi)).OrderBy(p => p.Name);
        }

        private T SetDataTypeProperties<T>(T item, Type type)
            where T : DataType
        {
            if (type.IsCollection())
            {
                item.Type = "array";
                item.Items = CreateItems(type);
            }
            else if (IsPrimitive(type))
            {
                item.Type = GetSwaggerTypeName(type);
            }
            else
            {
                item.Ref = GetSwaggerTypeName(type);
            }

            return item;
        }

        #endregion Methods
    }
}