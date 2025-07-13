namespace Entity.Enums
{
    [Flags]
    public enum LogTarget
    {
        None = 0,
        Database = 1,
        File = 2,
        CloudWatch = 4,
        All = Database | File | CloudWatch
    }

    public enum LogType
    {
        Debugging = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Critical = 5,
    }

    public enum RecordStatus
    {
        None = 0,
        Active = 1,
        Inactive = 2,
        Archived = 3,
        Deleted = 4,
        Purged = 5
    }
}
