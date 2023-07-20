// Importando los espacios de nombres necesarios.
using JoseAntonio02.NET.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using Net02.Api.Models.Employees;


namespace Net02.Api.Controllers
{
    // Etiqueta que indica que esta clase es un controlador de API.
    [ApiController]
    // Etiqueta que establece la ruta en la que se accederá a este controlador.
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        // Definiendo una base de datos en memoria para almacenar empleados.
        public static readonly Dictionary<string, GetEmployee> _database = new()
        {
              { "1", new GetEmployee() { Id = "1", FirstName = "John", LastName = "Doe", HiredDate = DateTime.Now, Email = "johndoe@example.com", PhoneNumber = "1234567890", Address = new Address { Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" }, Department = "HR", DateOfBirth = new DateTime(1990, 1, 1), History = new List<EmployeeHistory> { new EmployeeHistory { Date = DateTime.Now, Event = "Hired" } } } },
              { "2", new GetEmployee() { Id = "2", FirstName = "Jane", LastName = "Smith", HiredDate = DateTime.Now, Email = "janesmith@example.com", PhoneNumber = "2345678901", Address = new Address { Street = "234 Elm St", City = "Othertown", State = "NY", ZipCode = "67890" }, Department = "Sales", DateOfBirth = new DateTime(1985, 2, 2), History = new List<EmployeeHistory> { new EmployeeHistory { Date = DateTime.Now, Event = "Hired" } } } },
              { "3", new GetEmployee() { Id = "3", FirstName = "Bob", LastName = "Johnson", HiredDate = DateTime.Now, Email = "bobjohnson@example.com", PhoneNumber = "3456789012", Address = new Address { Street = "345 Pine St", City = "Sometown", State = "TX", ZipCode = "78901" }, Department = "Marketing", DateOfBirth = new DateTime(1980, 3, 3), History = new List<EmployeeHistory> { new EmployeeHistory { Date = DateTime.Now, Event = "Hired" } } } },
              { "4", new GetEmployee() { Id = "4", FirstName = "Alice", LastName = "Williams", HiredDate = DateTime.Now, Email = "alicewilliams@example.com", PhoneNumber = "4567890123", Address = new Address { Street = "456 Oak St", City = "Anothertown", State = "FL", ZipCode = "89012" }, Department = "IT", DateOfBirth = new DateTime(1975, 4, 4), History = new List<EmployeeHistory> { new EmployeeHistory { Date = DateTime.Now, Event = "Hired" } } } },
        };

        // Método para obtener un empleado por nombre.
        [HttpGet("GetByName")]
        public ActionResult GetByName(string firstName, string lastName)
        {
            // Buscar un empleado con el nombre y apellido proporcionados.
            var employee = _database.Values.FirstOrDefault(e => e.FirstName == firstName && e.LastName == lastName);

            // Devolver el empleado si se encuentra, de lo contrario devolver NotFound (404).
            return employee != null ? Ok(employee) : NotFound();
        }

        // Método para obtener un empleado por ID.
        [HttpGet("GetById")]
        public ActionResult GetById(string id)
        {
            // Si el empleado con el ID proporcionado existe, devolverlo, de lo contrario devolver NotFound (404).
            return _database.TryGetValue(id, out GetEmployee? employee) ? Ok(employee) : NotFound();
        }

        // Método para listar empleados.
        [HttpPost("List")]
        public ActionResult List([FromQuery] PaginationCriteria paginationCriteria)
        {
            // Comprobar si los valores de PaginationCriteria son válidos.
            if (paginationCriteria.Page < 1 || paginationCriteria.PageSize < 1)
            {
                return BadRequest("Invalid pagination criteria");
            }

            // Calcular cuántos empleados saltar basándose en la página actual y el tamaño de la página.
            int skip = (paginationCriteria.Page - 1) * paginationCriteria.PageSize;

            // Obtener y ordenar los empleados por apellido.
            var orderedEmployees = _database.Values.OrderBy(e => e.LastName);

            // Aplicar la paginación: saltar y tomar según PageSize.
            var paginatedEmployees = orderedEmployees.Skip(skip).Take(paginationCriteria.PageSize).ToList();

            // Si no hay empleados en la página especificada, devolver NotFound (404).
            if (!paginatedEmployees.Any())
            {
                return NotFound();
            }

            // Devolver los empleados paginados.
            return Ok(paginatedEmployees);
        }

        // Método para añadir un nuevo empleado.
        [HttpPost]
        public ActionResult Create(CreateEmployee createEmployee, [FromQuery] ApiActionCriteria apiActionCriteria)
        {
            // Generar un nuevo ID para el empleado.
            var id = Guid.NewGuid().ToString();

            // Crear un nuevo empleado con los datos proporcionados.
            var newEmployee = new GetEmployee
            {
                Id = id,
                FirstName = createEmployee.FirstName,
                LastName = createEmployee.LastName,
                DateOfBirth = createEmployee.DateOfBirth,
                Email = createEmployee.Email,
                PhoneNumber = createEmployee.PhoneNumber,
                Department = createEmployee.Department,
                Address = createEmployee.Address,
                HiredDate = DateTime.Now,
                History = new List<EmployeeHistory> { new EmployeeHistory { Date = DateTime.Now, Event = "Hired" } }
            };
            // Añadir el nuevo empleado a la base de datos.
            _database.Add(id, newEmployee);

            // Devolver el nuevo empleado si se especificó que se debe devolver, de lo contrario devolver OK.
            return apiActionCriteria.HasReturn ? Ok(newEmployee) : Ok();
        }

        // Método para actualizar un empleado existente.
        [HttpPut]
        public ActionResult Update(string id, GetEmployee employee, [FromQuery] ApiActionCriteria apiActionCriteria)
        {
            // Si el empleado con el ID proporcionado no existe, devolver NotFound (404).
            if (!_database.ContainsKey(id))
            {
                return NotFound();
            }

            // Actualizar el empleado en la base de datos.
            _database[id] = employee;

            // Devolver OK.
            return Ok();
        }

        // Método para eliminar un empleado.
        [HttpDelete]
        public ActionResult Delete(string id, [FromQuery] ApiActionCriteria apiActionCriteria)
        {
            // Si el empleado con el ID proporcionado no existe, devolver NotFound (404).
            // Si existe, eliminarlo y devolverlo si se especificó que se debe devolver, de lo contrario devolver OK.
            return _database.Remove(id, out GetEmployee? result)
                ? apiActionCriteria.HasReturn ? Ok(result) : Ok()
                : NotFound();
        }
    }
}
