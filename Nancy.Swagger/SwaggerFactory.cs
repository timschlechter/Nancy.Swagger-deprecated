using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy.Routing;
using Nancy.Swagger.Model;
using Newtonsoft.Json;

namespace Nancy.Swagger
{
	public class SwaggerFactory
	{
		#region Static Helpers

		private static Dictionary<Type, string> _swaggerTypeNames = new Dictionary<Type, string>
        {
            { typeof(System.String), "string"},
            { typeof(System.Int16), "short"},
            { typeof(System.Int32), "integer"},
            { typeof(System.Int64), "long"},
            { typeof(System.Boolean), "boolean"}
        };

		private static string GetSwaggerTypeName(Type type)
		{
			var isArray = type.IsArray;
			var isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(type);

			if (typeof(String).IsAssignableFrom(type))
			{
				return "string";
			}
			else if (isArray)
			{
                return "array of " + GetSwaggerTypeName(type.GetElementType());
			}
			else if (isEnumerable)
			{
				var elementType = type.GetGenericArguments().FirstOrDefault();
				if (elementType == null)
				{
					return "array";
				}
				else
				{
					return GetSwaggerTypeName(elementType) + "[]";
				}
			}

			if (_swaggerTypeNames.ContainsKey(type))
			{
				return _swaggerTypeNames[type];
			}

			return type.FullName;
		}

		private static bool IsReferenceToSwaggerModel(Type type)
		{
			return !type.IsArray
				&& !typeof(System.Collections.IEnumerable).IsAssignableFrom(type)
				&& !_swaggerTypeNames.ContainsKey(type);
		}

		#endregion Static Helpers

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
				SwaggerVersion = StaticConfiguration.SwaggerVersion,
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

		public IEnumerable<DocumentedMethod> CreateDocumentedMethods(NancyModule module)
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

		public Model.Model CreateModel(Type type)
		{
			return new Model.Model
			{
				Id = GetSwaggerTypeName(type),
				Properties = type.GetProperties()
								.Select(pi => CreatePropertyType(pi))
								.ToDictionary(
									pt => pt.Name,
									pt => pt
								)
			};
		}

		public IEnumerable<Model.Model> CreateModels(NancyModule module)
		{
			var documentedMethods = CreateDocumentedMethods(module);

			var distinctReturnTypes = documentedMethods.Select(d => d.OperationAttribute.Type).Distinct();

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
                operation.Type = GetSwaggerTypeName(attr.Type);
				operation.Parameters = CreateParameters(documentedMethod.Method);
			}

			return operation;
		}

		public PropertyType CreatePropertyType(PropertyInfo propertyInfo)
        {
            var jsonPropertyAttribute = propertyInfo.GetAttribute<JsonPropertyAttribute>();

            var propertyType = propertyInfo.PropertyType;
            var name = jsonPropertyAttribute != null ? jsonPropertyAttribute.PropertyName : propertyInfo.Name;
            var result = new PropertyType
            {
                Name = name
            };

			if (IsReferenceToSwaggerModel(propertyType))
            {
                result.Reference = GetSwaggerTypeName(propertyType);
            }
            else
            {
                result.Type = GetSwaggerTypeName(propertyType);
            }

            return result;
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
				SwaggerVersion = StaticConfiguration.SwaggerVersion,
				Apis = modules.Select(m => CreateResource(m)).OrderBy(a => a.Path)
			};
		}

		public string CreateRoutePath(NancyModule module)
		{
			var path = module.ModulePath.StartsWith("/") ? module.ModulePath : "/" + module.ModulePath;

			return path;
		}

		private Parameter CreateParameter(ParameterInfo pi)
		{
			var parameter = new Parameter
			{
				Name = pi.Name,
				ParamType = ParamType.query,
				Type = GetSwaggerTypeName(pi.ParameterType)
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

		#endregion Methods
	}
}