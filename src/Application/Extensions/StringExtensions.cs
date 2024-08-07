using Application.Constants;
using Domain.Enums;
using System.Text.RegularExpressions;

namespace Application.Extensions
{
    public static class StringExtensions
    {        
        public static bool IsBookStatusAllowed(this string bookStatus)
        {
            return Enum.TryParse(typeof(BookStatus), bookStatus, true, out var parsedBookStatus) &&
                 AppConstants.AllowedBookStatuses.Contains((BookStatus)parsedBookStatus);
        }

        public static bool IsBookStatus(this string bookStatus)
        {
            return Enum.TryParse(typeof(BookStatus), bookStatus, true, out _);
        }
    }
}
