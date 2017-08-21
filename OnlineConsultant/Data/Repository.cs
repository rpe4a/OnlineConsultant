using System.Data.Entity;
using System.Linq;

namespace OnlineConsultant.Data
{
    public interface IRepository<T> where T: class
    {
        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> All { get; }

        void Commit();
    }


    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbConsultantContext _context;

        public Repository(DbConsultantContext context)
        {
            _context = context;
        }

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> All { get { return _context.Set<T>().AsQueryable(); } }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}