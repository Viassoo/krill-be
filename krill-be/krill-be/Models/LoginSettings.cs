namespace krill_be.Models
{
	public class LoginSettings : Model
	{
		public bool autoCreateUser { get; set; } = false;
		public List<string>	allowedEmailDomains { get; set; } =  new List<string>();

		public LoginSettings() { }
	}
}
