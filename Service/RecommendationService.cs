using System.Collections;
using System.Security.Cryptography.X509Certificates;
using BackendApp.Model;
using BackendApp.Util;

namespace BackendApp.Service;



public interface IRecommendationService
{
    public JobPost[] RecommendJobs(RegularUser user, int top);
}

public class RecommendationService
(
    IRegularUserService regularUserService,
    IConnectionService connectionService,
    IInterestService interestService,
    IJobService jobService,
    ILinkedOutPostService postService
)
: IRecommendationService
{
    private readonly IRegularUserService regularUserService = regularUserService;
    private readonly IConnectionService connectionService = connectionService;
    private readonly IInterestService interestService = interestService;
    private readonly IJobService jobService = jobService;
    private readonly ILinkedOutPostService postService = postService;

    public JobPost[] RecommendJobs(RegularUser user, int top)
    {
        var users = this.regularUserService.GetAllUsers();
        var jobs = this.jobService.GetAllJobs();
        var userIndex = users.ToList().IndexOf(user);
        if(userIndex == -1) return [];
        
        double[,] dataMatrix = this.CreateInitialMatrix(users, jobs);
        double[,] matrixApproximation = this.MatrixFactorization(dataMatrix, 100, 0.005);
        double[] jobRow = MatrixOperations.GetRow(matrixApproximation, userIndex);

        JobPost[] jobsSelected = jobRow
            .Select( (jobRating, jobIndex) => (jobRating, job: jobs[jobIndex]))
            .Where( pair => !pair.job.InterestedUsers.Contains(user) )
            .OrderByDescending( x => x.jobRating )
            .Select( x => x.job )
            .Take(top)
            .ToArray();
        
        return jobsSelected;
    }

    public double[,] MatrixFactorization
    (
        double[,] dataMatrix, 
        uint latentFeatures,
        double learningRate,
        bool normalize = false,
        ulong maxIterations = 0
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
        double error = 0;
        ulong iterations = 0;
        while(keepGoing)
        {
            double[,] apprMatrix = MatrixOperations.Multiply(V, F);
            var errorMatrix = MatrixOperations.Subtract(dataMatrix, apprMatrix);
            
            var newV = new double[numberOfUsers, latentFeatures];
            var newF = new double[latentFeatures, numberOfProducts];
            for(var (i, j)  = (0, 0); i < numberOfUsers && j < numberOfProducts; i++, j++){
                for(var k = 0; k < latentFeatures; k++){
                    newV[i, k] = V[i,k] + learningRate * 2 * errorMatrix[i,j] * F[k,j];
                    newF[k, j] = F[k,j] + learningRate * 2 * errorMatrix[i,j] * V[i,k];
                }
            }
            V = newV;
            F = newF;
            var newError = errorMatrix.Cast<double>().Select(a => a * a).Sum();
            iterations++;
            if(error <= newError || iterations == maxIterations) keepGoing = false;
    
        }
        double[,] finalApproximation = MatrixOperations.Multiply(V,F);
        return finalApproximation; 
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