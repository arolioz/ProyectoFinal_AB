using System.Linq.Expressions;
public class ProductBLL
{
    private Context _context;
    public ProductBLL(Context context)
    {	
        _context = context;
    }
    public bool Exist(int ProductId)
    {
        return _context.Products.Any(prd => prd.ProductId == ProductId);
    }
    public bool Insert(Product product)
    {
        bool changes = false;
        _context.Products.Add(product);
        changes = _context.SaveChanges() > 0;
        _context.Products.Entry(product).State = EntityState.Detached;
        return changes;
    }
    public bool Modify(Product product)
    {
        bool changes = false;
        _context.Entry(product).State = EntityState.Modified;
        changes =  _context.SaveChanges() > 0;
        _context.Products.Entry(product).State = EntityState.Detached;
        return changes;
    }
    public bool Save(Product product)
    {
        try{
            if(!Exist(product.ProductId))
                return Insert(product);
            else
                return Modify(product);
        }
        catch(Exception e)
        {
            System.Console.WriteLine($"Error {e}");
            return false;
        }   
    }
    public bool Delete(Product product)
    {
        bool changes = false;
        try{
            _context.Entry(product).State = EntityState.Deleted;
            changes = _context.SaveChanges() > 0;
            _context.Products.Entry(product).State = EntityState.Detached;
            return changes;
        }
        catch(Exception){
            return false;
        }
    }
    public Product? Search(int productId)
    {
        return _context.Products.Where(prd => prd.ProductId == productId).AsNoTracking().SingleOrDefault();
    }
    public List<Product> GetList(Expression<Func<Product,bool>> criterion)
    {
        return _context.Products.AsNoTracking().Where(criterion).ToList();
    }
}