We already discussed Onion/Clean Architecture in our previous article. Please check out this article [Clean Architecture Overview](https://www.arunyadav.in/blogs/post/20/clean-architecture-overview). 

![Clean Architecture Onion View](https://www.dropbox.com/s/vsvqhcomy9nyant/Clean%20Architectur%20Onion%20View.png?raw=1 "Clean Architecture Onion View")

### Let's look at Solution Components
![Solution Components](https://www.dropbox.com/s/mdwo0zlgnxmjlfg/Solution-Structure.png?raw=1 "Solution Components")

**_As observed, the solution is comprised of an `OnionDemo.Web` project serving as the ASP.NET Core application and other class library projects. The `OnionDemo.Domain` project contains the implementation of the Domain layer, while `OnionDemo.Services` make up the Service layer. The `OnionDemo.Persistence` project represents the Infrastructure layer and the Presentation project implements the Presentation layer._**

> Let's delve into each layer in more detail (In this article you will find `pingpong` as an example)

### **OnionDemo.Domain**

- Entities
- Exceptions
- Repositories

Let's look at the code in **`Entities`**
```csharp
namespace OnionDemo.Domain.Entities
{
    public class PingPong
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public DateTimeOffset? PublishedDate { get; set; }
    }
}
```

Let's look at next the code in **`Repositories`**

```csharp
namespace OnionDemo.Domain.Repositories
{
    public interface IPingPongRepository
    {
        Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellation);
    }
}

```


```csharp
namespace OnionDemo.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
```

> `IRepositoryBase<T>` where T is a generic type. The interface defines several methods for basic CRUD (Create, Read, Update, Delete) operations on an entity of type T. This interface provides a way to encapsulate the basic CRUD operations and can be implemented by concrete classes to provide a specific implementation of these operations for a particular type of entity.

Let's look at the code in **`Domain Related Exceptions`** in the **`Exceptions`** folder.
```csharp
namespace OnionDemo.Domain.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        {
        }
    }
}
```
```csharp
namespace OnionDemo.Domain.Exceptions
{
    public sealed class PingPongNotFoundException : NotFoundException
    {
        public PingPongNotFoundException(int postId)
            : base($"The Ping Pong with the identifier {postId} was not found.")
        {
        }
    }
}
```

--------------------------------------------
### **OnionDemo.Service**

```csharp
namespace OnionDemo.Services
{
    public sealed class PingPongService : IPingPongService
    {
        private readonly IRepositoryManager _repositoryManager;
        public PingPongService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

        public async Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken)
        {
            var pongPongs = await _repositoryManager.PingPongRepository.GetAllPingPongAsync(cancellationToken);
            throw new NotImplementedException();
        }
    }

    //We can seperate it out in Services.Abstractions
    public interface IPingPongService
    {
        Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken);
    }
}
```
This is place where we will keep all the services and it's abstract interfaces so these can further consumed by our presentation layer with help of `ServiceManager`

> You will also notice **`IRepositoryManager`** which provide a common interface for working with repositories, allowing for better separation of concerns and more flexible code, as the specific implementation of repositories can change without affecting the code that uses them.

> We will talk more about **`IRepositoryManager`**

```csharp
namespace OnionDemo.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IPingPongService> _lazyPingPongService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyPingPongService = new Lazy<IPingPongService>(() => new PingPongService(repositoryManager));
        }

        public IPingPongService PingPongService => _lazyPingPongService.Value;
    }

    public interface IServiceManager
    {
        IPingPongService PingPongService { get; }
    }
}
```
> **`IServiceManager`** provide a common interface for working with services, allowing for better separation of concerns and more flexible code, as the specific implementation of services can change without affecting the code that uses them.

--------------------------------------------------------------------

### **OnionDemo.Persistance**
![Persistence Layer](https://www.dropbox.com/s/f8ulra8ux14y5qv/Persistance-layer.png?raw=1 "Persistence Layer")

> In the **Onion Architecture/Clean Architecture**, the **`Persistence layer`** is part of the Infrastructure and is responsible for data storage and retrieval. This layer acts as an intermediary between the **higher-level Domain layer** and the **lower-level** data storage systems, such as databases or file systems. The Persistence layer provides abstractions and APIs for working with the data storage, allowing the higher-level layers to be independent of the specific data storage implementation. By separating the concerns of data storage and retrieval into the Persistence layer, the Onion Architecture/Clean Architecture helps to promote a clean separation of concerns, making the system more maintainable and scalable.

```csharp
namespace OnionDemo.Persistance.Repositories
{
    public sealed class PingPongRepository: IPingPongRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public PingPongRepository(RepositoryDbContext dbContext)
        {
            _dbContext= dbContext;
        }

        public async Task<IEnumerable<PingPong>> GetAllPingPongAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.PingPong.ToListAsync(cancellationToken);
        }
    }
}

```
> **`PingPongRepository`** is a specific repository class in C#. It likely implements the basic CRUD operations for a specific entity related to a Ping Pong game, such as keeping track of the scores, the players, and the game rules.

```csharp
namespace OnionDemo.Persistance.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IPingPongRepository> _lazyPingPongRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _lazyPingPongRepository = new Lazy<IPingPongRepository>(() => new PingPongRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IPingPongRepository PingPongRepository => _lazyPingPongRepository.Value;

        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
```
> Here is implementation of **`RepositoryManager`** and **`IRepositoryManager`** interface comes from **`Domain`** layer repositories folder

![Presentation Layer](https://www.dropbox.com/s/2w0jgyhoma1sl7r/presentation-layer.png?raw=1 "Presentation Layer")


```csharp
namespace OnionDemo.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingPongController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PingPongController(IServiceManager serviceManager) => _serviceManager = serviceManager;

        [HttpGet]
        public async Task<IActionResult> GetPingPongs(CancellationToken cancellationToken)
        {
            var pingPongDto = await _serviceManager.PingPongService.GetAllPingPongAsync(cancellationToken);

            return Ok(pingPongDto);
        }
    }
}
```

> Note here that we are injecting **`IServiceManager`** and from this we are accessing object for **`PingPongService`**, the benefit with this approach, if you have a other dependent service which need to injected then you really don't need to inject here in Controller, you can **`IServiceManager`** and `Lazy` load that service.


------------------------------------------------------------------------
### **OnionDemo.Web**

> UI Layer contains necessary bootstrapper for your backend services through **`Program.cs`**

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnionDemo.Domain.Repositories;
using OnionDemo.Persistance;
using OnionDemo.Persistance.Repositories;
using OnionDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("pingongDB");

// Add services to the container.
builder.Services.AddControllers()
               .AddApplicationPart(typeof(OnionDemo.Presentation.AssemblyReference).Assembly);

builder.Services.AddScoped<IServiceManager, ServiceManager>();

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

builder.Services.AddSwaggerGen(c =>
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }));

builder.Services.AddDbContextPool<RepositoryDbContext>(option =>
{
    option.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();

```

> Note to add your controller part from other library in **`Program.cs`**

```
builder.Services.AddControllers()
               .AddApplicationPart(typeof(OnionDemo.Presentation.AssemblyReference).Assembly);
```

> Here is full code to run through swagger, Please download from [Github](https://github.com/engg-aruny/OnionTemplate)

> Here is URL https://localhost:44461/swagger/index.html

![Swagger Output](https://www.dropbox.com/s/um3gjxwyvsqv4ei/swagger.png?raw=1 "Swagger Output")

### Conclusion
The Onion Architecture/Clean Architecture consists of several layers, each with a specific purpose and responsibility. The main layers are:

- **Presentation layer:** This layer contains the user interface and is responsible for presenting data to the user and accepting user input.
- **Application layer:** This layer contains the application logic, such as validation and orchestration of the domain layer.
- **Domain layer:** This layer contains the core business logic and entities that represent the business concepts.
- **Infrastructure layer:** This layer contains the data access and storage logic and is responsible for persisting and retrieving data.

Each layer is separated from the others, and dependencies are only allowed to flow inwards, from the outer layers towards the inner layers. This ensures that the higher-level layers are not tightly coupled to the lower-level layers and can be easily changed or updated without affecting the rest of the system.

The importance of each layer can be seen in how it helps to promote separation of concerns and improve the **maintainability, scalability, and testability** of the system. By having well-defined layers with specific responsibilities, the system becomes more organized and easier to understand and modify, making it easier to evolve and adapt over time.
