# Grupo06-Caso01

API REST desarrollada con ASP.NET Core Web API, Entity Framework Core, PostgreSQL y Swagger. El proyecto administra categorías, productos, proveedores, pedidos y movimientos de inventario.

## 1. Tecnologías utilizadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Neon Database
- Npgsql
- Swagger / OpenAPI
- Git y GitHub
- Patrón Repository
- Generic Repository
- Unit of Work
- Service Layer
- Inyección de dependencias

## 2. Creación del proyecto

El proyecto se creó como una aplicación ASP.NET Core Web API:

```bash
dotnet new webapi -n Grupo06-Caso01
cd Grupo06-Caso01
```

Después se agregó la solución:

```bash
dotnet new sln -n Grupo06-Caso01
dotnet sln add Grupo06-Caso01/Grupo06-Caso01.csproj
```

## 3. Instalación de dependencias

Se instalaron los paquetes necesarios para trabajar con PostgreSQL, Entity Framework Core y Swagger:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore
```

Para verificar los paquetes instalados:

```bash
dotnet list package
```

## 4. Configuración de la base de datos

La aplicación utiliza PostgreSQL alojado en Neon. La cadena de conexión no se guarda directamente en el repositorio.

El archivo `appsettings.json` contiene la configuración general:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=HOST;Port=5432;Database=neondb;Username=USUARIO;Password=CONTRASEÑA;SSL Mode=Require;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

En un entorno real se recomienda utilizar User Secrets o variables de entorno.

## 5. Configuración mediante User Secrets

El proyecto tiene configurado un `UserSecretsId` en el archivo `.csproj`.

Para inicializar User Secrets:

```bash
dotnet user-secrets init
```

Para guardar la conexión:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=HOST;Port=5432;Database=neondb;Username=USUARIO;Password=CONTRASEÑA;SSL Mode=Require;"
```

Para comprobar que existe la configuración:

```bash
dotnet user-secrets list
```

Los secretos se almacenan fuera del proyecto, evitando publicar contraseñas en GitHub.

## 6. Migración inversa o scaffolding

La base de datos ya contenía las tablas y relaciones. Por eso se utilizó Entity Framework Core Scaffolding para generar automáticamente los modelos y el contexto.

Comando utilizado:

```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c ApplicationDbContext --no-onconfiguring -f
```

Significado de los parámetros:

- `dbcontext scaffold`: genera clases desde una base de datos existente.
- `Name=ConnectionStrings:DefaultConnection`: utiliza la cadena configurada.
- `Npgsql.EntityFrameworkCore.PostgreSQL`: proveedor de PostgreSQL.
- `-o Models`: guarda las entidades en la carpeta `Models`.
- `-c ApplicationDbContext`: nombre del contexto.
- `--no-onconfiguring`: evita guardar la conexión dentro del contexto.
- `-f`: sobrescribe archivos existentes.

El scaffolding generó:

- `ApplicationDbContext.cs`
- `Categoria.cs`
- `Producto.cs`
- `Proveedore.cs`
- `Pedido.cs`
- `Movimientosinventario.cs`

## 7. Estructura del proyecto

```text
Grupo06-Caso01/
│
├── Controllers/
│   ├── CategoriaController.cs
│   ├── ProductoController.cs
│   ├── ProveedorController.cs
│   ├── PedidoController.cs
│   └── MovimientoInventarioController.cs
│
├── Dtos/
│   ├── CategoriaRequestDto.cs
│   ├── ProductoRequestDto.cs
│   ├── ProveedorRequestDto.cs
│   ├── PedidoRequestDto.cs
│   └── MovimientoInventarioRequestDto.cs
│
├── Models/
│   ├── ApplicationDbContext.cs
│   ├── Categoria.cs
│   ├── Producto.cs
│   ├── Proveedore.cs
│   ├── Pedido.cs
│   └── Movimientosinventario.cs
│
├── Repositories/
│   ├── IGenericRepository.cs
│   ├── IUnitOfWork.cs
│   ├── ICategoriaRepository.cs
│   ├── IProductoRepository.cs
│   ├── IProveedorRepository.cs
│   ├── IPedidoRepository.cs
│   ├── IMovimientoInventarioRepository.cs
│   └── Implementations/
│       ├── GenericRepository.cs
│       ├── UnitOfWork.cs
│       ├── CategoriaRepository.cs
│       ├── ProductoRepository.cs
│       ├── ProveedorRepository.cs
│       ├── PedidoRepository.cs
│       └── MovimientoInventarioRepository.cs
│
├── Services/
│   ├── ICategoriaService.cs
│   ├── IProductoService.cs
│   ├── IProveedorService.cs
│   ├── IPedidoService.cs
│   ├── IMovimientoInventarioService.cs
│   └── Implementations/
│
├── Program.cs
├── appsettings.json
├── appsettings.example.json
└── Grupo06-Caso01.csproj
```

## 8. Patrón Generic Repository

El repositorio genérico contiene operaciones comunes para todas las entidades:

- Obtener todos los registros.
- Obtener un registro por ID.
- Insertar.
- Actualizar.
- Eliminar.

Interfaz principal:

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    Task<bool> DeleteAsync(int id);
}
```

Esto evita repetir el mismo código en cada repositorio.

## 9. Repositorios específicos

Cada entidad tiene su propia interfaz y clase de repositorio. Esto permite agregar consultas particulares.

Ejemplos:

```csharp
public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Producto>> GetWithLowStockAsync();
}
```

```csharp
public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<IEnumerable<Pedido>> GetByProveedorAsync(int proveedorId);
}
```

Los repositorios específicos contienen consultas propias de cada entidad que no pertenecen al repositorio genérico.

## 10. Patrón Unit of Work

`UnitOfWork` agrupa todos los repositorios y permite guardar los cambios mediante un único contexto de Entity Framework Core.

```csharp
public interface IUnitOfWork
{
    ICategoriaRepository Categorias { get; }
    IProductoRepository Productos { get; }
    IProveedorRepository Proveedores { get; }
    IPedidoRepository Pedidos { get; }
    IMovimientoInventarioRepository MovimientosInventario { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
```

El objetivo es coordinar varias operaciones y confirmar los cambios con:

```csharp
await _uow.SaveChangesAsync();
```

En los movimientos de inventario también se utiliza una transacción para actualizar el stock y registrar el movimiento como una sola operación.

## 11. Capa de servicios

Los servicios contienen las reglas de negocio y utilizan `IUnitOfWork`.

Ejemplo de flujo:

```text
Controller
    ↓
Service
    ↓
UnitOfWork
    ↓
Repository
    ↓
ApplicationDbContext
    ↓
PostgreSQL
```

La capa de servicios valida:

- Campos obligatorios.
- Valores negativos.
- Relaciones existentes.
- Stock disponible.
- Eliminaciones con registros relacionados.
- Estados y fechas válidas.

## 12. Inyección de dependencias

En `Program.cs` se registraron el contexto, repositorios, Unit of Work y servicios utilizando `AddScoped`.

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>),
    typeof(GenericRepository<>));

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IMovimientoInventarioRepository,
    MovimientoInventarioRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IMovimientoInventarioService,
    MovimientoInventarioService>();
```

`AddScoped` crea una instancia por cada solicitud HTTP, lo cual es adecuado para `DbContext`, repositorios y servicios.

## 13. DTOs

Los controllers no reciben directamente las entidades de Entity Framework en las operaciones `POST` y `PUT`.

Se crearon DTOs de entrada para evitar que Swagger solicite propiedades de navegación como `Categoria`, `Proveedor` o `Producto`.

Ejemplo:

```csharp
public class ProductoRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stockactual { get; set; }
    public int Stockminimo { get; set; }
    public int Categoriaid { get; set; }
}
```

El controller convierte el DTO en una entidad antes de enviarla al servicio.

## 14. Endpoints disponibles

### Categorías

```text
GET     /api/categorias
GET     /api/categorias/{id}
POST    /api/categorias
PUT     /api/categorias/{id}
DELETE  /api/categorias/{id}
```

### Productos

```text
GET     /api/productos
GET     /api/productos/{id}
POST    /api/productos
PUT     /api/productos/{id}
DELETE  /api/productos/{id}
```

### Proveedores

```text
GET     /api/proveedores
GET     /api/proveedores/{id}
POST    /api/proveedores
PUT     /api/proveedores/{id}
DELETE  /api/proveedores/{id}
```

### Pedidos

```text
GET     /api/pedidos
GET     /api/pedidos/{id}
POST    /api/pedidos
PUT     /api/pedidos/{id}
DELETE  /api/pedidos/{id}
```

### Movimientos de inventario

```text
GET     /api/movimientos-inventario
GET     /api/movimientos-inventario/{id}
POST    /api/movimientos-inventario
PUT     /api/movimientos-inventario/{id}
DELETE  /api/movimientos-inventario/{id}
```

## 15. Swagger

La interfaz Swagger se habilitó en `Program.cs`:

```csharp
builder.Services.AddSwaggerGen();

app.UseSwagger();
app.UseSwaggerUI();
```

Para ejecutar el proyecto:

```bash
dotnet run
```

Luego se accede a:

```text
http://localhost:5002/swagger
```

También puede utilizarse la URL HTTPS definida en `launchSettings.json`.

Swagger permite ejecutar y verificar los endpoints directamente desde el navegador.

## 16. Ejemplos de solicitudes

### Crear una categoría

```json
{
  "nombre": "Tecnología"
}
```

### Crear un producto

```json
{
  "nombre": "Laptop Lenovo",
  "descripcion": "Laptop para trabajo",
  "precio": 2500,
  "stockactual": 10,
  "stockminimo": 2,
  "categoriaid": 1
}
```

### Crear un proveedor

```json
{
  "nombre": "Proveedor Principal",
  "contacto": "Juan Pérez",
  "telefono": "999999999",
  "email": "proveedor@correo.com"
}
```

### Crear un pedido

```json
{
  "proveedorid": 1,
  "fechapedido": "2026-09-15T10:00:00",
  "estado": "Pendiente"
}
```

### Crear un movimiento de inventario

```json
{
  "productoid": 1,
  "tipomovimiento": "Entrada",
  "cantidad": 5,
  "fecha": "2026-09-15T10:00:00"
}
```

## 17. Validaciones implementadas

- No se permiten nombres vacíos.
- No se permiten precios negativos.
- No se permiten cantidades negativas.
- La categoría debe existir antes de crear un producto.
- El proveedor debe existir antes de crear un pedido.
- El producto debe existir antes de crear un movimiento.
- Un movimiento de salida no puede dejar stock negativo.
- No se eliminan categorías con productos relacionados.
- No se eliminan proveedores con pedidos relacionados.
- No se eliminan productos con movimientos relacionados.
- Los estados de pedido tienen una longitud máxima de 50 caracteres.

## 18. Comandos de compilación y prueba

Restaurar dependencias:

```bash
dotnet restore
```

Compilar:

```bash
dotnet build
```

Ejecutar:

```bash
dotnet run
```

Verificar el proyecto:

```bash
dotnet build --no-restore
```

## 19. Flujo de Git utilizado

Clonar el repositorio:

```bash
git clone https://github.com/AndrewMechDev/Grupo06-Caso01.git
cd Grupo06-Caso01
```

Actualizar `develop`:

```bash
git fetch origin
git switch develop
git pull origin develop
```

Crear una rama de trabajo:

```bash
git switch -c feature/repositories origin/develop
```

Subir una rama:

```bash
git add .
git commit -m "feat: implementar repositorios"
git push -u origin feature/repositories
```

Actualizar una rama con `develop`:

```bash
git switch nombre-de-la-rama
git fetch origin
git merge origin/develop
```

Integrar una rama en `develop`:

```bash
git switch develop
git pull origin develop
git merge nombre-de-la-rama
git push origin develop
```

## 20. Buenas prácticas aplicadas

- Separación por capas.
- Uso de interfaces.
- Inyección de dependencias.
- Reutilización mediante Generic Repository.
- Consultas específicas en repositorios especializados.
- Reglas de negocio dentro de Services.
- DTOs para recibir datos de la API.
- Uso de transacciones en movimientos de inventario.
- Uso de User Secrets para credenciales.
- No publicar contraseñas en GitHub.
- Validación de datos antes de guardar.
- Uso de operaciones asíncronas con `async` y `await`.
- Documentación y pruebas mediante Swagger.

## 21. Resultado final

El proyecto permite administrar las entidades principales del sistema mediante una API REST organizada en capas. Entity Framework Core conecta la aplicación con PostgreSQL, los repositorios encapsulan el acceso a datos, Unit of Work coordina las operaciones, los servicios contienen las reglas de negocio y los controllers exponen los endpoints mediante Swagger.
