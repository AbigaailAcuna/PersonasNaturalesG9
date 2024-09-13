using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonasNaturalesG9.Controllers;
using PersonasNaturalesG9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Personas.Test
{
    public class PersonasControllerTest
    {
        [Fact]
        public async Task PostPersona_AgregarPersona_CuandoPersonaEsValido()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);
            var nuevapersona = new Persona
            {
               PrimerNombre = "Erika",
               SegundoNombre = null,
               PrimerApellido = "Acuña",
               SegundoApellido = null,
               Dui = "06357032-2",
               FechaNacimiento = new DateTime(2002,9,5)
            };

            var result = await controller.PostPersona(nuevapersona);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var persona = Assert.IsType<Persona>(createdResult.Value);
            Assert.Equal("Erika", persona.PrimerNombre);

        }
        [Fact]
        public async Task GetPersona_RetornaPersona_CuandoIdEsValido()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);
            var persona = new Persona { PrimerNombre = "Lorena", SegundoNombre= "Marisol", PrimerApellido = "Melara", SegundoApellido = "Martínez",
                Dui= "123456678-3", FechaNacimiento = new DateTime(1998, 5, 6) };
            context.Persona.Add(persona);
            await context.SaveChangesAsync();

            var result = await controller.GetPersona(persona.Id);

            var actionResult = Assert.IsType<ActionResult<Persona>>(result);
            var returnValue = Assert.IsType<Persona>(actionResult.Value);
            Assert.Equal("Lorena", returnValue.PrimerNombre);
        }
        [Fact]
        public async Task GetPersona_RetornaNotFound_CuandoIdNoExiste()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);
            var result = await controller.GetPersona(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }
        //Validación de Primer Nombre
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoPrimerNombreEsNulo()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);
            //probando nombre nulo
            var primerpersona = new Persona
            {
                PrimerNombre = "Mónica",
                SegundoNombre = "Abigail",
                PrimerApellido = "Melara",
                SegundoApellido = "López",
                Dui = "12347678-3",
                FechaNacimiento = new DateTime(1999, 5, 6)
            };
            await controller.PostPersona(primerpersona);
            var nuevapersona = new Persona
            {
                PrimerNombre = "Daniela",
                SegundoNombre = "Nicole",
                PrimerApellido = "Turcios",
                SegundoApellido = "Melara",
                Dui = "11476678-3",
                FechaNacimiento = new DateTime(2002, 5, 6)
            };

            await controller.PostPersona(nuevapersona);
            var personas = await context.Persona.ToListAsync();

            Assert.Equal(2, personas.Count);
        }
        //validación de primer apellido
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoPrimerApellidoEsNulo()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Michelle",
                SegundoNombre = "Abigail",
                PrimerApellido = "Linares", 
                SegundoApellido = "López",
                Dui = "12344578-3",
                FechaNacimiento = new DateTime(2001, 5, 6)
            };

            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación campos opcionales
        [Fact]
        public async Task PostPersona_AgregaPersona_CuandoSegundoNombreYSegundoApellidoSonOpcionales()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var nuevapersona = new Persona
            {
                PrimerNombre = "Carlos",
                SegundoNombre = null,  // Segundo nombre opcional
                PrimerApellido = "Molina",
                SegundoApellido = null,  // Segundo apellido opcional
                Dui = "98765432-1",  
                FechaNacimiento = new DateTime(1986, 8, 15)  
            };

            var result = await controller.PostPersona(nuevapersona);

            Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }

        //Validación de longitud para primernombre
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoPrimerNombreExcedeLongitud()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Erika", 
                SegundoNombre = "Abigail",
                PrimerApellido = "Melara",
                SegundoApellido = "López",
                Dui = "12347061-3",
                FechaNacimiento = new DateTime(1999, 5, 6)
            };

            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación longitud para segundo nombre
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoSegundoNombreExcedeLongitud()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Katherine",
                SegundoNombre = "Abigail",
                PrimerApellido = "Melara",
                SegundoApellido = "Sibrian",
                Dui = "12347478-3",
                FechaNacimiento = new DateTime(1999, 5, 6)
            };
            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación longitud para primer apellido
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoPrimerApellidoExcedeLongitud()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Wilson",
                SegundoNombre = "Alejandro",
                PrimerApellido = "Melara",
                SegundoApellido = "Salguero",
                Dui = "12347479-3",
                FechaNacimiento = new DateTime(1873, 5, 6)
            };

            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación longitud para primer apellido
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoSegundoApellidoExcedeLongitud()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "David",
                SegundoNombre = "Alejandro",
                PrimerApellido = "Melara",
                SegundoApellido = "Salguero",
                Dui = "12347479-3",
                FechaNacimiento = new DateTime(1873, 5, 6)
            };

            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación de Dui
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoDuiIncorrecto()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Mónica",
                SegundoNombre = "Abigail",
                PrimerApellido = "Melara",
                SegundoApellido = "López",
                Dui = "12345672-1", 
                FechaNacimiento = new DateTime(1978, 5, 6)
            };

            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación de fecha nula
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoFechaNacimientoEsNula()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Mónica",
                SegundoNombre = "Abigail",
                PrimerApellido = "Melara",
                SegundoApellido = "López",
                Dui = "12347667-3",
                FechaNacimiento = new DateTime(2005, 7, 6)
            };
            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }
        //validación de fecha inválida
        [Fact]
        public async Task PostPersona_NoAgregaPersona_CuandoFechaNacimientoEsInvalida()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new PersonasController(context);

       
            var persona = new Persona
            {
                PrimerNombre = "Ana",
                SegundoNombre = "Andrea",
                PrimerApellido = "Melara",
                SegundoApellido = "Hernández",
                Dui = "87114321-0",  
                FechaNacimiento = new DateTime(2003, 5, 3) 
            };

            var result = await controller.PostPersona(persona);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personas = await context.Persona.ToListAsync();
            Assert.Single(personas);
        }


    }
}
