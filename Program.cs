using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//add this to be able to read connection string from key vault
//in key vault create connection string like this
//ConnectionStrings--DefaultConnection and then add the value of connection string    DefaultConnection can be anything you want, it is just a name for the connection string
//after this you can read your connection string in your code as configuration.GetConnectionString("DefaultConnection"). just as usual
//you don't need to add it in appsettings.json, it will be read from key vault
builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["AzureKeyVaultUrl"]!),
    new DefaultAzureCredential() //connects locally using Visual Studio or Azure CLI credentials, and in production it will use managed identity if available
);

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

app.UseAuthorization();

app.MapControllers();

app.Run();

//add these roles to your key vault. go to key vault > iam > add role assignment > select the role and assign it to the user or service principal that will be accessing the key vault. the roles are:
//key vault reader
//key vault secrets user
//key vault secrets officer
