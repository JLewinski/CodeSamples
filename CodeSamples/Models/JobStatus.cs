using CodeSamples.Extensions.DataAccess;
using CodeSamples.Helpers.DataAccess;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CodeSamples.Models
{
    public class JobStatus
    {
        public static JobStatus Create(SqlDataReader dataReader)
        {
            return new JobStatus(dataReader);
        }

        private JobStatus(SqlDataReader dataReader)
        {
            Id = dataReader.GetValue<int>(nameof(Id));
            Name = dataReader.GetValue<string>(nameof(Name));
            IsRunning = dataReader.GetValue<bool>(nameof(IsRunning));
            LastRunWasSuccessful = dataReader.GetValue<bool>(nameof(LastRunWasSuccessful));
            LastRunCompletionDate = dataReader.GetValue<DateTime?>(nameof(LastRunCompletionDate));
            LastDateCycle = dataReader.GetValue<DateTime?>(nameof(LastDateCycle));
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsRunning { get; set; }
        public bool LastRunWasSuccessful { get; set; }
        public DateTime? LastRunCompletionDate { get; set; }
        public DateTime? LastDateCycle { get; set; }
        /// <summary>
        /// A calculation to see the difference in time from now to the last time the job ran.
        /// </summary>
        public TimeSpan LastRunCompletionDateToNow
        {
            get
            {
                if (LastRunCompletionDate.HasValue)
                {
                    return DateTime.UtcNow - LastRunCompletionDate.Value;
                }
                return TimeSpan.Zero;
            }

        }

        public static Task<JobStatus> GetDMSJobStatus()
        {
            return StoredProcedureHelper.GetItemAsync("ps_JobStatus_GetDMSJobStatus", Create);
        }

        public static Task<bool> CheckSAPProcess(bool start)
        {
            return StoredProcedureHelper.GetItemAsync("ps_JobStatus_CheckSAPProcess", new { Start = start }, dataReader => dataReader.GetValue<bool>("CanStart"));
        }

        public static Task<JobStatus> GetByNameAsync(SqlConnection connection, string name)
        {
            return StoredProcedureHelper.GetItemAsync("JobStatus_GetByName", new { name }, Create, connection);
        }
        public static JobStatus GetByName(SqlConnection connection, string name)
        {
            return StoredProcedureHelper.GetItem("JobStatus_GetByName", new { name }, Create, connection);
        }

        public Task Update(SqlConnection connection)
        {
            var parameters = new
            {
                Id,
                Name,
                IsRunning,
                LastRunWasSuccessful,
                LastRunCompletionDate,
                LastDateCycle
            };
            return StoredProcedureHelper.ExecuteStoredProcedureAsync("JobStatus_Update", parameters, connection);
        }
    }
}
