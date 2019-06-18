using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Animals.DTM;

namespace Animals.Service
{
    public class Startup
    {
        /// <summary>
        /// Стартовый путь приложения
        /// </summary>
        private string _rootPath = "";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _rootPath = Directory.GetParent(env.ContentRootPath).FullName;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Добавление репозитория в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            // Вставка стартового пути приложения в строку подключения к локальной БД
            string connectionString = Configuration.GetConnectionString("AnimalsContext");
            if (connectionString.Contains("%ROOTPATH%"))
            {
                connectionString = connectionString.Replace("%ROOTPATH%", _rootPath);
            }

            services.AddMvc();

            // IoC
            services.AddEntityFrameworkSqlServer().AddDbContext<AnimalsRepozitory>(options => options.UseSqlServer(connectionString));
            services.AddScoped(repoFactory);
        }

        private Func<IServiceProvider, IAnimalsRepozitory> repoFactory = x =>
        {
            return x.GetService<AnimalsRepozitory>();
        };

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
