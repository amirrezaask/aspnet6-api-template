using MinimalPlus;
using MinimalPlus.Configurations;
using MinimalPlus.Handlers;
using System.Reflection;

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