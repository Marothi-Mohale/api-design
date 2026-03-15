namespace TutoringPlatform.Api.OpenApi;

public static class OpenApiUiEndpointExtensions
{
    public static IEndpointRouteBuilder MapOpenApiUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/docs", async context =>
        {
            const string html = """
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>Tutoring Platform API Docs</title>
  <link rel="stylesheet" href="https://unpkg.com/swagger-ui-dist@5/swagger-ui.css" />
  <style>
    body { margin: 0; background: #f5f3ef; color: #1f2937; font-family: Georgia, "Times New Roman", serif; }
    .topbar { padding: 20px 28px; background: linear-gradient(135deg, #17324d, #35607a); color: #fff; }
    .topbar h1 { margin: 0 0 6px; font-size: 28px; }
    .topbar p { margin: 0; max-width: 760px; opacity: 0.92; }
    #swagger-ui { max-width: 1200px; margin: 0 auto; }
  </style>
</head>
<body>
  <section class="topbar">
    <h1>Tutoring Platform API</h1>
    <p>Development OpenAPI explorer for auth, tutors, subjects, sessions, and health checks.</p>
  </section>
  <div id="swagger-ui"></div>
  <script src="https://unpkg.com/swagger-ui-dist@5/swagger-ui-bundle.js"></script>
  <script>
    window.ui = SwaggerUIBundle({
      url: "/openapi/v1.json",
      dom_id: "#swagger-ui",
      deepLinking: true,
      displayRequestDuration: true,
      defaultModelsExpandDepth: 1,
      persistAuthorization: true
    });
  </script>
</body>
</html>
""";

            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(html);
        })
        .AllowAnonymous();

        return endpoints;
    }
}
