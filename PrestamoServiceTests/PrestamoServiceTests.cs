using Application.DTOs;
using Infraestructure.Data;
using Infraestructure.Services;
using Microsoft.EntityFrameworkCore;

namespace PrestamoServiceTests
{
    public class PrestamoServiceTests
    {
        [Fact]
        public async Task SolicitarPrestamoAsync_CreaPrestamoCorrectamente()
        {
            // Arrange: contexto en memoria
            var options = new DbContextOptionsBuilder<PRUEBA_MAKERS_DBCONTEXT>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using var context = new PRUEBA_MAKERS_DBCONTEXT(options);
            var service = new PrestamoService(context);

            var dto = new PrestamoSolicitudDto
            {
                UsuarioId = 1,
                Monto = 5000,
                PlazoMeses = 12
            };

            // Act
            var resultado = await service.SolicitarPrestamoAsync(dto);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task SolicitarPrestamoAsync_DatosInvalidos_RetornaFalse()
        {
            var options = new DbContextOptionsBuilder<PRUEBA_MAKERS_DBCONTEXT>()
            .UseInMemoryDatabase(databaseName: "TestDb2") // Usa un nombre distinto para cada test
            .Options;

            using var context = new PRUEBA_MAKERS_DBCONTEXT(options);
            var service = new PrestamoService(context);

            var dto = new PrestamoSolicitudDto
            {
                UsuarioId = 1,
                Monto = 0,
                PlazoMeses = 0
            };

            var resultado = await service.SolicitarPrestamoAsync(dto);

            Assert.False(resultado);
        }
    }
}