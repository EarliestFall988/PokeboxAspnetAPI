var builder = WebApplication.CreateBuilder(args);

var CORSFix = "*"; //allowing origins

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORSFix, policy =>
    {
        //should allow any kind of CORS request. Normally can be a security flaw, but for the scope of the project we will leave it like this.
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

// Add services to the container.

builder.Services.AddControllers();
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

app.UseCors(CORSFix); //adding cors to the app

app.MapControllers();

// try this in postman: https://localhost:7071/test/<your name>
app.MapGet("/test/{name}", (string name) => "{\n\'status\': \'200\',\n\'message\': \'Hello " + name + "\'\n}"); //used to test origins to see if the server is running

//you should get status: 200, message: 'Hello <your name>'

app.Run();
