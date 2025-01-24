// ----------------------------------------------------------------------------
// Developer:      Ismail Hamzah
// Email:         go2ismail@gmail.com
// ----------------------------------------------------------------------------
 
using Microsoft.AspNetCore.Mvc;
 

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
builder.Services.AddRazorPages();
builder.Services.AddControllers()
;

builder.Services.AddEndpointsApiExplorer();



builder.Services.Configure<ApiBehaviorOptions>(x =>
{
    x.SuppressModelStateInvalidFilter = true;
});

//>>> Register Seeder
//builder.Services.RegisterSystemSeedManager(builder.Configuration);
//builder.Services.RegisterDemoSeedManager(builder.Configuration);

var app = builder.Build();



app.UseExceptionHandler(options => { });

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();


app.MapRazorPages();
app.MapControllers();

 

if (app.Environment.IsDevelopment())
{

    //no cache during development
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            ctx.Context.Response.Headers.Append("Pragma", "no-cache");
            ctx.Context.Response.Headers.Append("Expires", "0");
        }
    });

}


app.Run();
