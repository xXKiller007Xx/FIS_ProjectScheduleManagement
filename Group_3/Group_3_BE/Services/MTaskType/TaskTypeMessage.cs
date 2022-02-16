namespace Group_3_BE.Services.MTaskType
{
    public class TaskTypeMessage
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
            StatusNotExisted,
            TaskTypeInUsed
        }
    }
}
