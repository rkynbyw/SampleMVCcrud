using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SampleMVC.Views.Shared
{
    public class ValidationScript : PageModel
    {
        private readonly ILogger<ValidationScript> _logger;

        public ValidationScript(ILogger<ValidationScript> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}