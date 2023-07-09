using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congratulator.Models
{
    public class UserDbModel : User
    {
        public UserDbModel() { }

        public UserDbModel(User user)
        {
            Update(user);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public void Update(User user)
        {
            Name = user.Name;
            Surname = user.Surname;
            BDate = user.BDate;
            Avatar = user.Avatar;
        }
    }
}