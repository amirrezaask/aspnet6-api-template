using MinimalPlus;
using MinimalPlus.Configurations;
using MinimalPlus.Handlers;

WebApplication
    .CreateBuilder(args)
    .WantConfigurations()
    .WantSwagger()
    .WantAuthentication()
    .WantDatabase()
    .Build()
    .ConfigurePipeline()
    .MapAPIs()
    .Run();
