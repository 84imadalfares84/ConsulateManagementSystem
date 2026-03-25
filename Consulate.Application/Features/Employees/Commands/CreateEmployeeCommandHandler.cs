using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using MediatR;
using AutoMapper;

namespace Consulate.Application.Features.Employees.Commands
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public CreateEmployeeCommandHandler(
            IEmployeeRepository employeeRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            //  إنشاء Employee
            var employee = _mapper.Map<Employee>(request);
            employee.Id = Guid.NewGuid();

            //  إنشاء User مرتبط بالموظف
            //var defaultPassword = "123456"; // كلمة مرور افتراضية
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                UserRoles = new List<UserRole>()
            };

            //  تحديد الدور (افتراضي "Officer")
            var roleName = string.IsNullOrEmpty(request.RoleName) ? "Officer" : request.RoleName;
            var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);

            if (role != null)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }

            //  ربط Employee بالـ User
            employee.UserId = user.Id;

            //  إضافة الكيانات إلى الـ Repository
            await _userRepository.AddAsync(user);
            await _employeeRepository.AddAsync(employee);

            return employee.Id;
        }
    }
}