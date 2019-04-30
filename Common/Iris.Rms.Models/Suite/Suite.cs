using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models.Suite
{
    public class Suite
    {
        [Key]
        public Guid SuiteId { get; set; }

        public string RoomNumber { get; set; }
        public List<Zone> Zones { get; set; }
        public Privacy PrivacyStatus { get; set; }

    }
}
