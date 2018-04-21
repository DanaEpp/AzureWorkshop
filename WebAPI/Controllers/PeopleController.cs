using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureWorkshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureWorkshop.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        List<Person> people = People.GetPeople();

        // GET api/people
        // LAB1: Uncomment when using Retry handler
        //[ThrottleFilter(RequestLimit: 4, TimeoutInSeconds: 5)]
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return people;
        }

        // GET api/people/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return People.GetPeople().FirstOrDefault(p => p.Id == id);
        }
    }
}
