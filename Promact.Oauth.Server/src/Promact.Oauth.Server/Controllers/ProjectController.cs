using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly PromactOauthDbContext _appDbContext;
        public ProjectController(PromactOauthDbContext appContext)
        {
            _appDbContext = appContext;
        }

        // GET: api/values
        [HttpGet]
        [Route("projects")]
        public IEnumerable<ProjectAc> Get()
        {
            //var projects = _appDbContext.Projects?.ToList();
            //var projectAcs = new List<ProjectAc>();
            //projects?.ForEach(x =>
            //{
            //    projectAcs.Add(new ProjectAc
            //    {
            //        Id=x.Id,
            //        Name = x.Name,
            //        SlackChannelName=x.SlackChannelName,
            //        IsActive=x.IsActive

            //    });
            //});
            return new List<ProjectAc>
            {
                new ProjectAc {
                    Id=1,
                    Name="Huddle",
                    SlackChannelName="test",
                    IsActive=true
                },
                new ProjectAc {
                    Id=2,
                    Name="Whiteboard",
                    SlackChannelName="test1",
                    IsActive=true

                }
            };//projectAcs;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("addProject")]
        public IActionResult Post([FromBody]ProjectAc project)
        {
            //if (_appDbContext.Projects == null) _appDbContext.Projects = new DbSet<Models.Project>();
            _appDbContext.Projects.Add(new Models.Project
            {
                Name = project.Name,
                SlackChannelName = project.SlackChannelName,
                IsActive=project.IsActive
                
            });
            _appDbContext.SaveChanges();
            return RedirectToAction("Get");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deleteProject/{id}")]
        public IActionResult Delete(int id)
        {
            _appDbContext.Projects.Remove(new Models.Project { Id=id});
            _appDbContext.SaveChanges();
            return RedirectToAction("Get");
        }
    }
}
