using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Carpool.Contracts.Request;
using Carpool.Contracts.Response;
using Carpool.Controllers;
using Carpool.Data;
using Carpool.Database;
using Carpool.Service;
using FluentAssertions;
using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;
using Xunit.Sdk;

namespace OkrApp.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private Fixture _fixture = new Fixture();

        [Fact]
        public void GetAvailableEmployees_WhenNoEmployees_ReturnsNoContent()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(es => es.GetAvailableEmployees(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((false, Enumerable.Empty<Employee>()));

            var request = _fixture.Create<AvailableEmployeesRequest>();
            MyMvc
                .Controller<EmployeeController>(instance => instance
                    .WithDependencies(employeeServiceMock.Object)
                )
                .Calling(c => c.GetEmployees(request))
                .ShouldReturn()
                .NoContent();
        }

        [Fact]
        public void GetAvailableEmployees_WhenEmployeesAvailable_ReturnsOk()
        {
            var employees = SeedData.GenerateEmployees(2);
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(es => es.GetAvailableEmployees(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((true, employees));

            var request = _fixture.Create<AvailableEmployeesRequest>();

            MyMvc
                .Controller<EmployeeController>(instance => instance
                    .WithDependencies(employeeServiceMock.Object)
                )
                .Calling(c => c.GetEmployees(request))
                .ShouldReturn()
                .Ok(result => result.WithModelOfType<IEnumerable<EmployeeDto>>()
                    .Passing(model =>
                    {
                        model.Should().NotBeEmpty();
                        model.Should().HaveCount(employees.Count);
                    }));
            ;
        }
    }
}