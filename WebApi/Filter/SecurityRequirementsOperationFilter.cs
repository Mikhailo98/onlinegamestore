using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Filter
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var authorizeAttr = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Distinct();

            if (authorizeAttr.Any())
            {
                var requiredRoles = authorizeAttr.Select(attr => attr.Roles).Distinct();

                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    { "bearer", requiredRoles },
                    { "basic", requiredRoles }
                });
            }
        }
    }
}
