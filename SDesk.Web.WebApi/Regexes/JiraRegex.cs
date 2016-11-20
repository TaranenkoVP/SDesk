using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SDesk.Web.WebApi.Regexes
{
    public class JiraRegex
    {
        public Regex Get()
        {
            return new Regex(@"^Jira-([1-9]\d*)$");
        }
    }
}