using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBase = new Uri("http://localhost:5107"); // your WebAPI URL

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = apiBase
});

builder.Services.AddHttpClient<IUserService, HttpUserService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<IPostService, HttpPostService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<ICommentService, HttpCommentService>(c => c.BaseAddress = apiBase);
builder.Services.AddScoped<SimpleAuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SimpleAuthProvider>());



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();



app.Run();