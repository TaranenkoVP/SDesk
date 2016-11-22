using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SDesk.Web.WebApi.Regexes
{
    /// <summary>
    /// 
    /// </summary>
    public class JiraRegex
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Regex Get()
        {
            return new Regex(@"^Jira-([1-9]\d*)$");
        }
    }
}