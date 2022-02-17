namespace Group_3_BE.Services.MProject
{
    public class ProjectMessage
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
            StatusNotExisted,
            ProjectInUsed
        }
    }
}
