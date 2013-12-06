using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SalesApplication.Api
{
    /// <summary>
    /// An lightweight ApiController useful for development verification/testing
    /// </summary>
    public class BugsController : ApiController
    {
        // TODO: implement 
        List<string> _bugs = new List<string>();

        public BugsController()
        {
            _bugs.Add("BugA");
            _bugs.Add("BugB");
        }

        public IEnumerable<string> Get()
        {
            return _bugs;
        }

        [Route("api/bugs/")]
        public IEnumerable<string> GetBugs([FromBody] string id)
        {
            return Get();
        }

        #region    Hidden
        //[Route("api/customers/backlog")]
        //public Customer MoveToBacklog([FromBody] string id)
        //{
        //    var customer = _customerRepository.GetCustomerById(id);
        //    // TODO - set some variable or alter the customer
        //    // customer.state = "backlog";
        //    return customer;
        //}
        //[Route("api/customers/working")]
        //public Customer MoveToWorking([FromBody] string id)
        //{
        //    var customer = _customerRepository.GetCustomerById(id);
        //    // TODO - set some variable or alter the customer
        //    //customer.state = "working";
        //    return customer;
        //}
        //[Route("api/customers/done")]
        //public Customer MoveToDone([FromBody] string id)
        //{
        //    var customer = _customerRepository.GetCustomerById(id);
        //    // TODO - set some variable or alter the customer
        //    // customer.state = "done";
        //    return customer;
        //}
        #endregion Hidden
    }
}
