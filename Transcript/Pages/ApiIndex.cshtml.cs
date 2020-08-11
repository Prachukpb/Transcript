using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Transcript.Pages
{
    public class ApiIndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostChallenge1Async()
        {
            var id = Request.Form["studentid"];
            return Redirect("api/people/" + Int16.Parse(id));
        }
        public async Task<IActionResult> OnPostChallenge2Async()
        {
            return Redirect("api/people");
        }
        
    }
}
