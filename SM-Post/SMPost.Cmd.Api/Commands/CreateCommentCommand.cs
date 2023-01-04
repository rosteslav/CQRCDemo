using SQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class CreateCommentCommand : BaseCommand
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
