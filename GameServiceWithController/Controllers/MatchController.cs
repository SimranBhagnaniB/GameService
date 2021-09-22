using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameServiceWithController.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameServiceWithController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchFactory _matchFactory;
        public MatchController(IMatchFactory matchService)
        {
            _matchFactory = matchService;
        }


        [HttpGet("GetNewMatchForTwoPlayerGame")]
        public Guid GetNewMatchForTwoPlayerGame()
        {
            return _matchFactory.NewMatchForGameType(Models.GameType.TWOPLAYER);
        }

       

    }
}
