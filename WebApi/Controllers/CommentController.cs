using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/comment")]
    public class CommentController : Controller
    {
        private readonly ICommentService gameService;

        public CommentController(ICommentService gameService)
        {
            this.gameService = gameService;
        }
        

   
        // GET: api/Comment/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Comment
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
    
    }
}
