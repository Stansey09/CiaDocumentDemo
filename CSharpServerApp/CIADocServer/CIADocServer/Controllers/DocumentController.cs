using CIADocServer.Domain.Models;
using CIADocServer.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CIADocServer.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentService docService, ICensorshipRulesDataStore cdStore)
        {
            this.documentService = docService;
        }

        //TODO set this up to work with my browser client.

        [HttpGet("api/Documents")]
        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            IEnumerable<string> titles = this.documentService.GetDocumentTitles();
            return titles;
        }

        [HttpGet("api/Documents/{title}")]
        public async Task<string> GetCensoredText(string title)
        {
            string censoredText = this.documentService.GetCensoredText(title);
            return censoredText;
        }

        [HttpPost("api/Documents")]
        public void PostNewDocument([FromBody] ClassifiedDocument doc)
        {
            this.documentService.AddDocument(doc);
            return;
        }

    }
}
