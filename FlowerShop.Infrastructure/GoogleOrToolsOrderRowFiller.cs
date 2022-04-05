using FlowerShop.Domain;
using Google.OrTools.LinearSolver;

namespace FlowerShop.Infrastructure;

public class GoogleOrToolsOrderRowFiller : IOrderRowFiller, IDisposable
{
    private readonly Solver _solver;
    
    public GoogleOrToolsOrderRowFiller()
    {
        _solver = Solver.CreateSolver("SCIP");
    }
    
    public IEnumerable<Tuple<Bundle, int>> Fill(Order.Row orderRow, IEnumerable<Bundle> availableBundles)
    {
        var bundlesAndBundleCountVariables = availableBundles
            .Select(bundle => new Tuple<Bundle, Variable>(bundle, CreateVariable(bundle)))
            .ToList();

        var totalProductQuantityExpression = PrepareTotalProductQuantityExpression(bundlesAndBundleCountVariables);
        _solver.Add(totalProductQuantityExpression == orderRow.Quantity);
        
        var totalBundlesCountExpression = PrepareTotalBundlesCountExpression(bundlesAndBundleCountVariables);
        _solver.Minimize(totalBundlesCountExpression);

        var resultStatus = _solver.Solve();
        
        if (resultStatus == Solver.ResultStatus.INFEASIBLE)
            throw new NoFillingBundleConfigurationExistsException(orderRow, availableBundles);

        return bundlesAndBundleCountVariables
            .Where(bundleAndBundleCountVariable => bundleAndBundleCountVariable.Item2.SolutionValue() > 0)
            .Select(bundleAndBundleCountVariable => new Tuple<Bundle, int>(
                bundleAndBundleCountVariable.Item1,
                Convert.ToInt32(bundleAndBundleCountVariable.Item2.SolutionValue())
            ));
    }

    private Variable CreateVariable(Bundle bundle)
    {
        return _solver.MakeIntVar(
            0.0,
            double.PositiveInfinity,
            $"{bundle.ProductCode}_{bundle.Quantity}"
        );
    }

    private LinearExpr PrepareTotalProductQuantityExpression(List<Tuple<Bundle, Variable>> bundlesAndBundleCountVariables)
    {
        var totalProductQuantityExpression = new LinearExpr();
        
        foreach (var bundleAndBundleCountVariable in bundlesAndBundleCountVariables)
        {
            var bundle = bundleAndBundleCountVariable.Item1;
            var bundleCountVariable = bundleAndBundleCountVariable.Item2;

            totalProductQuantityExpression += (bundle.Quantity * bundleCountVariable);
        }

        return totalProductQuantityExpression;
    }

    private LinearExpr PrepareTotalBundlesCountExpression(List<Tuple<Bundle, Variable>> bundlesAndBundleCountVariables)
    {
        var totalBundlesCountExpression = new LinearExpr();
        
        foreach (var bundleAndBundleCountVariable in bundlesAndBundleCountVariables)
        {
            var bundleCountVariable = bundleAndBundleCountVariable.Item2;

            totalBundlesCountExpression += bundleCountVariable;
        }

        return totalBundlesCountExpression;
    }

    public void Dispose()
    {
        _solver.Dispose();
    }
}