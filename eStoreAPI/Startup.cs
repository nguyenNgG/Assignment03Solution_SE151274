using BusinessObject;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

namespace eStoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products").EntityType.HasKey(x => x.ProductId);
            builder.EntitySet<Member>("Members").EntityType.HasKey(x => x.Id);
            builder.EntitySet<Order>("Orders").EntityType.HasKey(x => x.OrderId);
            builder.EntitySet<OrderDetail>("OrderDetails").EntityType.HasKey(x => new { x.OrderId, x.ProductId });
            builder.EntitySet<Category>("Categories").EntityType.HasKey(x => x.CategoryId);
            return builder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddDbContext<eStoreDbContext>();
            services.AddIdentity<Member, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<eStoreDbContext>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();

            services.AddCors();

            services.AddControllers();
            services.AddControllers().AddOData(option => option
            .Select().Filter().Count().OrderBy().Expand().SetMaxTop(100).AddRouteComponents("odata", GetEdmModel()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eStoreAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eStoreAPI v1"));
            }

            app.UseODataBatching();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
