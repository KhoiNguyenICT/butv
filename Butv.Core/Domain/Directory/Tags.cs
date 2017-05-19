using System.ComponentModel.DataAnnotations;
namespace BUTV.Core.Domain.Directory
{
    public class Tags : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int Hit { get; set; }
    }
}
