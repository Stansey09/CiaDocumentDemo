using CIADocServer.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService documentService;
        public DocumentController(IDocumentService docService)
        {
            this.documentService = docService;
        }

        //TODO set this up to work with my browser client.

        [HttpGet]
        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            IEnumerable<string> titles = this.documentService.GetDocumentTitles();
            return titles;
        }
    }
}
