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

            CreateMap<Employee, EmployeeDto>()
               .ForMember(dest => dest.Email,
          opt => opt.MapFrom(src => src.User.Email))

               .ForMember(dest => dest.RoleName,
          opt => opt.MapFrom(src =>
              src.User.UserRoles
                  .Select(ur => ur.Role.Name)
                  .FirstOrDefault()
          ));

            // Mapping من CreateEmployeeCommand إلى Entity
            CreateMap<CreateEmployeeCommand, Employee>();

        }
    }
}
