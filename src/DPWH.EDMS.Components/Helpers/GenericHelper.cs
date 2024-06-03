using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DPWH.EDMS.Components.Helpers
{
    public static class GenericHelper
    {
        public static string GetPriceFormat(dynamic value)
        {
            return (value.GetType() == typeof(float) || value.GetType() == typeof(double))
                ? string.Format("{0:N2}", value)
                : "Invalid Type";
        }
        public static List<Dto> GetListByDataSource<Dto>(ICollection<object> data)
        {
            var jsonResult = JsonConvert.SerializeObject(data);
            var listResult = JsonConvert.DeserializeObject<List<Dto>>(jsonResult)!.ToList();

            return listResult;
        }

        public static ObservableCollection<Dto> GetObservableCollectionByDataSource<Dto>(ICollection<object> data)
        {
            var jsonResult = JsonConvert.SerializeObject(data);
            var listResult = JsonConvert.DeserializeObject<List<Dto>>(jsonResult)!.ToList();

            return new ObservableCollection<Dto>(listResult);
        }
        public static T DeepCopy<T>(this T self)
        {
            var serialized = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<T>(serialized)!;
        }
        public static string GetDisplayValue(string? text, string emptyMessage = "N/A")
        {
            return !string.IsNullOrEmpty(text) ? text : emptyMessage;
        }
        public static string SetNullOrEmptyStringValue(string? text)
        {
            return !string.IsNullOrEmpty(text) ? text : string.Empty;
        }
        public static bool IsObjectFieldNotNull(object? obj, string fieldName)
        {
            return obj != null && obj.GetType().GetField(fieldName)?.GetValue(obj) != null;
        }
        public static bool IsObjectFieldEqualTo(object? obj, string fieldName, object value)
        {
            return obj?.GetType().GetProperty(fieldName)?.GetValue(obj)?.Equals(value) ?? false;
        }
        public static string GetDateTimeDisplay(DateTimeOffset? dt)
        {
            if (dt.HasValue)
            {
                var dtUtc = new DateTimeOffset(dt!.Value.DateTime, TimeSpan.Zero);
                var dtRes = dtUtc!.ToLocalTime();

                return dtRes.ToString("MM/dd/yyyy hh:mm tt");
            }

            return "N/A";
        }
        public static string GetDateNameAndTimeDisplay(DateTimeOffset? dt)
        {
            if (dt.HasValue)
            {
                var dtUtc = new DateTimeOffset(dt!.Value.DateTime, TimeSpan.Zero);
                var dtRes = dtUtc!.ToLocalTime();

                return dtRes.ToString("MMM dd, yyyy hh:mm tt");
            }

            return "N/A";
        }
        public static string GetUTCDateTimeDisplay(DateTimeOffset? dt)
        {
            if (dt.HasValue)
            {
                var dtUtc = new DateTimeOffset(dt!.Value.DateTime, TimeSpan.Zero);
                return dtUtc.ToString("MM-dd-yyyy hh:mm tt");
            }

            return "N/A";
        }
        public static string GetDateDisplay(DateTimeOffset? dt, string format = "MMM dd, yyyy")
        {
            if (dt.HasValue)
            {
                var dtUtc = new DateTimeOffset(dt!.Value.DateTime, TimeSpan.Zero);
                var dtRes = dtUtc!.ToLocalTime();

                return dtRes.ToString(format);
            }

            return "N/A";
        }
        public static string GetYearDisplay(DateTimeOffset? dt, string format = "yyyy")
        {
            if (dt.HasValue)
            {
                var dtUtc = new DateTimeOffset(dt!.Value.DateTime, TimeSpan.Zero);
                var dtRes = dtUtc!.ToLocalTime();

                return dtRes.ToString(format);
            }

            return "N/A";
        }
        public static string GetRelativeTime(DateTimeOffset dt)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
        public static List<string> GetFormErrorMessages<T>(T model)
        {
            var validationContext = new ValidationContext(model!);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model!, validationContext, validationResults, true);

            var invalidMessages = new List<string>();
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    invalidMessages.Add(validationResult.ErrorMessage!);
                }
            }

            return invalidMessages;
        }
        public static string GetFileSizeString(long fileSizeInBytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (fileSizeInBytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                fileSizeInBytes /= 1024;
            }
            return $"{fileSizeInBytes:0.##} {sizes[order]}";
        }

        public static string CapitalizeFirstLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return char.ToUpper(str[0]) + str.Substring(1);
        }

        //public static async Task<List<T>> GetListByQuery<T>(
        //    DataSourceRequest dReq, 
        //    Func<DataSourceRequest, Task<DataSourceResult>> ServiceCb, 
        //    Action<string> ErrCb
        //)
        //{
        //    try
        //    {
        //        var result = await ServiceCb(dReq);
        //        var items = GetListByDataSource<T>(result.Data);

        //        return items;
        //    }
        //    catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        //    {
        //        var problemDetails = apiExtension.Result;
        //        var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();

        //        if (ErrCb != null)
        //            ErrCb.Invoke(error ?? $"GetAllItemsByQuery<,> Something went wrong!");

        //        return new List<T>();
        //    }
        //}
    }
}
