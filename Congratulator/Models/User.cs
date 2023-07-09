using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congratulator.Models
{
    public class User
    {
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        public DateTime BDate { get; set; } = DateTime.Today;

        public string Avatar { get; set; }

    }
}