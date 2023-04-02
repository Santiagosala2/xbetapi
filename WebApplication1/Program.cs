using Auth.UserManager;
using DataStore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Dtos;
using Users.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Wallet.Dtos;
using Wallet.Manager;
using Search.Manager;
using Search.Users.Dtos;
using Friends.Dtos;
using Bets.Models;
using Bets.Manager;
using Bets.Dtos;
using System.Text.Json.Serialization;

const string CookieScheme = "BetUserManager";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowClient",
                    builder =>
                    {
                    builder.WithOrigins("https://localhost:4001")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
});

builder.Services.AddDbContext<DataStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BetDBConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(CookieScheme)
    .AddCookie(CookieScheme);

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IWalletManager, WalletManager>();
builder.Services.AddScoped<ISearchManager, SearchManager>();
builder.Services.AddScoped<IBetsManager, BetsManager>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("allowClient");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapPost("api/verify", () => true).RequireAuthorization()
.WithName("Verify");

app.MapPost("api/login", async  (HttpContext ctx, IUserManager userManager, IMapper mapper, [FromBody] UserReadDto  user) =>
{

    // 1. Verify payload - email and password correct
    var userModel = mapper.Map<User>(user);
    //2.Check if user exists in the db // if yes - check the password provided - hash it
    var result =  await userManager.VerifyUserAsync(user.Email,user.Password);
    if (!result)
    {
        // not throw error
        return Results.NotFound(new { errorMessage = "Invalid email or password" });
    }

    if (result)
    {
        // 4. create user claim and sign cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email)
        };
        await ctx.SignInAsync(
            CookieScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieScheme)),
            new AuthenticationProperties()
            {
                IsPersistent = true
            } 
        );
    }

    return Results.Ok(result);
})
.WithName("Login");


app.MapPost("api/register", async (HttpContext ctx, IUserManager userManager, IMapper mapper, [FromBody] UserCreateDto user) =>
{
    // 1. Do validation
    // 2. Map payload from user to class model
    // 3. Add user model into db
    var userModel = mapper.Map<User>(user);
    var result = await userManager.CreateUserAsync(userModel);
    if (!result)
    {
        return Results.BadRequest(new { errorMessage = "User already exists" });

    }

    return Results.Ok(result);


})
.WithName("RegisterUser");

app.MapGet("api/user", async (HttpContext ctx, IUserManager userManager, IMapper mapper) =>
{

   var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
   if (userEmail != null)
   {
       var user = await userManager.GetUserAsync(userEmail);
       var userModel = mapper.Map<UserProfileDto>(user);
        return Results.Ok(userModel);

   }
     
   return Results.BadRequest(new { errorMessage = "User not found" });
})
.WithName("GetUser")
.RequireAuthorization();

// Deposit
app.MapPost("api/user/deposit", async (HttpContext ctx, IUserManager userManager, IWalletManager walletManager, IMapper mapper , [FromBody] WalletDepositDto deposit) =>
{

    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    if (userEmail != null)
    {
        bool succesfulDeposit = false;
        decimal currentBalance = 0;
        var user = await userManager.GetUserAsync(userEmail);
        (succesfulDeposit, currentBalance) = await walletManager.DepositAsync(user, deposit.Amount,deposit.PaymentProvider);
        if (succesfulDeposit)
        {
            return Results.Ok(new { balance = currentBalance});
        }
    }
    return Results.BadRequest(new { errorMessage = "Something went wrong" });
})
.WithName("UserDeposit")
.RequireAuthorization();

app.MapGet("api/user/friends", async (HttpContext ctx, IUserManager userManager, IMapper mapper) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    var friendsResult = await userManager.GetFriendsAsync(userEmail!);
    return Results.Ok(friendsResult);
})
.WithName("GetFriends")
.RequireAuthorization();


app.MapGet("api/search/user", async (HttpContext ctx, ISearchManager search , IMapper mapper, [FromQuery] string value) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    var usersResult = await search.GetUsersAsync(value,userEmail!);
    return Results.Ok(usersResult);
})
.WithName("SearchUser")
.RequireAuthorization();


app.MapPost("api/user/addFriend", async (HttpContext ctx, IUserManager userManager, IMapper mapper, [FromBody] FriendshipCreateDto friendship ) =>
{
    var (usersResult,errorMessage) = await userManager.AddFriendRequest(friendship.FromEmail, friendship.ToEmail);
    if (!usersResult)
    {
        return Results.BadRequest(errorMessage);
    }
    return Results.Ok();
})
.WithName("AddFriend")
.RequireAuthorization();

app.MapPost("api/user/acceptRequest", async (HttpContext ctx, IUserManager userManager, IMapper mapper, [FromBody] FriendshipRequestDto friendship) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    var (usersResult, errorMessage) = await userManager.AcceptRequest(friendship.FriendshipId , userEmail!);
    if (!usersResult)
    {
        return Results.BadRequest(errorMessage);
    }
    return Results.Ok();
})
.WithName("AceeptRequest")
.RequireAuthorization();


app.MapPost("api/user/rejectRequest", async (HttpContext ctx, IUserManager userManager, IMapper mapper, [FromBody] FriendshipRequestDto friendship) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    var (usersResult, errorMessage) = await userManager.RejectRequest(friendship.FriendshipId, userEmail!);
    if (!usersResult)
    {
        return Results.BadRequest(errorMessage);
    }
    return Results.Ok();
})
.WithName("RejectRequest")
.RequireAuthorization();

app.MapPost("api/user/bets", async (HttpContext ctx, IUserManager userManager, IBetsManager betManager, IMapper mapper, [FromBody] CreateBetDto bet) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    var betCreated = false;
    var betCreatedId = 0;
    if (userEmail != null)
    {
        var user = await userManager.GetUserAsync(userEmail);
        if (user != null )
        {
            (betCreated, betCreatedId) = await betManager.CreateBetAsync(bet);
        }       
    }

    if (betCreated)
    {
        return Results.Created($"/user/bet/{betCreatedId}",bet);
    }

    return Results.BadRequest("Something went wrong");
})
.WithName("CreateBet")
.RequireAuthorization();

app.MapGet("api/user/bets", async (HttpContext ctx, IUserManager userManager, IBetsManager betManager, IMapper mapper) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    var getUserBets = new List<Bet>();
    var awaitingUserBets = new List<Bet>();
    if (userEmail != null)
    {
        var user = await userManager.GetUserAsync(userEmail);
        if (user != null)
        {
             (getUserBets, awaitingUserBets) = await betManager.GetUserBetsAsync(user.UserID);
              List<ReadBetDto> userBets = mapper.Map<List<ReadBetDto>>(getUserBets);
              List<ReadBetDto> awaitingBets = mapper.Map<List<ReadBetDto>>(awaitingUserBets);
            return Results.Ok(new
            {
                Pending = userBets,
                Awaiting = awaitingBets
            });
        }
    }

    return Results.BadRequest("Something went wrong");
})
.WithName("GetBets")
.RequireAuthorization();


app.MapGet("api/user/bet/{id}", async (int betId, HttpContext ctx , IUserManager userManager, IBetsManager betManager, IMapper mapper) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    if (userEmail != null)
    {
        var user = await userManager.GetUserAsync(userEmail);
        if (user != null)
        {
          var bet = await betManager.GetBetAsync(betId, user.UserID);
          if (bet != null) return Results.Ok(bet);
          return Results.NotFound();
        }     
    }
    return Results.BadRequest("Something went wrong");
})
.WithName("GetBet")
.RequireAuthorization();

app.MapPost("api/user/bet/{id}/accept", async (string id, HttpContext ctx, IUserManager userManager, IBetsManager betManager, IMapper mapper, [FromBody] AnswerClimateBetDto answer ) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    if (userEmail != null)
    {
        var user = await userManager.GetUserAsync(userEmail);
        if (user != null)
        {
            int betIdInt = Int32.Parse(id);
            var updateBet = await betManager.AcceptBetAsync(betIdInt, user.UserID , answer.FriendClimate);
            if (updateBet) return Results.Ok(updateBet);
            return Results.NotFound();
        }
    }
    return Results.BadRequest("Something went wrong");
})
.WithName("AcceptBet")
.RequireAuthorization();

app.MapPost("api/user/bet/{id}/reject", async (string id, HttpContext ctx, IUserManager userManager, IBetsManager betManager, IMapper mapper) =>
{
    var userEmail = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
    if (userEmail != null)
    {
        var user = await userManager.GetUserAsync(userEmail);
        if (user != null)
        {
            int betIdInt = Int32.Parse(id);
            var updateBet = await betManager.RejectBetAsync(betIdInt);
            if (updateBet) return Results.Ok(updateBet);
            return Results.NotFound();
        }
    }
    return Results.BadRequest("Something went wrong");
})
.WithName("RejectBet")
.RequireAuthorization();


app.Run();

