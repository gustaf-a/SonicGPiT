
using GptHandler.GptClient;
using Shared.Configurations;
using Shared.Prompts;
using SonicGPiT.GenerationStrategies;
using SonicGPiT.GenerationStrategies.ChangeOneThing;
using SonicGPiT.Services;

namespace SonicGPiT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IGptClient, GptClient>();
            builder.Services.AddSingleton<IGptResponseParser, GptResponseParser>();

            builder.Services.AddSingleton<IPromptBuilder, PromptBuilder>();

            builder.Services.AddSingleton<ICodeGeneratorService, CodeGeneratorService>();
            builder.Services.AddSingleton<ICodeGenerationStrategyFactory, CodeGenerationStrategyFactory>();
            
            builder.Services.AddSingleton<ICodeGenerationStrategy, ChangeOneThingStrategy>();
            builder.Services.AddSingleton<IChangeOneThingPromptDirector, ChangeOneThingPromptDirector>();

            builder.Services.AddControllers();

            // Add options
            builder.Services.Configure<OpenAiOptions>(builder.Configuration.GetSection(OpenAiOptions.OpenAi));
            builder.Services.Configure<FeatureFlagsOptions>(builder.Configuration.GetSection(FeatureFlagsOptions.FeatureFlags));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}