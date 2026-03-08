using AutoMapper;
using Consulate.Application.DTOs;
using Consulate.Application.Features.Employees.Commands;
using Consulate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Mappings
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            // Mapping من Entity إلى DTO
            CreateMap<Employee, EmployeeDto>();

            // Mapping من CreateEmployeeCommand إلى Entity
            CreateMap<CreateEmployeeCommand, Employee>();

        }
    }
}
