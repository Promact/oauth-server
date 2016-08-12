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

namespace Promact.Oauth.Server.Tests
{

    public class ProjectTests:BaseProvider
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataRepository<Project> _dataRepository;
        private readonly IDataRepository<ProjectUser> _dataRepositoryProjectUser;
        private readonly IDataRepository<ApplicationUser> _dataRepositoryUser;
        private readonly IUserRepository _userRepository;
        

        public ProjectTests():base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<Project>>();
            _dataRepositoryProjectUser = serviceProvider.GetService<IDataRepository<ProjectUser>>();
            _dataRepositoryUser = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
        }
        
       
      

        /// <summary>
        /// Add New Project
        /// </summary>

        [Fact, Trait("Category", "A")]
        public void AddProject()
        {
            ProjectAc projectac = new ProjectAc()
            {
                Name = "slack",
                SlackChannelName = "SlackChannelName",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni"
                
            };
            var id = _projectRepository.AddProject(projectac, "Ronak");
            var project = _dataRepository.FirstOrDefault(x => x.Id==id);
            Assert.NotNull(project);
        }
        /// <summary>
        /// Add User and Project in User Project Table
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void AddUserProject()
        {
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = "Slack" },
                UserId = "1",
                User = new ApplicationUser { FirstName = "Roshni" }
            };
            _projectRepository.AddUserProject(projectUser);
            var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(ProjectUser);
        }
        /// <summary>
        /// get project using Id
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void GetById()
        {
             
            ProjectAc projectac = new ProjectAc()
            {
                Name = "slack",
                SlackChannelName = "SlackChannelName",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Admin" ,LastName="Admin",Email="test@yahoo.com" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = "Slack" },
                UserId = "1",
                User = new ApplicationUser { FirstName = "Admin" }
            };
            UserAc user = new UserAc()
            {
                
                FirstName = "Admin1",
                LastName="test1",
                Email= "test131@yahoo.com"
            };
            var TId=_userRepository.AddUser(user, "Ronak");
            projectac.TeamLeaderId = TId;
            var id = _projectRepository.AddProject(projectac, "Ronak");
            
            _projectRepository.AddUserProject(projectUser);
         
            ProjectAc Pc = _projectRepository.GetById(id);
            Assert.NotNull(Pc);
        }

        /// <summary>
        /// Edit Project 
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void EditProject()
        {
            ProjectAc projectac = new ProjectAc()
            {
                Name = "slack",
                SlackChannelName = "SlackChannelName",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = "Slack" },
                UserId = "1",
                User = new ApplicationUser { FirstName = "Roshni" }
            };
            UserAc user = new UserAc()
            {
                Id = "1",
                FirstName = "Roshni"
            };
            UserAc userSecound = new UserAc()
            {
                Id = "2",
                FirstName = "Ronit"
            };
            UserAc userThird = new UserAc()
            {
                Id = "3",
                FirstName = "Raj"
            };
            _userRepository.AddUser(user,"Ronak");
            _userRepository.AddUser(userSecound, "Ronak");
            _userRepository.AddUser(userThird, "Ronak");
            var id = _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            


            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(new UserAc { Id = "2", FirstName = "Ronit" });
            userlist.Add(new UserAc { Id = "3", FirstName = "Raj" });
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = id,
                Name = "slackEdit",
                SlackChannelName = "SlackChannelNameEdit",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.EditProject(projectacSecound, "Ronak");
            var project = _dataRepository.Fetch(x => x.Id == 1);
            _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(project);
        }
        /// <summary>
        /// Check Duplicate Project
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void checkDuplicateNegative()
        {
            ProjectAc projectac = new ProjectAc()
            {
                Name = "slack",
                SlackChannelName = "SlackChannelName",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            _projectRepository.AddProject(projectac, "Ronak");
           var project =_projectRepository.checkDuplicate(projectac);
            Assert.Null(project.Name);
        }

        [Fact, Trait("Category", "A")]
        public void checkDuplicatePositive()
        {
            ProjectAc projectac = new ProjectAc()
            {
                Name = "slack",
                SlackChannelName = "SlackChannelName",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            _projectRepository.AddProject(projectac, "Ronak");
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(new UserAc { Id = "2", FirstName = "Ronit" });
            userlist.Add(new UserAc { Id = "3", FirstName = "Raj" });
            ProjectAc projectac2 = new ProjectAc()
            {
                Id = 2,
                Name = "slackEdit",
                SlackChannelName = "SlackChannelNameEdit",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            var project = _projectRepository.checkDuplicate(projectac2);
            Assert.Null(project.Name);
        }
        /// <summary>
        /// Get All Projects
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void GetAllProject()
        {
            UserAc user = new UserAc()
            {

                FirstName = "Admin",
                LastName = "test",
                Email = "test13@yahoo.com"
            };
            var TId = _userRepository.AddUser(user, "Ronak");
            ProjectAc projectac = new ProjectAc()
            {
                Name = "slack",
                SlackChannelName = "SlackChannelName",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Admin", LastName = "test", Email = "test13@yahoo.com" },
                TeamLeaderId = TId,
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = "Slack" },
                UserId = "1",
                User = new ApplicationUser { FirstName = "Admin" }
            };
            _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            IEnumerable<ProjectAc> p = _projectRepository.GetAllProjects();
            Assert.NotNull(p);
        }



    }
}