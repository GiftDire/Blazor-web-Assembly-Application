namespace BlazorApp.Models
{
    public class RegisterDto
    {
        public string Email { get; set; } = "";///defualt value is an empty value= it ensure the value is never by default
        //it initialize the property with an empty string 
        public string Password { get; set; } = "";
    }
}
