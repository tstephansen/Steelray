using System.Collections.Generic;

namespace CodingAssessment.Models
{
    public class IdNode
    {
        public int Id { get; set; }
        public IdNode Parent { get; set; }
        public List<IdNode> Children { get; set; }
    }
}
