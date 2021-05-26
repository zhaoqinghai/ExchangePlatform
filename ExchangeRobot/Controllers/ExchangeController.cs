using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRobot.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ExchangeRobot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        public void SetExchangeSymbolList(IEnumerable<SymbolModel> symbols, ExchangeType type)
        {

        }

        public void GetExchangeAllSymbolList(ExchangeType type)
        {

        }

        public void GetExchangeSymbolList(ExchangeType type)
        {

        }
    }

    
}
