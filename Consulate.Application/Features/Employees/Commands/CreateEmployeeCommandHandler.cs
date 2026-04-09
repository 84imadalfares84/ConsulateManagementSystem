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
            // انشاء اليوزر
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                UserRoles = new List<UserRole>()
            };

            // تحديد دور اليوزر واذا لم يحدد نضعه افسر
            var roleName = string.IsNullOrWhiteSpace(request.RoleName)
                ? "Officer"
                : request.RoleName;

            var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);

            if (role == null)
                throw new Exception($"Role '{roleName}' not found");

            // ربط اليوزر مع الرول
            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            // انشاء الموظف وربطه باليوزر
            var employee = _mapper.Map<Employee>(request);
            employee.Id = Guid.NewGuid();
            employee.UserId = user.Id;

            //  حفظ
            await _userRepository.AddAsync(user);
            await _employeeRepository.AddAsync(employee);

            return employee.Id;
        }
    }

}