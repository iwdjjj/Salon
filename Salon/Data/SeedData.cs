using Salon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Salon.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (context.Groups.Any())
            {
                return;
            }

            Customer customer = new Customer
            {
                Surname = "Александрова",
                Name = "Александра",
                Midname = "Александровна",
                Phone = "89001001010",
                Address = "ул. Соколова, 6"
            };
            context.Customers.Add(customer);
            context.SaveChanges();

            Customer customer1 = new Customer
            {
                Surname = "Васильева",
                Name = "Вера",
                Midname = "Васильевна",
                Phone = "89001002020",
                Address = "ул. Соколова, 7"
            };
            context.Customers.Add(customer1);
            context.SaveChanges();

            Group group = new Group
            {
                GroupName = "Имиджевые",
                Description = "",
                Services_Count = 0
            };
            context.Groups.Add(group);
            context.SaveChanges();

            Group group1 = new Group
            {
                GroupName = "Медицинские",
                Description = "",
                Services_Count = 0
            };
            context.Groups.Add(group1);
            context.SaveChanges();

            Job job = new Job
            {
                JobName = "Парикмахер-стилист",
                GroupId = group.GroupId,
                Schedule = "8:00-18:00"
            };
            context.Jobs.Add(job);
            context.SaveChanges();

            Job job1 = new Job
            {
                JobName = "Косметолог",
                GroupId = group1.GroupId,
                Schedule = "8:00-18:00"
            };
            context.Jobs.Add(job1);
            context.SaveChanges();

            Employee employee = new Employee
            {
                Surname = "Александрова",
                Name = "Александра",
                Midname = "Александровна",
                JobId = job.JobId,
                Phone = "89001001010",
                Address = "ул. Соколова, 6",
                EmpDate = new DateTime(2022, 9, 27, 18, 0, 0),
            };
            context.Employees.Add(employee);
            context.SaveChanges();

            Employee employee1 = new Employee
            {
                Surname = "Иванова",
                Name = "Ксения",
                Midname = "Викторовна",
                JobId = job1.JobId,
                Phone = "89001001010",
                Address = "ул. Соколова, 6",
                EmpDate = new DateTime(2022, 9, 27, 18, 0, 0),
            };
            context.Employees.Add(employee1);
            context.SaveChanges();

            Service service = new Service
            {
                ServiceName = "Стрижка",
                GroupId = group.GroupId,
                ProductionCost = 1000,
                Price = 1500,
                Description = ""
            };
            context.Services.Add(service);
            context.SaveChanges();

            Service service1 = new Service
            {
                ServiceName = "Чистка лица",
                GroupId = group1.GroupId,
                ProductionCost = 1000,
                Price = 1500,
                Description = ""
            };
            context.Services.Add(service1);
            context.SaveChanges();

            Visit visit = new Visit
            {
                CustomerId = customer.CustomerId,
                ServiceId = service.ServiceId,
                EmployeeId = employee.EmployeeId,
                VisitDate = new DateTime(2023, 12, 27, 18, 0, 0)
            };
            context.Visits.Add(visit);
            context.SaveChanges();

            Doljnost doljnost1 = new Doljnost
            {
                DoljnostName = "Администратор"
            };
            context.Doljnosts.Add(doljnost1);

            Doljnost doljnost2 = new Doljnost
            {
                DoljnostName = "Сотрудник"
            };
            context.Doljnosts.Add(doljnost2);

            context.SaveChanges();

            string[] roles = new string[] { "Administrator", "Guest" };
            foreach (string role in roles)
            {
                CreateRole(serviceProvider, role);
            }

            CustomUser customUser1 = new() { Surname = "Alekseev", Name = "Aleksei", Midname = "Alekseevich", UserName = "alekseev@mail.ru", Email = "alekseev@mail.ru", DoljnostId = doljnost1.DoljnostId };

            AddUserToRole(serviceProvider, "Password123!", "Administrator", customUser1);

            CustomUser customUser2 = new() { Surname = "Ivanov", Name = "Ivan", Midname = "Ivanovich", UserName = "ivanov@mail.ru", Email = "ivanov@mail.ru", DoljnostId = doljnost2.DoljnostId };

            AddUserToRole(serviceProvider, "Password123!", "Guest", customUser2);
        }

        private static void CreateRole(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                roleResult.Wait();
            }

        }

        private static void AddUserToRole(IServiceProvider serviceProvider, string userPwd, string roleName, CustomUser customUser)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

            Task<CustomUser> checkAppUser = userManager.FindByEmailAsync(customUser.Email); ;

            checkAppUser.Wait();

            if (checkAppUser.Result == null)
            {
                Task<IdentityResult> taskCreateAppUser = userManager.CreateAsync(customUser, userPwd);

                taskCreateAppUser.Wait();

                if (taskCreateAppUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(customUser, roleName);
                    newUserRole.Wait();
                }
            }
        }
    }
}
