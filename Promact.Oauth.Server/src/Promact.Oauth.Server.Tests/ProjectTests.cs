using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System;
using Promact.Oauth.Server.Models;
using System.Collections.Generic;
using Promact.Oauth.Server.Repository;
using System.Globalization;
using System.Threading.Tasks;
using Promact.Oauth.Server.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Promact.Oauth.Server.Tests
{

    public class ProjectTests : BaseProvider
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataRepository<Project> _dataRepository;
        private readonly IDataRepository<ProjectUser> _dataRepositoryProjectUser;
        private readonly IDataRepository<ApplicationUser> _dataRepositoryUser;
        private readonly IUserRepository _userRepository;
        private readonly PromactOauthDbContext context;

        public ProjectTests() : base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<Project>>();
            _dataRepositoryProjectUser = serviceProvider.GetService<IDataRepository<ProjectUser>>();
            _dataRepositoryUser = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            context = serviceProvider.GetService<PromactOauthDbContext>();
        }

        ProjectAc projectac = new ProjectAc()
        {
            Name = StringConstant.Name,
            SlackChannelName = StringConstant.SlackChannelName,
            IsActive = StringConstant.IsActive,
            TeamLeader = new UserAc { FirstName = StringConstant.FirstName },
            TeamLeaderId = StringConstant.TeamLeaderId,
            CreatedBy = StringConstant.CreatedBy

        };

        ProjectUser projectUser = new ProjectUser()
        {
            ProjectId = 1,
            Project = new Project { Name = StringConstant.Name },
            UserId = StringConstant.UserId,
            User = new ApplicationUser { FirstName = StringConstant.FirstName }
        };

        UserAc user = new UserAc()
        {

            FirstName = StringConstant.FirstName,
            LastName = StringConstant.LastName,
            Email = StringConstant.Email
        };
        UserAc userSecound = new UserAc()
        { Id = StringConstant.UserIdSecond, FirstName = StringConstant.FirstNameSecond };
        UserAc userThird = new UserAc()
        { Id = StringConstant.UserIdThird, FirstName = StringConstant.FirstNameThird };
        /// <summary>
        /// This test case to add a new project
        /// </summary>

        [Fact, Trait("Category", "Required")]
        public void AddProject()
        {
            Task<int> id = _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            var project = _dataRepository.FirstOrDefault(x => x.Id == id.Result);
            Assert.NotNull(project);
        }
        /// <summary>
        /// This test case for the add user and project in userproject table
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUserProject()
        {

            _projectRepository.AddUserProject(projectUser);
            var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(ProjectUser);
        }
        /// <summary>
        /// This test case for gets project By Id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetById()
        {

            //UserAc user = new UserAc()
            //{

            //    FirstName = "kuch b",
            //    LastName = "phir se",
            //    Email = "julie@promactinfo.vom"
            //};
            //var TId = _userRepository.AddUser(user, StringConstant.CreatedBy).Result;
            //projectac.TeamLeaderId = TId;
            //Task<int> id = _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            //_projectRepository.AddUserProject(projectUser);
            //Task<ProjectAc> project = _projectRepository.GetById(id.Result);
            //Assert.NotNull(project);

            AddRole();
            var TeamLeaderId = _userRepository.AddUser(user, StringConstant.CreatedBy).Result;
            projectac.TeamLeaderId = TeamLeaderId;
            Task<int> id = _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            _projectRepository.AddUserProject(projectUser);
            Task<ProjectAc> project = _projectRepository.GetById(id.Result);
            Assert.NotNull(project);
        }

        /// <summary>
        /// This test case edit project 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void EditProject()
        {
            _userRepository.AddUser(user, StringConstant.FirstName);
            _userRepository.AddUser(userSecound, StringConstant.FirstName);
            _userRepository.AddUser(userThird, StringConstant.FirstName);
            Task<int> id = _projectRepository.AddProject(projectac, StringConstant.FirstName);
            _projectRepository.AddUserProject(projectUser);
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(userSecound);
            userlist.Add(userThird);
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = id.Result,
                Name = StringConstant.EditName,
                SlackChannelName = StringConstant.SlackChannelName,
                IsActive = StringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = StringConstant.FirstName },
                TeamLeaderId = StringConstant.TeamLeaderId,
                CreatedBy = StringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.EditProject(projectacSecound, StringConstant.CreatedBy);
            var project = _dataRepository.Fetch(x => x.Id == 1);
            _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(project);
        }
        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void checkDuplicateNegative()
        {
            _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            var project = _projectRepository.checkDuplicate(projectac);
            Assert.Null(project.Name);
        }
        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void checkDuplicatePositive()
        {

            _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(userSecound);
            userlist.Add(userThird);
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 2,
                Name = StringConstant.ProjectName,
                SlackChannelName = StringConstant.SlackChannelName,
                IsActive = true,
                TeamLeader = new UserAc { FirstName = StringConstant.FirstName },
                TeamLeaderId = StringConstant.TeamLeaderId,
                CreatedBy = StringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.AddProject(projectacSecound, StringConstant.CreatedBy);
            var project = _projectRepository.checkDuplicate(projectacSecound);
            Assert.Null(project.Name);
        }
        /// <summary>
        /// This test case for the get all projects
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetAllProject()
        {

            var TId = _userRepository.AddUser(user, StringConstant.CreatedBy);
            _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            _projectRepository.AddUserProject(projectUser);
            Task<IEnumerable<ProjectAc>> projects = _projectRepository.GetAllProjects();
            Assert.NotNull(projects);
        }

        /// <summary>
        /// Fetches Users of the given Project Name(slack channel name)
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void GetProjectUserByGroupName()
        {
            AddRole();
            var TId = _userRepository.AddUser(user, StringConstant.CreatedBy).Result;
            _projectRepository.AddProject(projectac, StringConstant.CreatedBy);
            _projectRepository.AddUserProject(projectUser);
            // var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            var projectUsers = _projectRepository.GetProjectUserByGroupName(projectac.SlackChannelName);
            Assert.NotEqual(projectUsers.Count, 2);
        }

        /// <summary>
        /// Fetch the project of the given slack channel name
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void GetProjectByGroupName()
        {
            _projectRepository.AddProject(projectac, "Roshni");
            var project = _projectRepository.GetProjectByGroupName(projectac.SlackChannelName);
            Assert.NotEqual(null, project);
        }

        private void AddRole()
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" });
                roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });

                foreach (var role in roles)
                {
                    var roleExit = roleManager.RoleExistsAsync(role.Name).Result;
                    if (!roleExit)
                    {
                        context.Roles.Add(role);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}