using Post.Common.Events;
using SQRS.Core.Domain;
using SQRS.Core.Events;
using SQRS.Core.Messages;
using System.Collections.Generic;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        { 
            get => _active; 
            set => _active = value; 
        }

        public PostAggregate()
        { }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }

        public override void Apply(BaseEvent @event)
        {
            _id = @event.Id;
            switch (@event)
            {
                case PostCreatedEvent postCreatedEvent:
                    _active = true;
                    _author = postCreatedEvent.Author;
                    break;
                case CommentAddedEvent commentAddedEvent:
                    _comments.Add(
                        commentAddedEvent.CommentId,
                        Tuple.Create(commentAddedEvent.Comment, commentAddedEvent.UserName));
                    break;
                case CommentUpdatedEvent commentUpdatedEvent:
                    _comments[commentUpdatedEvent.CommentId] = Tuple.Create(
                        commentUpdatedEvent.Comment,
                        commentUpdatedEvent.UserName);
                    break;
                case CommentRemoveEvent commentRemoveEvent:
                    _comments.Remove(commentRemoveEvent.CommentId);
                    break;
                case PostRemovedEvent postRemovedEvent:
                    _active = false;
                    break;
                default:
                    break;
            }
        }

        public void EditMessage(string message)
        {
            if (!_active) 
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null of empty!");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message
            });
        }


        public void LikeMessage()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }

            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add comment of an inactive post");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null of empty!");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                UserName = username,
                CommentDate = DateTime.Now
            });
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit comment of an inactive post");
            }

            if (!_comments.ContainsKey(commentId) ||
                !_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed ot edit this coment");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                UserName = username,
                EditDate = DateTime.Now
            });
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive post");
            }

            if (!_comments.ContainsKey(commentId) ||
                !_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed ot remove this coment");
            }

            RaiseEvent(new CommentRemoveEvent
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You post is already removed");
            }

            if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You cannot remove someone elses comment");
            }

            RaiseEvent(new PostRemovedEvent
            { 
                Id = _id
            });
        }
    }
}
