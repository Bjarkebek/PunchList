using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PunchList.Components;
using PunchList.Components.Account;
using PunchList.Data;
using PunchList.Models;
using PunchList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = IdentityConstants.ApplicationScheme;
    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

// DbContext
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? throw new InvalidOperationException("Missing DefaultConnection.");
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(cs));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(o =>
{
    o.SignIn.RequireConfirmedAccount = false; // 'true' if we had email sender
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Services added ----------------------------------------------------
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddScoped<ISubTaskItemService, SubTaskItemService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// ---- demo data ----
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    var users = sp.GetRequiredService<UserManager<ApplicationUser>>();
    var roles = sp.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var r in new[] { "Admin", "User" })
        if (!await roles.RoleExistsAsync(r)) await roles.CreateAsync(new IdentityRole(r));

    async Task Ensure(string email, string role)
    {
        var u = await users.FindByEmailAsync(email);
        if (u == null)
        {
            u = new ApplicationUser { UserName = email, Email = email, DisplayName = email.Split('@')[0] };
            await users.CreateAsync(u, "Passw0rd!");
        }
        if (!await users.IsInRoleAsync(u, role)) await users.AddToRoleAsync(u, role);
    }

    await Ensure("admin@admin.net", "Admin");
    await Ensure("user1@user.net", "User");
    await Ensure("user2@user.net", "User");
    await Ensure("user3@user.net", "User");
    await Ensure("user4@user.net", "User");

    if (!db.Projects.Any())
    {
        var p = new Project { Name = "Project test 1", Description = "PunchList demo" };
        p.Tasks.Add(new TaskItem
        {
            Title = "task test 1",
            Description = "desc test 1",
            Order = 1,
            SubTasks = new List<SubTaskItem> { 
                new() { Title = "task 1 title test 1" }, 
                new() { Title = "task 1 title test 2" },
                new() { Title = "task 1 title test 3" }, 
                new() { Title = "task 1 title test 3" }
            }
        });
        p.Tasks.Add(new TaskItem
        {
            Title = "task test 2",
            Description = "desc test 2",
            Order = 1,
            SubTasks = new List<SubTaskItem> {
                new() { Title = "task 2 title test 1" },
                new() { Title = "task 2 title test 2" },
                new() { Title = "task 2 title test 3" },
                new() { Title = "task 2 title test 3" }
            }
        });
        var p2 = new Project { Name = "Project test 2", Description = "PunchList demo" };
        p2.Tasks.Add(new TaskItem
        {
            Title = "task test 1",
            Description = "desc test 1",
            Status = PunchList.Models.TaskStatus.New,
            Order = 1,
            SubTasks = new List<SubTaskItem> {
                new() { Title = "task 1 title test 1" },
                new() { Title = "task 1 title test 2" },
                new() { Title = "task 1 title test 3" },
                new() { Title = "task 1 title test 3" }
            }
        });
        p2.Tasks.Add(new TaskItem
        {
            Title = "task test 2",
            Description = "desc test 2",
            Status = PunchList.Models.TaskStatus.New,
            Order = 1,
            SubTasks = new List<SubTaskItem> {
                new() { Title = "task 2 title test 1" },
                new() { Title = "task 2 title test 2" },
                new() { Title = "task 2 title test 3" },
                new() { Title = "task 2 title test 3" }
            }
        });
        db.Add(p);
        db.Add(p2);
        await db.SaveChangesAsync();
    }
}

app.Run();
