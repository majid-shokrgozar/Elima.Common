
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.WebApiSample.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PersonalWeb.Shared.PipelineBehavior;

namespace Elima.Common.WebApiSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterType<SampleDataContext>().As<IDatabaseContext>();
                builder.Register<IUnitOfWork>((ctx, sp) =>
                {
                    var allContext = new List<IDatabaseContext>
                    {
                        ctx.Resolve<SampleDataContext>()
                    };

                    var unitOfWorkEventPublisher = ctx.Resolve<IUnitOfWorkEventPublisher>();
                    var unitOfWorkDefaultOptions = ctx.Resolve<IOptions<AbpUnitOfWorkDefaultOptions>>();
                    return new UnitOfWork(unitOfWorkEventPublisher, unitOfWorkDefaultOptions, allContext);
                });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Validation 
            builder.Services.AddValidatorsFromAssembly(typeof(SampleDataContext).Assembly, includeInternalTypes: true);


            // MediatR

            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<WeatherForecast>();
                //cfg.AddBehavior<ValidationPipelineBehavior<,>>();
            });

            // Database
            builder.Services.AddDbContext<SampleDataContext>(options =>
                        options
                            .UseSqlServer(
                                builder
                                .Configuration
                                .GetConnectionString("Default"))

                        );

            // UnitOfWork

            builder.Services.AddScoped<IUnitOfWorkEventPublisher, UnitOfWorkEventPublisher>();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<SampleDataContext>();
                context.Database.EnsureCreated();
                // DbInitializer.Initialize(context);
            }


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
