namespace Group_3_BE.Services.MTask
{
    public class TaskMessage
    {
        public enum Information
        {

        }

        public enum Warning
        {

        }

        public enum Error
        {
            IdNotExisted,
            CodeEmpty,
            CodeExisted,
            CodeHasSpecialCharacter,
            CodeOverLength,
            NameEmpty,
            NameOverLength,
            DescriptionOverLength,
            StartDateNotExisted,
            FinishDateNotExisted,
            PercentageInvalid,
            ProjectEmpty,
            ProjectNotExisted,
            TaskTypeEmpty,
            TaskTypeNotExisted,
            StatusNotExisted,
            TaskInUsed
        }
    }
}
