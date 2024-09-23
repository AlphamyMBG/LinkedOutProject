using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackendApp.Model;
using BackendApp.Data;
using BackendApp.Model.Requests;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using BackendApp.Model.Enums;
using BackendApp.auth;
using Utilities;
using Util;

namespace BackendApp.Service
{
    public interface ITimelineService{
        Post[] GetPostTimelineForUser(RegularUser user, int skip, int take);
        Post[] GetPostTimelineForUser(RegularUser user);
    }
    public class TimelineService(
        ApiContext context,
        IRecommendationService recommendationService
    ) : ITimelineService
    {  
        private readonly ApiContext context = context;
        private readonly IRecommendationService recommendationService = recommendationService;


        public Post[] GetPostTimelineForUser(RegularUser user, int skip, int take)
        {
            var result = this
                .CreateQueryForPostsOfConnectedUsers(user)
                .Skip(skip)
                .Take(take);


            return [.. result];
        }

        public Post[] GetPostTimelineForUser(RegularUser user)
        {
            return [.. this.CreateQueryForPostsOfConnectedUsers(user)];
        }

        private IQueryable<Post> CreateQueryForPostsOfConnectedUsers(RegularUser user)
        {
            return context.Posts
                .Where( 
                    post => 
                        context.Connections
                        .Where( 
                            con =>
                                con.Accepted && 
                                (con.SentBy == user || con.SentTo == user) 
                        )
                        .Select( con => con.SentBy == user ? con.SentTo : con.SentBy )
                        .Contains( post.PostedBy )
                );
        }
    }
}




