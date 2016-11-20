using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Epam.Sdesk.Model;
using SDesk.DAL.EF;
using SDesk.Web.WebApi.Regexes;

namespace SDesk.Web.WebApi.Controllers
{
    [RoutePrefix("api/jiraitems")]
    public class JiraitemsController : ApiController
    {
        private readonly IRepository<JiraItem> _jiraItemRepository;
        private readonly IUnitOfWork _unit;
        private readonly Regex _jiraRegex = new JiraRegex().Get();
        public JiraitemsController()
        {
            _unit = new UnitOfWork<SdeskContext>();
            _jiraItemRepository = _unit.GetRepository<JiraItem>();
        }

        // GET api/jiraitems/{id}
        // GET api/jiraitems must return the same as for GET api/jiraitems/1                  
        [Route("{id:long?}")]
        public IHttpActionResult Get(long id = 1)
        {
            //throw new NotImplementedException();
            var jiraItem = _jiraItemRepository.GetById(id);
            if (jiraItem == null)
            {
                return NotFound();
            }
            return Ok(jiraItem);
        }
        // GET by <Jira-Id : “Jira-1034”>
        //[Route(@"{id:regex(^Jira-([1-9]\d*)$)}")]
        
        // GET api/jiraitems/{id:jiraid}
        [Route("{id:jiraid}")]
        public IHttpActionResult Get(string id)
        {
            var match = _jiraRegex.Match(id);
            if (!match.Success)
            {
                return BadRequest();
            }
            long jiraId;
            if (match.Groups.Count != 2 || !long.TryParse(match.Groups[1].Value, out jiraId))
            {
                return BadRequest();
            }
            var jiraItem = _jiraItemRepository.GetById(jiraId);
            if (jiraItem == null)
            {
                return NotFound();
            }
            return Ok(jiraItem);
        }
    }
}
