namespace BusinessLogicLayer.Dtos
{
    public class CreateAnswerCommentDto
    {
        public string Body { get; set; }

        public int GameId { get; set; }

        public int ParentCommentId { get; set; }
        
        public string Name { get; set; }

    }
}