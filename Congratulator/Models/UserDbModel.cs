using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congratulator.Models
{
    public class UserDbModel : User
    {
        public UserDbModel() { }

        public UserDbModel(User user)
        {
            this.Update(user);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public void Update(User user)
        {
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.BDate = user.BDate;
            this.Avatar = user.Avatar;
        }
    }
}