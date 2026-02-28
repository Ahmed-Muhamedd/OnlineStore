using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos
{
    public class AuthRegisterDto
    {
        [Required, MinLength(3) , MaxLength(100)]
        public string FirstName { set; get; } = null!;

        [Required, MinLength(3) , MaxLength(100)]
        public string SecondName { set; get; } = null!;

        [Required , Phone]
        public string PhoneNumber { set; get; } = null!;

        [Required , EmailAddress]
        public string Email { set; get; } = null!;

        [Required]
        public string Password { set; get; } = null!;
        [Required , MinLength(4) , MaxLength(40)]
        public string Username { set; get; } = null!;
        [Required]
        public bool IsCustomer { set; get; }

    }
}
