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
        ProjectAc projectac = new ProjectAc()
        {
            Name = "slack",
            SlackChannelName = "SlackChannelName",
            IsActive = true,
            TeamLeader = new UserAc { FirstName = "Roshni" },
            TeamLeaderId = "0ddef180-2c61-41de-84f0-be7397c1f5f3",
            CreatedBy = "Roshni",
            CreatedDate = DateTime.Now.ToString()
        };
        ProjectUser projectUser = new ProjectUser()
        {
            ProjectId = 1,
            Project = new Project { Name = "Slack" },
            UserId = "1",
            User = new ApplicationUser { FirstName = "Ronak" }
        };
        UserAc user = new UserAc()
        {
            Id = "1",
            FirstName = "Ronak"
        };
        UserAc user1 = new UserAc()
        {
            Id = "2",
            FirstName = "Ronit"
        };
        UserAc user2 = new UserAc()
        {
            Id = "3",
            FirstName = "Raj"
        };



        [Fact]
        public void AddProject()
        {
            var id = _projectRepository.AddProject(projectac, "Ronak");
            var project = _dataRepository.FirstOrDefault(x => x.Id==id);
            Assert.NotNull(project);
        }

        [Fact]
        public void AddUserProject()
        {
            _projectRepository.AddUserProject(projectUser);
            var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(ProjectUser);
        }

        [Fact]
        public void GetById()
        {
            var id = _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            _projectRepository.AddUserProject(new ProjectUser
            {
                ProjectId = 1,
                Project = new Project { Name = "Slack" },
                UserId = "2",
                User = new ApplicationUser { FirstName = "Ronit" }
            });
            ProjectAc Pc = _projectRepository.GetById(id);
            Assert.NotNull(Pc);
        }

        
        [Fact]
        public void EditProject()
        {
            _userRepository.AddUser(user);
            _userRepository.AddUser(user1);
            _userRepository.AddUser(user2);
            var id = _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            ProjectUser projectUser2 = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = "Slack" },
                UserId = "2",
                User = new ApplicationUser { FirstName = "Ronit" }
            };


            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(new UserAc { Id = "2", FirstName = "Ronit" });
            userlist.Add(new UserAc { Id = "3", FirstName = "Raj" });
            ProjectAc projectac2 = new ProjectAc()
            {
                Id = id,
                Name = "slackEdit",
                SlackChannelName = "SlackChannelNameEdit",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "0ddef180-2c61-41de-84f0-be7397c1f5f3",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(),
                ApplicationUsers = userlist
            };

           

            _projectRepository.EditProject(projectac2, "Ronak");
            var project = _dataRepository.Fetch(x => x.Id == 1);
            var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(project);
        }

        [Fact]
        public void checkDuplicateNegative()
        {
             _projectRepository.AddProject(projectac, "Ronak");
           var project =_projectRepository.checkDuplicate(projectac);
            Assert.Null(project.Name);
        }

        [Fact]
        public void checkDuplicatePositive()
        {
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
                TeamLeaderId = "0ddef180-2c61-41de-84f0-be7397c1f5f3",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(),
                ApplicationUsers = userlist
            };
            var project = _projectRepository.checkDuplicate(projectac2);
            Assert.NotNull(project.Name);
        }

        [Fact]
        public void GetAllProject()
        {
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
                TeamLeaderId = "0ddef180-2c61-41de-84f0-be7397c1f5f3",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(),
                ApplicationUsers = userlist
            };
            _projectRepository.AddProject(projectac2, "Ronak");
            IEnumerable<ProjectAc> p = _projectRepository.GetAllProjects();
            Assert.NotNull(p);
        }



    }
}