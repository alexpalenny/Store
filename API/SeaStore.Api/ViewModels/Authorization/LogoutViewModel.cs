using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SeaStore.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
