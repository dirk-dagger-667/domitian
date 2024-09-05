namespace dominitian_ui.Models.Requests.Registration
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }

        public string Code { get; set; }
    }
}
