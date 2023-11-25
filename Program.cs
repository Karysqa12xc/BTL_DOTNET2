using BTL_DOTNET2.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestForumContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Server=ADMIN-PC\\SQLEXPRESS; Database=TestForum; User Id = nam; Password=1234; Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True; Encrypt= True;")));
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// app.Use(async (context, next) => {
//     await next();
//     if(context.Response.StatusCode == 404){
//         context.Request.Path = "/Home/Privacy";
//         await next();
//     }
// });
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();


app.MapControllers();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
