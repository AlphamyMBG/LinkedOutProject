using System.Collections;
using System.Security.Cryptography.X509Certificates;
using BackendApp.Model;
using BackendApp.Util;

namespace BackendApp.Service;



public interface IRecommendationService
{
    public JobPost[] RecommendJobs(RegularUser user, uint top);
}

public class RecommendationService
(
    IRegularUserService regularUserService,
    IConnectionService connectionService,
    IInterestService interestService,
    ILinkedOutJobService jobService,
    ILinkedOutPostService postService
)
: IRecommendationService
{
    private readonly IRegularUserService regularUserService = regularUserService;
    private readonly IConnectionService connectionService = connectionService;
    private readonly IInterestService interestService = interestService;
    private readonly ILinkedOutJobService jobService = jobService;
    private readonly ILinkedOutPostService postService = postService;

    public JobPost[] RecommendJobs(RegularUser user, uint top)
    {
        
        throw new NotImplementedException(); //TODO: Implement Matrix Factorization Algorithm here
    }

    public double[,] MatrixFactorization
    (
        double[,] dataMatrix, 
        uint latentFeatures,
        double learningRate,
        bool normalize = false
    )
    {
        if(normalize) throw new NotImplementedException();
        uint numberOfUsers = (uint)dataMatrix.GetLongLength(0);
        uint numberOfProducts = (uint)dataMatrix.GetLongLength(1);
        var initialMatrixMin = MatrixOperations.MatrixMin(dataMatrix); 
        var initialMatrixMax = MatrixOperations.MatrixMin(dataMatrix);

        double[,] V = MatrixOperations.RandomDoubleMatrix(
            numberOfUsers, 
            latentFeatures, 
            initialMatrixMin, 
            initialMatrixMax
        );
        double[,] F = MatrixOperations.RandomDoubleMatrix(
            latentFeatures, 
            numberOfProducts, 
            initialMatrixMin, 
            initialMatrixMax
        );
        
        bool keepGoing = true;
        double[,] apprMatrix = new double[numberOfUsers, numberOfProducts];
        while(keepGoing)
        {
            apprMatrix = MatrixOperations.MultiplyMatrices(V, F);
            var error = MatrixOperations.SquareError(dataMatrix, apprMatrix);
        }

        return apprMatrix;
    }

    private double[,] CreateInitialMatrix(IEnumerable<RegularUser> users, IEnumerable<JobPost> posts)
    {
        double[,] matrix = new double[users.Count(), posts.Count()];
        foreach(var (user, userIndex) in users.Select( (user, index) => (user,index) ))
        {
            foreach(var (post, postIndex) in posts.Select( (post, index) => (post,index) ))
            {
                matrix[userIndex, postIndex] = this.DetermineMatrixCellValue(user, post);
            }
        }
        return matrix;
    }

    private double DetermineMatrixCellValue(RegularUser user, PostBase post)
    {
        double value = 0;
        if(post.InterestedUsers.Contains(user)) value += 5;
        var connectedUsers = this.connectionService.GetUsersConnectedTo(user);
        foreach(var connectedUser in connectedUsers)
        {
            if(post.InterestedUsers.Contains(connectedUser)) value += 1;
        }
        return value;
    }



}