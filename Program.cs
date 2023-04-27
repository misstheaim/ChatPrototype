var builder = WebApplication.CreateBuilder(args);



builder.Services.AddMvc();
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();
var app = builder.Build();


app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "authentication",
    pattern: "{controller}/{action}");
app.MapControllerRoute(
    name: "authentication",
    pattern: "{controller}/{action}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}");
app.MapControllerRoute(
    name: "websocket",
    pattern: "ws",
    defaults: new { controller = "Websocket", action = "AddSocket" });


//WebSockets perfomance
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30),
};
app.UseWebSockets(webSocketOptions);

app.Run();
