using System.ComponentModel.DataAnnotations;

namespace CrudSystem.API.Model
{
    public class AuditTrail
    {

        public string  UserName { get; set; }
        public string UserEmail { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Changes { get; set; }

    }

}
